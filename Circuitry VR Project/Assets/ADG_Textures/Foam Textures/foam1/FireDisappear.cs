using UnityEngine;

public class FoamCollision : MonoBehaviour
{
    void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Fire")) //to make the fire disappear when hit by foam particles
        {
            other.SetActive(false);
        }
    }
}
