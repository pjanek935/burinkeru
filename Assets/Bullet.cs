using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{
    [SerializeField] List<ParticleSystem> onHitParticles = new List<ParticleSystem>();

    private void OnCollisionEnter(Collision collision)
    {
        playOnHitParticles();
    }

    void playOnHitParticles ()
    {
        if (onHitParticles != null)
        {
            for (int i = 0; i < onHitParticles.Count; i++)
            {
                if (onHitParticles[i] != null)
                {
                    onHitParticles[i].Play();
                }
            }
        }
    }
}
