using UnityEngine;

public class FireDisappear : MonoBehaviour
{   
    private BatteryFire batteryOnFire;
    void Start()
    {
        batteryOnFire = GetComponentInParent<BatteryFire>();
    }
    void OnParticleCollision(GameObject other)
    {
        if(other.CompareTag("Foam")) //to make the fire disappear when hit by foam particles
        {
            Debug.Log("Foam hit the fire");
            batteryOnFire.DisableFire();
        }
    }
}
