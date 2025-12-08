using UnityEngine;

public class FireDisappear : MonoBehaviour
{
    void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Fire")) //to make the fire disappear when hit by foam particles
        {
            Debug.Log("Fire hit by foam particles");
            other.SetActive(false);
        }
    }
}
