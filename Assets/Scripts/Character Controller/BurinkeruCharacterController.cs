using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurinkeruCharacterController : MonoBehaviour
{
    [SerializeField] CharacterController baseController = null;
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float jumpHeight = 10f;

    CharacterControllerStateBase currentCharacterState;
    BurinkeruInputManager inputManager;

    static Vector3 movementAxes = new Vector3 (1f, 0f, 1f);
    public const float GRAVITY = 9.8f;
    const float heightOffset = 0.5f;

    Vector3 velocity = Vector3.zero;

    public Vector3 Velocity
    {
        get { return velocity; }
    }

    public float MovementSpeed
    {
        get { return movementSpeed; }
    }

    public float JumpHeight
    {
        get { return jumpHeight; }
    }

    public bool IsGrounded
    {
        get;
        private set;
    }

    public float IsGroundedRayCastLentgth
    {
        get;
        private set;
    }

    public float GetCharacterHeight ()
    {
        return baseController.height + heightOffset;
    }

    void setNewState <T> () where T : CharacterControllerStateBase
    {
        if (currentCharacterState == null || typeof (T) != currentCharacterState.GetType ())
        {
            CharacterControllerStateBase newState = (T)Activator.CreateInstance(typeof(T));

            if (currentCharacterState != null)
            {
                exitState(currentCharacterState);
            }

            enterState(newState);
            currentCharacterState = newState;
        }
        
    }

    void setNewState (CharacterControllerStateBase newState)
    {
        if (currentCharacterState == null || newState.GetType () != currentCharacterState.GetType())
        {
            if (currentCharacterState != null)
            {
                exitState(currentCharacterState);
            }

            enterState(newState);
            currentCharacterState = newState;
        }
    }

    void enterState (CharacterControllerStateBase state)
    {
        state.OnSetNewStateRequested += setNewState;
        state.OnAddVelocityRequested += addVelocity;
        state.OnSetVelocityRequested += setVelocity;
        state.Enter(currentCharacterState, inputManager, this);
    }

    void exitState (CharacterControllerStateBase state)
    {
        state.OnSetNewStateRequested -= setNewState;
        state.OnAddVelocityRequested -= addVelocity;
        state.OnSetVelocityRequested -= setVelocity;
        state.Exit();
    }

    // Start is called before the first frame update
    void Start()
    {
        inputManager = BurinkeruInputManager.Instance;
        calculateIsGroundedRayCastLentgth();
        setNewState<InAirState>();
    }

    // Update is called once per frame
    void Update()
    {
        updateIsGrounded();
        updateMovement();
        applyForces();
        
    }

    void updateMovement ()
    {
        Vector3 deltaPosition = Vector3.zero;
        Vector3 forwardDirection = this.transform.forward;
        Vector3 rightDirection = this.transform.right;

        if (inputManager.IsCommandPressed (BurinkeruInputManager.InputCommand.FORWARD))
        {
            deltaPosition += (forwardDirection * movementSpeed * Time.deltaTime);
        }
        else if (inputManager.IsCommandPressed(BurinkeruInputManager.InputCommand.BACKWARD))
        {
            deltaPosition -= (forwardDirection * movementSpeed * Time.deltaTime);
        }

        if (inputManager.IsCommandPressed(BurinkeruInputManager.InputCommand.RIGHT))
        {
            deltaPosition += (rightDirection * movementSpeed * Time.deltaTime);
        }
        else if (inputManager.IsCommandPressed(BurinkeruInputManager.InputCommand.LEFT))
        {
            deltaPosition -= (rightDirection * movementSpeed * Time.deltaTime);
        }

        deltaPosition.Scale(movementAxes);
        baseController.Move(deltaPosition);

        if (currentCharacterState != null)
        {
            currentCharacterState.UpdateMovement();
        }
    }

    void applyForces ()
    {
        baseController.Move(velocity * Time.deltaTime);

        if (currentCharacterState != null)
        {
            currentCharacterState.ApplyForces();
        }
    }

    void addVelocity (Vector3 velocityDelta)
    {
        velocity += velocityDelta;
    }

    void setVelocity (Vector3 newVelocty)
    {
        velocity = newVelocty;
    }

    void updateIsGrounded ()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up), out hit, IsGroundedRayCastLentgth))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.up) * hit.distance, Color.yellow);
            IsGrounded = true;
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.up) * 1000, Color.white);
            IsGrounded = false;
        }
    }

    void calculateIsGroundedRayCastLentgth()
    {
        IsGroundedRayCastLentgth = baseController.height / 2f + heightOffset;
    }
}
