using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FireExtinguisherSpray : MonoBehaviour
{
    public ParticleSystem foamSpray;
    public ActionBasedController controller;

    // Update is called once per frame
    void Update()
    {
        float triggerValue = controller.activateAction.action.ReadValue<float>();
        //when the trigger is pressed by the user
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
