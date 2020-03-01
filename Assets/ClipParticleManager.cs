using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipParticleManager : ParticlesManagerBase
{
    [SerializeField] Transform parentTransform;

    public Vector3 BaseVelocity
    {
        get;
        set;
    }

    const float randRange = 0.1f;
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

        return particle;
    }

    protected override Transform getParentTransform()
    {
        return parentTransform;
    }

    protected override bool worldPositionStays()
    {
        return true;
    }
}
