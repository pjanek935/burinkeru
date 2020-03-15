using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipParticleManager : ParticlesManagerBase
{
    Coroutine currentCoroutine;

    const float visibleDuration = 1f;
    const float randRange = 0.1f;

    public Vector3 BaseVelocity
    {
        get;
        set;
    }

    public override GameObject ShootParticle(Vector3 position, Vector3 forward, Vector3 upward)
    {
        GameObject particle = base.ShootParticle(position, forward, upward);
        Rigidbody rigidbody = particle.GetComponent<Rigidbody>();

        if (rigidbody != null)
        {
            rigidbody.velocity = BaseVelocity;

            Vector3 force = -2f * forward;
            force += new Vector3(Random.Range (-randRange, randRange),
                Random.Range(-randRange, randRange),
                Random.Range(-randRange, randRange));
            Vector3 randTorque = new Vector3(Random.Range(-randRange, randRange),
                Random.Range(-randRange, randRange),
                Random.Range(-randRange, randRange));

            rigidbody.AddForce(force, ForceMode.Impulse);
            rigidbody.AddTorque(randTorque, ForceMode.Impulse);
        }

        stopCoroutine();
        currentCoroutine = StartCoroutine(waitAndDisable (particle));

        return particle;
    }

    protected override bool worldPositionStays()
    {
        return true;
    }

    IEnumerator waitAndDisable (GameObject gameObject)
    {
        yield return new WaitForSeconds(visibleDuration);

        if (gameObject != null)
        {
            gameObject.SetActive(false);
        }
    }

    void stopCoroutine ()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
    }
}
