using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitable : MonoBehaviour
{
    new Rigidbody rigidbody = null;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void OnParticleCollision(GameObject other)
    {
        Debug.Log("!!");
        ParticleSystem particleSystem = other.GetComponent<ParticleSystem>();
        List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
        particleSystem.GetCollisionEvents(this.gameObject, collisionEvents);

        for (int i = 0; i < collisionEvents.Count; i ++)
        {
            Vector3 force = collisionEvents[i].velocity * 2f + Vector3.up * 2f;
            rigidbody.AddForce(force);
        }
    }
}
