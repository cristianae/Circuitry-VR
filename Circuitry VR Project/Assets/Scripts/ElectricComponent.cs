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
    [SerializeField] public string componentName;
    [SerializeField] GameObject pairedNode;
    [SerializeField] bool omnidirectional;
    void Start()
    {
        //interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>(); <-- Replaced with the line below
        interactable = GetComponent<XRBaseInteractable>();

        if (interactable != null)
        {
            interactable.activated.AddListener(activateWire);
        }
        //added
        else
        {
            Debug.LogWarning($"[ElectricComponent] No XRBaseInteractable found on {gameObject.name}");
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
            // If part of a polar component, add internal wire to pool
            if(pairedNode != null){
                if(omnidirectional){
                    wiringPool.addOmniWire(this.gameObject, pairedNode);
                }
                else{
                    wiringPool.addInternalWire(this.gameObject, pairedNode);
                }
            }
        }
        else
        {
            // Not selected when activated, remove wires
            wiringPool.removeWired(args);
            if(pairedNode != null){
                if(omnidirectional){
                    wiringPool.removeOmniWire(this.gameObject);
                }
                else{
                    wiringPool.removeInternalWire(this.gameObject);
                }
            }
        }
    }
}
