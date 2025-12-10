using System;
using UnityEngine;
using UnityEngine.Events;
public class OnCollision : MonoBehaviour
{
    [Serializable] public class CollisionEvent : UnityEvent<Collision> { }

    // When the object enters a collision
    public CollisionEvent OnEnter = new CollisionEvent();

    // exit collision
    public CollisionEvent OnExit = new CollisionEvent();

    private void OnCollisionEnter(Collision collision)
    {
        OnEnter.Invoke(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        OnExit.Invoke(collision);
    }

    private void OnValidate()
    {
        if (TryGetComponent(out Collider collider))
            collider.isTrigger = false;
    }
}
