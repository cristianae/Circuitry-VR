using UnityEngine;


public class Battery9V : MonoBehaviour
{
    [Range(0f, 12f)] public float voltage = 9f;

    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor positiveSocket;
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor negativeSocket;

    public bool HasCompleteCircuit(out UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable posSel, out UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable negSel)
    {
        posSel = positiveSocket != null ? positiveSocket.firstInteractableSelected : null;
        negSel = negativeSocket != null ? negativeSocket.firstInteractableSelected : null;
        return posSel != null && negSel != null;
    }
}
