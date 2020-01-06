using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] int maxProjectiles;

    Queue<Projectile> projectiles;

    private void Awake()
    {
        initProjectilesIfNeeded();
    }

    protected void initProjectilesIfNeeded()
    {
        if (projectiles == null && projectilePrefab != null)
        {
            projectiles = new Queue<Projectile>();

            for (int i = 0; i < maxProjectiles; i++)
            {
                GameObject newGameObject = Instantiate(projectilePrefab);
                newGameObject.transform.SetParent(this.transform, false);
                projectiles.Enqueue(newGameObject.GetComponent <Projectile> ());
            }
        }
    }

    public virtual Projectile Shoot(Vector3 position, Vector3 forward, Vector3 upward)
    {
        Projectile projectile = null;

        if (projectilePrefab != null)
        {
            initProjectilesIfNeeded();
            projectile = projectiles.Dequeue();
            projectile.Shoot(position, forward, upward);
            projectiles.Enqueue(projectile);
        }

        return projectile;
    }
}
