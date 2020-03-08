using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{
    [SerializeField] List<ParticleSystem> onHitParticles = new List<ParticleSystem>();
    [SerializeField] TrailRenderer trailRenderer;
    [SerializeField] TimeBasedTrailRenderer timeBasedTrailRenderer;

    protected override void OnCollisionEnter(Collision collision)
    {
        if (IsActive)
        {
            playOnHitParticles();
        }

        base.OnCollisionEnter(collision);
    }

    protected override void activate()
    {
        trailRenderer.Clear();
        timeBasedTrailRenderer.Clear();
        timeBasedTrailRenderer.Shoot();

        base.activate();
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
    public override float GetBaseSpeed()
    {
        return 400f;
    }
}
