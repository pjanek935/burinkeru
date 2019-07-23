using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RigAnimationController : MonoBehaviour
{
    public UnityAction OnAttackEnded;
    public UnityAction OnAttackStarted;

    [SerializeField] Animator animator = null;
    [SerializeField] GameObject trailParticlePrefab = null;
    [SerializeField] List <Transform> originTransforms = new List<Transform> ();
    [SerializeField] GameObject pierceParticle = null;
    [SerializeField] ObjectCutter objectCutter = null;

    public void SetWalk (bool walk)
    {
        animator.SetBool("Walk", walk);
    }
    
    public void Attack ()
    {
        animator.SetTrigger("Attack");
    }

    public void Uppercut ()
    {
        animator.SetTrigger("Uppercut");
    }

    public void Stab()
    {
        animator.SetTrigger("Stab");
    }

    public void StartCut(int index)
    {
        //Debug.Log("Index: " + index);
        trailParticlePrefab.transform.rotation = originTransforms[index].transform.rotation;
        trailParticlePrefab.transform.position = originTransforms[index].position;

        if (index == 0)
        {
           
        }

        trailParticlePrefab.transform.Rotate(new Vector3(0, -90, -125));

        ParticleSystem[] particles = trailParticlePrefab.GetComponentsInChildren<ParticleSystem>();
        
        if (particles != null)
        {
            for (int i = 0; i < particles.Length; i ++)
            {
                particles[i].Play();
            }
        }

        objectCutter.Cut(index);
        OnAttackStarted?.Invoke();

    }

    public void OnEndAttack()
    {
        OnAttackEnded?.Invoke();
    }

    public void StartPierce ()
    {
        ParticleSystem[] particles = pierceParticle.GetComponentsInChildren<ParticleSystem>();

        if (particles != null)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].Play();
            }
        }

        OnAttackStarted?.Invoke();
    }
}
