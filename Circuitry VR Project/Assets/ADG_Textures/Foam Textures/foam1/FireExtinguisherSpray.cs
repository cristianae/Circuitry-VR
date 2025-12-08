using UnityEngine;
using UnityEngine.InputSystem;


public class FireExtinguisherSpray : MonoBehaviour
{
    public ParticleSystem foamSpray;
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    public InputActionReference triggerAction;

    private bool isHeld = false;

    private void Awake()
    {
        grabInteractable.selectEntered.AddListener(_ => isHeld = true);
        grabInteractable.selectExited.AddListener(_ => isHeld = false);
    }

    private void Update()
    {
        if (!isHeld) 
        {
            if (foamSpray.isPlaying)
                foamSpray.Stop();
            return;
        }

        float triggerValue = triggerAction.action.ReadValue<float>();

        if (triggerValue > 0.1f)
        {
            if (!foamSpray.isPlaying)
                foamSpray.Play();
        }
        else
        {
            if (foamSpray.isPlaying)
                foamSpray.Stop();
        }
    }
}
