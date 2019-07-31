using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigWithKatanaAnimationController : RigAnimationController
{
    [SerializeField] GameObject trailParticlePrefab = null;
    [SerializeField] List<Transform> originTransforms = new List<Transform>();
    [SerializeField] GameObject pierceParticle = null;
    [SerializeField] ObjectCutter objectCutter = null;

    public override void OnStartAttack(int index)
    {
        trailParticlePrefab.transform.rotation = originTransforms[index].transform.rotation;
        trailParticlePrefab.transform.position = originTransforms[index].position;
        trailParticlePrefab.transform.Rotate(new Vector3(0, -90, -125));

        ParticleSystem[] particles = trailParticlePrefab.GetComponentsInChildren<ParticleSystem>();

        if (particles != null)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].Play();
            }
        }

        objectCutter.Cut(index);

        base.OnStartAttack(index);
    }

    public void Uppercut()
    {
        animator.SetTrigger("Uppercut");
    }

    public void Stab()
    {
        animator.SetTrigger("Stab");
    }
}
