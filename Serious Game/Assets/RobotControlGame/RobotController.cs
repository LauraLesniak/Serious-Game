using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float rotationSpeed = 90f; // degrees per second

    public IEnumerator MoveForward()
    {
        float moveTime = 1f; // Move forward for 1 second
        float elapsedTime = 0;

        while (elapsedTime < moveTime)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator TurnLeft()
    {
        float rotateTime = 1f;
        float elapsedTime = 0;

        while (elapsedTime < rotateTime)
        {
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator TurnRight()
    {
        float rotateTime = 1f;
        float elapsedTime = 0;

        while (elapsedTime < rotateTime)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
