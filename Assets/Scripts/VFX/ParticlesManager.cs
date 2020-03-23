using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesManager : MonoBehaviour
{
    [SerializeField] ProjectileManager bulletsManager;
    [SerializeField] SlowMoSlashParticleManager slowMotionTrailsManager;
    [SerializeField] SlashParticleManager slashTrailManager;
    [SerializeField] ClipParticleManager clipParticleManager;
    [SerializeField] ParticlesManagerBase smokeParticleManager;
    [SerializeField] OnHitParticleManager swordOnHitParticleManager;

    static ParticlesManager instance;

    public static ParticlesManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Nie moga istniec dwie instancje ParticlesManager na scenie");
        }
        else
        {
            instance = this;
        }
    }

    private void OnDestroy()
    {
        if (this == instance)
        {
            instance = null;
        }
    }

    public ProjectileManager BulletsManager
    {
        get { return bulletsManager; }
    }

    public SlashParticleManager SlashTrailManager
    {
        get { return slashTrailManager; }
    }

    public SlowMoSlashParticleManager SlowMotionTrailsManager
    {
        get { return slowMotionTrailsManager; }
    }

    public ClipParticleManager ClipParticleManager
    {
        get { return clipParticleManager; }
    }

    public ParticlesManagerBase SmokeParticleManager
    {
        get { return smokeParticleManager; }
    }

    public OnHitParticleManager SwordOnHitParticleManager
    {
        get { return swordOnHitParticleManager; }
    }
}
