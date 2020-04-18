using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{
    [SerializeField] List<ParticleSystem> onHitParticles = new List<ParticleSystem>();
    [SerializeField] TrailRenderer trailRenderer = null;
    [SerializeField] TimeBasedTrailRenderer timeBasedTrailRenderer = null;
    [SerializeField] Hittable hittable = null;

    const float BASE_SPEED = 400f;
    const float RICOCHET_PROBABILITY = 0.05f;

    float speed = BASE_SPEED;
    static System.Random random = new System.Random ();
    bool isARicochet = false;

    protected new void Awake()
    {
        base.Awake ();

        if (hittable != null)
        {
            hittable.OnHitterActivated += onHitterActivated;
        }
    }

    void onHitterActivated (ActivatableHitter activatableHitter, Hashtable parameters)
    {
        if (activatableHitter.HitterType == HitterType.BLADE)
        {
            this.transform.forward = -this.transform.forward;
            setSpeed ();
        }
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (IsActive)
        {
            playOnHitParticles ();
        }

        if (!isARicochet)
        {
            if (random.NextDouble () > RICOCHET_PROBABILITY)
            {
                if (this.gameObject.activeInHierarchy)
                {
                    //Waiting 2 frames so other hittables has a chance to get triggered
                    StartCoroutine (waitNFramesAndTrigger (2, deactivate));
                }
                else
                {
                    deactivate ();
                }
            }
            else
            {
                ricochet ();
            }
        }
        else
        {
            ricochet ();
        }
    }

    void ricochet ()
    {
        isARicochet = true;
        this.speed /= 2f;
        setSpeed ();
        rigidbody.angularVelocity = new Vector3 (1f, 1f, 1f);
        rigidbody.useGravity = true;
    }

    protected override void activate()
    {
        trailRenderer.Clear();
        timeBasedTrailRenderer.Clear();
        timeBasedTrailRenderer.Shoot();
        isARicochet = false;
        rigidbody.useGravity = false;
        speed = BASE_SPEED;

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
        return speed;
    }

    public override float GetSlowMoFactor()
    {
        return 0.01f;
    }
}
