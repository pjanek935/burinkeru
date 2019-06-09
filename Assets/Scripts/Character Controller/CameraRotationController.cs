using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotationController : MonoBehaviour
{
    public enum RotationAxes { X_AND_Y = 0, X = 1, Y = 2 }

    [SerializeField] RotationAxes axes = RotationAxes.X_AND_Y;
    [SerializeField] float sensitivityX = 15f;
    [SerializeField] float sensitivityY = 15f;

    [SerializeField] float minimumY = -60f;
    [SerializeField] float maximumY = 60f;

    [SerializeField] bool locked = false;

    float rotationY = 0f;
    float rotationX = 0f;

    void Update()
    {
        if (! locked)
        {
            updateRotation();
        }
    }

    void updateRotation ()
    {
        switch (axes)
        {
            case RotationAxes.X_AND_Y:

                updateXRotation();
                updateYRotation();

                break;

            case RotationAxes.X:

                updateXRotation();

                break;

            case RotationAxes.Y:

                updateYRotation();

                break;
        }
    }

    void updateXRotation ()
    {
        rotationX = transform.localEulerAngles.y + BurinkeruInputManager.Instance.GetRightAxis ().x * sensitivityX;
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, rotationX, 0);
    }

    void updateYRotation ()
    {
        rotationY += BurinkeruInputManager.Instance.GetRightAxis().y * sensitivityY;
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
        transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
    }
}
