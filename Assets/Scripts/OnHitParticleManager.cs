using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitParticleManager : ParticlesManagerBase
{
    [SerializeField] SlashParticleManager slashParticleManager;
    [SerializeField] List<Transform> originTransforms = new List<Transform>();
    public void ShootParticle ()
    {
        int index = slashParticleManager.LastTransformIndex;

        if (index >= 0 && index < originTransforms.Count && originTransforms[index] != null)
        {
            Vector3 pos = originTransforms[index].position;
            Vector3 forward = originTransforms[index].forward;
            Vector3 up = originTransforms[index].up;

            ShootParticle(pos, forward, up);
        }
    }
}
