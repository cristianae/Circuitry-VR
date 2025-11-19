using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class WiringPool : MonoBehaviour
{
    private List<List<GameObject>> wired = new List<List<GameObject>>();
    private LineRenderer lineRender;
    private GameObject controller;
    private bool pending;
    private List<LineRenderer> wirePool = new List<LineRenderer>();
    void Start()
    {
        pending = false;
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < wirePool.Count; i++)
        {
            wirePool[i].SetPosition(0, wired[i][0].transform.position);
            wirePool[i].SetPosition(1, wired[i][1].transform.position);
        }
    }

    private void createWire()
    {
        wirePool.Add(new GameObject().AddComponent<LineRenderer>());
        LineRenderer line = wirePool[wirePool.Count - 1];
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startColor = Color.red;
        line.endColor = Color.green;
        line.startWidth = 0.05f;
        line.endWidth = 0.05f;
        line.positionCount = 2;
    }

    public void addWired(ActivateEventArgs args)
    {
        if (!pending)
        {
            // Add selected object and controller to 
            // Create new lineRenderer for entry
            createWire();
            List<GameObject> temp = new List<GameObject>();
            temp.Add(args.interactableObject.transform.gameObject); // Entry 0
            temp.Add(args.interactorObject.transform.gameObject); // Entry 1
            wired.Add(temp);

            // Initalize line renderer positions
            wirePool[wirePool.Count - 1].SetPosition(0, args.interactableObject.transform.position);
            wirePool[wirePool.Count - 1].SetPosition(1, args.interactorObject.transform.position);
            controller = args.interactorObject.transform.gameObject;
            pending = true;
        }
        else
        {
            // Complete current pending object
            // Update existing line renderer
            wired[wired.Count - 1][1] = args.interactableObject.transform.gameObject;
            wirePool[wirePool.Count - 1].SetPosition(1, args.interactableObject.transform.position);
            wirePool[wirePool.Count - 1].SetPosition(0, wired[wired.Count - 1][0].transform.position);
            pending = false;
        }
    }
    public void removeWired(ActivateEventArgs args)
    {
        List<int> toRemove = new List<int>();
        for(int i = 0; i < wirePool.Count; i++)
        {
            // Search connected list; generate removal indices
            if(wired[i][0] == args.interactableObject.transform.gameObject || wired[i][1] == args.interactableObject.transform.gameObject)
            {
                toRemove.Add(i);
            }
        }

        // If pending connection and the final object is to be removed, reset pending
        if(pending && toRemove[toRemove.Count - 1] == wirePool.Count - 1)
        {
            pending = false;
        }
        // Remove line renderers and object connection from lists
        for(int j = toRemove.Count - 1; j >= 0; j--)
        {
            Destroy(wirePool[toRemove[j]]);
            wirePool.RemoveAt(toRemove[j]);
            wired.RemoveAt(toRemove[j]);
        }
    }
    public List<List<GameObject>> getWireConnections()
    {
        return wired;
    }
}