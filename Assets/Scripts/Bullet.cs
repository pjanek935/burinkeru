using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{
    [SerializeField] List<ParticleSystem> onHitParticles = new List<ParticleSystem>();
    [SerializeField] TrailRenderer trailRenderer;
    [SerializeField] TimeBasedTrailRenderer timeBasedTrailRenderer;
    [SerializeField] Hittable hittable;

    private void Awake()
    {
        hittable.OnHitterActivated += onHitterActivated;
    }

    void onHitterActivated (ActivatableHitter activatableHitter)
    {
        if (activatableHitter.HitterType == HitterType.SWORD)
        {
            float speed = GetBaseSpeed();

            if (TimeManager.Instance.IsSlowMotionOn)
            {
                speed *= GetSlowMoFactor();
            }

            Vector3 deltaMove = - this.transform.forward * speed;
            rigidbody.velocity = deltaMove;
        }
    }

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

    public override float GetSlowMoFactor()
    {
        return 0.01f;
    }
}
