using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ElectricComponent : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable interactable;
    [SerializeField] WiringPool wiringPool;
    void Start()
    {
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (interactable != null){
            interactable.activated.AddListener(activateWire);
        }
    }
    void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        if (interactable != null)
        {
            interactable.activated.RemoveListener(activateWire);
        }
    }
    public void activateWire(ActivateEventArgs args)
    {
        wiringPool.addWired(args);
    }
}
