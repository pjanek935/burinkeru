﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashParticleManager : ParticlesManagerBase
{
    struct RegisteredSlash
    {
        public Vector3 Position;
        public Vector3 Forward;
        public Vector3 Upward;
    }

    [SerializeField] float initialDelay = 0.1f;
    [SerializeField] float delayBetweenSlahses = 0.04f;

    List<RegisteredSlash> registeredSlashes = new List<RegisteredSlash>();

    private void OnEnable()
    {
        TimeManager.Instance.OnTimeFactorChanged += onTimeFactorChanged;
    }

    private void OnDisable()
    {
        TimeManager.Instance.OnTimeFactorChanged -= onTimeFactorChanged;
    }

    public void RegisterNewSlash (Vector3 pos, Vector3 forward, Vector3 upward)
    {
        RegisteredSlash newSlash;
        newSlash.Position = pos;
        newSlash.Forward = forward;
        newSlash.Upward = upward;
        registeredSlashes.Add(newSlash);
    }

    void onTimeFactorChanged ()
    {
        if (! TimeManager.Instance.IsSlowMotionOn)
        {
            StartCoroutine (fireAllSlashes(getRegisteredSlashesCopy ()));
        }

        registeredSlashes.Clear();
    }

    IEnumerator fireAllSlashes (List <RegisteredSlash> registeredSlashes)
    {
        yield return new WaitForSeconds(initialDelay);

        for (int i = 0; i < registeredSlashes.Count; i ++)
        {
            GameObject particle = base.ShootParticle(registeredSlashes[i].Position, registeredSlashes[i].Forward, registeredSlashes[i].Upward);
            particle.layer = LayerMask.NameToLayer("Default");

            yield return new WaitForSeconds(delayBetweenSlahses);
        }
    }

    List <RegisteredSlash> getRegisteredSlashesCopy ()
    {
        List<RegisteredSlash> result = new List<RegisteredSlash>();

        for (int i = 0; i < registeredSlashes.Count; i ++)
        {
            result.Add(registeredSlashes[i]);
        }

        return result;
    }

    public override GameObject ShootParticle(Vector3 position, Vector3 forward, Vector3 upward)
    {
        GameObject result = base.ShootParticle(position, forward, upward);
        result.layer = LayerMask.NameToLayer("Rig");

        return result;
    }
}
