using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TankDriveMovement : MonoBehaviour
{
    [Header("Tread Positions")]
    public Transform leftTread;
    public Transform rightTread;

    private float treadDistance;

    [Header("Speed Coefficient")]
    public float speedCoefficient = 1f;

    // Start is called before the first frame update
    void Start()
    {
        // Distance between the left and right treads
        treadDistance = Vector3.Distance(leftTread.position, rightTread.position);
    }

    float ls, rs, scls, scrs;   // For debug

    public void Move(float leftSpeed, float rightSpeed)
    {
        // Limits speeds to physical range
        leftSpeed = Mathf.Clamp(leftSpeed, -1f, 1f);
        rightSpeed = Mathf.Clamp(rightSpeed, -1f, 1f);

        ls = leftSpeed; rs = rightSpeed;    // For debug

        leftSpeed *= speedCoefficient;
        rightSpeed *= speedCoefficient;

        scls = leftSpeed; scrs = rightSpeed;    // For debug


        int dir = 0;
        float s = 0;    // slower tread (closer to 0)
        float f = 0;    // faster tread (farther from 0)

        if (leftSpeed == rightSpeed)    // going straight
        {
            transform.Translate(new Vector3(0, 0, leftSpeed * Time.deltaTime));
        }
        else
        {
            // set fast and slow
            if (Mathf.Abs(leftSpeed) < Mathf.Abs(rightSpeed))
            {
                dir = -1;
                s = leftSpeed;
                f = rightSpeed;
            }
            else
            {
                dir = 1;
                f = leftSpeed;
                s = rightSpeed;
            }

            f *= Time.deltaTime;
            s *= Time.deltaTime;

            // calculate radius of inner drive circle
            float r;    // radius
            if (s == 0)
            {
                r = 0;
            }
            else
            {
                r = treadDistance / ((f / s) - 1);
            }

            // calculate theta (degrees to rotate)
            float t = (f / (r + treadDistance));

            // calculate distance to travel
            float distance = (r + treadDistance / 2) * Mathf.Sin(t / 2) * 2;

            // movement
            transform.Rotate(new Vector3(0, (Mathf.Rad2Deg * t / 2) * dir, 0)); // rotate halfway
            transform.Translate(new Vector3(0, 0, distance));  // move
            transform.Rotate(new Vector3(0, (Mathf.Rad2Deg * t / 2) * dir, 0)); // finish rotation
        }
    }

    private void OnDrawGizmos()
    {
        Handles.Label(new Vector3(0, 8, 0), "Clamped Spped: " + "L: " + ls + " R: " + rs);
        Handles.Label(new Vector3(0, 9, 0), "Final Speed: " + "L: " + scls + " R: " + scrs);
    }
}
