using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BlinkingController : MonoBehaviour
{
    [SerializeField] CharacterComponents components;

    public UnityAction OnBlink;

    public bool IsBlinking
    {
        get;
        private set;
    }

    public Vector3 BlinkingVelocity
    {
        get;
        private set;
    }

    public int BlinkCounter
    {
        get;
        private set;
    }

    BurinkeruInputManager inputManager;

    private void Awake()
    {
        inputManager = BurinkeruInputManager.Instance;
        BlinkCounter = 0;
    }

    public void UpdateBlinkingInput()
    {
        if (inputManager.IsCommandDown(BurinkeruInputManager.InputCommand.BLINK))
        {
            if (CanBlink ())
            {
                blink();
            }
        }
    }

    public void UpdateBlinkingForces()
    {
        BlinkingVelocity = BlinkingVelocity * CharacterControllerParameters.Instance.BlinkingDrag;

        if (IsBlinking)
        {
            if (BlinkingVelocity.magnitude < 1f)
            {
                IsBlinking = false;
                components.BlinkShadePostProcessEffect.StopEffect();
            }
        }
    }

    public bool CanBlink ()
    {
        return BlinkCounter < CharacterControllerParameters.Instance.MaxBlinksInAir && ! IsBlinking;
    }

    public void ResetCounter ()
    {
        BlinkCounter = 0;
    }

    public void ForceStop ()
    {
        IsBlinking = false;
        components.BlinkShadePostProcessEffect.StopEffect();
        BlinkingVelocity = Vector3.zero;
    }

    void blink()
    {
        BlinkCounter++;
        Vector3 forwardDirection = transform.forward;
        Vector3 rightDirection = transform.right;
        Vector3 deltaPosition = Vector3.zero;

        if (inputManager.IsCommandPressed(BurinkeruInputManager.InputCommand.FORWARD))
        {
            deltaPosition += (forwardDirection);
        }
        else if (inputManager.IsCommandPressed(BurinkeruInputManager.InputCommand.BACKWARD))
        {
            deltaPosition -= (forwardDirection);
        }

        if (inputManager.IsCommandPressed(BurinkeruInputManager.InputCommand.RIGHT))
        {
            deltaPosition += (rightDirection);
        }
        else if (inputManager.IsCommandPressed(BurinkeruInputManager.InputCommand.LEFT))
        {
            deltaPosition -= (rightDirection);
        }

        if (deltaPosition.magnitude > 0.1f)
        {
            IsBlinking = true;
            BlinkingVelocity = deltaPosition * CharacterControllerParameters.Instance.BlinkingSpeed;

            OnBlink?.Invoke();
            components.BlinkShadePostProcessEffect.StartEffect();
        }
    }
}
