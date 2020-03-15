using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (TrailRenderer))]
public class TimeBasedTrailRenderer : MonoBehaviour
{
    [SerializeField] TrailRenderer trailRenderer;
    [SerializeField] float startAlpha = 1f;
    [SerializeField] float endAlpha = 0;
    [SerializeField] float duration;

    public void Clear ()
    {
        trailRenderer.Clear();
        Color startColor = trailRenderer.startColor;
        startColor.a = startAlpha;
        trailRenderer.startColor = startColor;
    }

    public void Shoot ()
    {
        DOTween.To(
            () =>
            { 
                return trailRenderer.startColor.a;
            },
            x => 
            {
                Color color = trailRenderer.startColor;
                color.a = x;
                trailRenderer.startColor = color;
            }, 
            endAlpha, duration);
    }
}
