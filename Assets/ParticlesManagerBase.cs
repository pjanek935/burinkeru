using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesManagerBase : MonoBehaviour
{
    [SerializeField] protected GameObject particlePrefab;
    [SerializeField] protected int maxParticles = 10;

    protected Queue<GameObject> particlesQueue;

    protected virtual void Awake ()
    {
        initParticlesIfNeeded();
    }
    protected void initParticlesIfNeeded()
    {
        if (particlesQueue == null)
        {
            particlesQueue = new Queue<GameObject>();

            for (int i = 0; i < maxParticles; i++)
            {
                GameObject newGameObject = Instantiate(particlePrefab);
                newGameObject.transform.SetParent(this.transform, false);
                particlesQueue.Enqueue(newGameObject);
            }
        }
    }

    public virtual GameObject ShootParticle (Vector3 position, Vector3 forward, Vector3 upward)
    {
        initParticlesIfNeeded();
        GameObject particle = particlesQueue.Dequeue();

        particle.transform.rotation = Quaternion.LookRotation(upward, forward);
        particle.SetActive(true);
        particle.transform.position = position;
        ParticleSystem[] particles = particle.GetComponentsInChildren<ParticleSystem>();

        if (particles != null)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].Play();
            }
        }

        particlesQueue.Enqueue(particle);

        return particle;
    }
}
