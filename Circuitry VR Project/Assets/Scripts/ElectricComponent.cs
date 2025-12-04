using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

//added
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ElectricComponent : MonoBehaviour
{
    //   private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable interactable;
    private XRBaseInteractable interactable;
    [SerializeField] WiringPool wiringPool;
    public string componentName;

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
        if(interactable.interactorsSelecting.Count > 0)
        {
            // Object is current selected when activated
            // Add wire
            wiringPool.addWired(args);
        }
        else
        {
            // Not selected when activated, remove wires
            wiringPool.removeWired(args);
        }
    }
}
