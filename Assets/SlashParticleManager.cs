using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashParticleManager : ParticlesManagerBase
{
    [SerializeField] List<Transform> originTransforms = new List<Transform>();

    public List<Transform> OriginTransforms
    {
        get { return originTransforms; }
    }

    public int LastTransformIndex
    {
        get;
        protected set;
    }

    public void ShootParticle (int index)
    {
        if (index >= 0 && index < originTransforms.Count && originTransforms[index] != null)
        {
            LastTransformIndex = index;
            Vector3 pos = originTransforms[index].position;
            Vector3 forward = originTransforms[index].forward;
            Vector3 up = originTransforms[index].up;

            ShootParticle(pos, forward, up);
        }
    }
}
