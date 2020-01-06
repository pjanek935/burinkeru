using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesManager : MonoBehaviour
{
    [SerializeField] ProjectileManager bulletsManager;
    [SerializeField] SlashParticleManager slashTrailManager;
    [SerializeField] ParticlesManagerBase slowMotionTrailsManager;

    public ProjectileManager BulletsManager
    {
        get { return bulletsManager; }
    }

    public SlashParticleManager SlashTrailManager
    {
        get { return slashTrailManager; }
    }

    public ParticlesManagerBase SlowMotionTrailsManager
    {
        get { return slowMotionTrailsManager; }
    }
}
