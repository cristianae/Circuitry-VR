using System.Collections.Generic;
using UnityEngine;

public class CircuitCheck : MonoBehaviour
{
    [Header("Circuit References")]
    public WiringPool wiringPool;   // drag WiringPool here
    public GameObject batteryNode;  // drag Battery_GrabRoot (the battery's wiring object)

    [Header("Visual Feedback")]
    public Renderer emissiveRenderer;  // drag LED mesh renderer here
    public Color onColor = Color.red;
    public float emissionIntensity = 3f;

    private Material matInstance;
    private bool isOn;

    void Awake()
    {
        // If not assigned, try to find a renderer on this object or its children
        if (emissiveRenderer == null)
            emissiveRenderer = GetComponentInChildren<Renderer>();

        if (emissiveRenderer != null)
        {
            // Create a unique material instance for this LED
            matInstance = emissiveRenderer.material;
            matInstance.EnableKeyword("_EMISSION");
            SetOff(); // start off
        }
    }

    void Update()
    {
        if (!Ready())
        {
            SetOff();
            return;
        }

        if (CircuitIsComplete())
            SetOn();
        else
            SetOff();
    }

    bool Ready()
    {
        return wiringPool != null && batteryNode != null && matInstance != null;
    }

    /// <summary>
    /// Returns true if there is ANY wired path from this LED object to the batteryNode.
    /// </summary>
    bool CircuitIsComplete()
    {
        // Get all wire connections: each entry is [objectA, objectB]
        List<List<GameObject>> connections = wiringPool.getWireConnections();
        if (connections == null || connections.Count == 0)
            return false;

        // Build adjacency graph: who is wired to who
        Dictionary<GameObject, List<GameObject>> graph = new Dictionary<GameObject, List<GameObject>>();

        foreach (var pair in connections)
        {
            if (pair == null || pair.Count < 2) continue;

            GameObject a = pair[0];
            GameObject b = pair[1];

            if (a == null || b == null) continue;

            if (!graph.ContainsKey(a)) graph[a] = new List<GameObject>();
            if (!graph.ContainsKey(b)) graph[b] = new List<GameObject>();

            if (!graph[a].Contains(b)) graph[a].Add(b);
            if (!graph[b].Contains(a)) graph[b].Add(a);
        }

        GameObject ledNode = gameObject;

        // LED and battery both have to be present in the wiring graph
        if (!graph.ContainsKey(ledNode) || !graph.ContainsKey(batteryNode))
            return false;

        // BFS from LED to see if we can reach the battery at all
        HashSet<GameObject> visited = new HashSet<GameObject>();
        Queue<GameObject> queue = new Queue<GameObject>();

        visited.Add(ledNode);
        queue.Enqueue(ledNode);

        while (queue.Count > 0)
        {
            GameObject current = queue.Dequeue();

            if (current == batteryNode)
            {
                // Found a wired path from LED to battery -> treat as complete circuit
                return true;
            }

            if (!graph.ContainsKey(current))
                continue;

            foreach (GameObject neighbor in graph[current])
            {
                if (neighbor == null || visited.Contains(neighbor))
                    continue;

                visited.Add(neighbor);
                queue.Enqueue(neighbor);
            }
        }

        // Never reached the battery
        return false;
    }

    void SetOn()
    {
        if (matInstance == null) return;
        if (isOn) return;

        isOn = true;

        Color emissionColor = onColor * Mathf.LinearToGammaSpace(emissionIntensity);
        matInstance.SetColor("_EmissionColor", emissionColor);

        if (matInstance.HasProperty("_Color"))
            matInstance.SetColor("_Color", onColor);
    }

    void SetOff()
    {
        if (matInstance == null) return;
        if (!isOn && matInstance.GetColor("_EmissionColor") == Color.black) return;

        isOn = false;
        matInstance.SetColor("_EmissionColor", Color.black);
    }
}
