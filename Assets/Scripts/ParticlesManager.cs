using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesManager : MonoBehaviour
{
    [SerializeField] GameObject bulletParticlePrefab;
    [SerializeField] int maxBulletParticles = 10;

    Queue<GameObject> bulletParticles;

    private void Awake()
    {
        initBulletParticlesIfNeeded();
    }

    void initBulletParticlesIfNeeded ()
    {
        if (bulletParticles == null)
        {
            bulletParticles = new Queue<GameObject>();

            for (int i = 0; i < maxBulletParticles; i ++)
            {
                GameObject newGameObject = Instantiate(bulletParticlePrefab);
                newGameObject.transform.SetParent(this.transform, false);
                bulletParticles.Enqueue(newGameObject);
            }
        }
    }

    public void Shoot (Vector3 position, Vector3 forward, Vector3 upward)
    {
        initBulletParticlesIfNeeded();
        GameObject bullet = bulletParticles.Dequeue();
        
        bullet.transform.rotation = Quaternion.LookRotation(upward, forward);
        bullet.SetActive(true);
        bullet.transform.position = position;
        bullet.GetComponent<ParticleSystem>().Play();

        bulletParticles.Enqueue(bullet);
    }
}
