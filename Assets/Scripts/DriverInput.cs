using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(TankDriveMovement))]
public class DriverInput : MonoBehaviour
{
    [Header("Input Curves")]
    [Min(0f)] public float translationCurve = 0f;
    [Min(0f)] public float rotationCurve = 0f;
    [Min(0f)] public float tankCurve = 0f;


    public enum InputTypes
    {
        Tank, Arcade
    };

    [Header("Input Type")]
    public InputTypes inputType = InputTypes.Tank;

    TankDriveMovement tdm;

    // Start is called before the first frame update
    void Start()
    {
        tdm = GetComponent<TankDriveMovement>();
    }

    float ls, rs, li, ri;   // For debug

    void FixedUpdate()
    {
        float leftSpeed = 0f, rightSpeed = 0f;

        switch (inputType)
        {
            case InputTypes.Tank:
                float leftInput = Input.GetAxis("LeftJoyVertical");
                float rightInput = Input.GetAxis("RightJoyVertical");

                li = leftInput; ri = rightInput;

                leftSpeed = curveInput(leftInput, tankCurve);
                rightSpeed = curveInput(rightInput, tankCurve);

                break;
            case InputTypes.Arcade:
                float translationInput = Input.GetAxis("LeftJoyVertical");
                float rotationInput = Input.GetAxis("RightJoyHorizontal");

                li = translationInput; ri = rotationInput;

                leftSpeed = curveInput(translationInput, translationCurve) + curveInput(rotationInput, rotationCurve);
                rightSpeed = curveInput(translationInput, translationCurve) - curveInput(rotationInput, rotationCurve);

                break;
        }

        ls = leftSpeed; rs = rightSpeed;    // For debug
        tdm.Move(leftSpeed, rightSpeed);
    }

    // Curves the input for smoother driver control
    float curveInput(float input, float t)
    {
        return Mathf.Exp(t * (Mathf.Abs(input) - 1) / 10) * input;
    }

    private void OnDrawGizmos()
    {
        Handles.Label(new Vector3(0, 6, 0), "Raw Input: " + "L: " + li + " R: " + ri);
        Handles.Label(new Vector3(0, 7, 0), "Raw Speed: " + "L: " + ls + " R: " + rs);
    }
}
