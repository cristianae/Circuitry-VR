using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Renderer))]
public class LEDLight : MonoBehaviour
{
    [Header("Electrical")]
    public float forwardVoltage = 2.0f;

    [Header("References")]
    public Battery9V battery;          // assign in Inspector
    public Renderer emissiveRenderer;  // assign LED body renderer (optional)

    [Header("Visuals")]
    public Color onColor = Color.red;
    public float emissionIntensity = 3f;

    Material matInstance;
    bool isOn;

    void Awake()
    {
        var rend = emissiveRenderer != null ? emissiveRenderer : GetComponent<Renderer>();
        matInstance = rend.material; // unique per LED
    }

    void OnEnable()
    {
        HookSockets(true);
    }

    void OnDisable()
    {
        HookSockets(false);
    }

    void HookSockets(bool subscribe)
    {
        if (battery == null) return;
        var pos = battery.positiveSocket;
        var neg = battery.negativeSocket;

        if (pos != null)
        {
            if (subscribe)
            {
                pos.selectEntered.AddListener(_ => Evaluate());
                pos.selectExited.AddListener(_ => Evaluate());
            }
            else
            {
                pos.selectEntered.RemoveListener(_ => Evaluate());
                pos.selectExited.RemoveListener(_ => Evaluate());
            }
        }

        if (neg != null)
        {
            if (subscribe)
            {
                neg.selectEntered.AddListener(_ => Evaluate());
                neg.selectExited.AddListener(_ => Evaluate());
            }
            else
            {
                neg.selectEntered.RemoveListener(_ => Evaluate());
                neg.selectExited.RemoveListener(_ => Evaluate());
            }
        }
    }

    void Start()
    {
        Evaluate();
    }

    void Evaluate()
    {
        if (battery == null)
        {
            SetOff();
            return;
        }

        if (!battery.HasCompleteCircuit(out var posSel, out var negSel))
        {
            SetOff();
            return;
        }

        // both sockets must hold the same LED object
        bool sameObject = posSel.transform.root == negSel.transform.root;
        bool isThisLED = posSel.transform.root == transform.root;

        if (sameObject && isThisLED && battery.voltage >= forwardVoltage)
            SetOn();
        else
            SetOff();
    }

    void SetOn()
    {
        if (isOn) return;
        isOn = true;
        matInstance.EnableKeyword("_EMISSION");
        matInstance.SetColor("_EmissionColor",
            onColor * Mathf.LinearToGammaSpace(emissionIntensity));
        if (matInstance.HasProperty("_Color"))
            matInstance.SetColor("_Color", onColor * 0.6f);
        DynamicGI.SetEmissive(emissiveRenderer != null ? emissiveRenderer : GetComponent<Renderer>(),
            onColor * emissionIntensity);
    }

    void SetOff()
    {
        if (!isOn) return;
        isOn = false;
        matInstance.SetColor("_EmissionColor", Color.black);
        DynamicGI.SetEmissive(emissiveRenderer != null ? emissiveRenderer : GetComponent<Renderer>(),
            Color.black);
    }
}
