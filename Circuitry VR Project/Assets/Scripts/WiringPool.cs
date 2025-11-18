using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class WiringPool : MonoBehaviour
{
    private List<GameObject> wired = new List<GameObject>();
    private LineRenderer lineRender;
    private GameObject controller;
    private bool pending;
    void Start()
    {
        lineRender = gameObject.AddComponent<LineRenderer>();
        lineRender.material = new Material(Shader.Find("Sprites/Default"));
        lineRender.startColor = Color.red;
        lineRender.endColor = Color.green;
        lineRender.startWidth = 0.05f;
        lineRender.endWidth = 0.05f;
        lineRender.positionCount = 2;
        pending = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (pending)
        {
            if (controller != null && wired.Count > 0)
            {
                // Controller and at least one object exist
                lineRender.SetPosition(1, wired[0].transform.position);
                lineRender.SetPosition(0, controller.transform.position);
            }
        }
        else
        {
            if(wired.Count >= 2)
            {
                lineRender.SetPosition(0, wired[0].transform.position);
                lineRender.SetPosition(1, wired[1].transform.position);
            }
        }
        
    }

    public void addWired(ActivateEventArgs args)
    {
        if (!pending)
        {
            wired.Add(args.interactableObject.transform.gameObject);
            lineRender.SetPosition(1, args.interactableObject.transform.position);
            lineRender.SetPosition(0, args.interactorObject.transform.position);
            controller = args.interactorObject.transform.gameObject;
            pending = true;
        }
        else
        {
            wired.Add(args.interactableObject.transform.gameObject);
            lineRender.SetPosition(1, args.interactableObject.transform.position);
            lineRender.SetPosition(0, wired[wired.Count - 2].transform.position);
            pending = false;
        }
        
    }
}