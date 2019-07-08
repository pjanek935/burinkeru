using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigAnimationController : MonoBehaviour
{
    [SerializeField] Animator animator = null;
    [SerializeField] GameObject trailParticlePrefab;
    [SerializeField] List <Transform> originTransforms;

    public void SetWalk (bool walk)
    {
        animator.SetBool("Walk", walk);
    }
    
    public void Attack ()
    {
        animator.SetTrigger("Attack");
    }

    public void StartCut(int index)
    {
        Debug.Log("Index: " + index);
        //GameObject newTrail = Instantiate(trailParticlePrefab);
        //newTrail.transform.position = originTransform.transform.position;
        //newTrail.transform.rotation = originTransform.transform.rotation;
        //newTrail.SetActive(true);
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
    }

    public void StopCut()
    {

    }
}
