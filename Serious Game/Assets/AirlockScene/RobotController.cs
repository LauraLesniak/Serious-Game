using System;
using System.Collections;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float rotationSpeed = 90f; // degrees per second
    public float bumpDistance = 0.2f; // Small bump distance when hitting the grid edge
    public float bumpSpeed = 2f;      // Speed of the bump animation

    public Vector2Int gridPosition = Vector2Int.zero;  // Robot's grid coordinates
    private Vector3[] directions = { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };
    private int currentDirectionIndex = 0; // 0 = forward, 1 = right, 2 = back, 3 = left

    private bool isMoving = false;

    public GridGenerator gridGenerator;  // Reference to the grid

    private float tileSize;

    void Start()
    {
        if (gridGenerator == null)
        {
            Debug.LogError("GridGenerator is not assigned!");
            return;
        }

        tileSize = gridGenerator.tileSize;

        AlignToGrid(gridGenerator.startGridPosition);
        //Debug.Log("Robot position: " + transform.position);
    }

    private void AlignToGrid(Vector2Int newGridPosition)
    {
        transform.localPosition = gridGenerator.GetPositionFromGrid(newGridPosition);
    }

    public IEnumerator MoveForward()
    {
        if (isMoving) yield break;
        isMoving = true;

        Vector2Int gridOffset = GetGridOffset();
        Vector2Int targetGridPosition = gridPosition + gridOffset;

        // Check if the target position is within the grid boundaries
        if (IsWithinGridBounds(targetGridPosition))
        {
            //transfer from local position to world position
            Vector3 targetPosition = transform.root.TransformPoint(gridGenerator.GetPositionFromGrid(targetGridPosition));

            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            transform.position = targetPosition;
            gridPosition = targetGridPosition;

            // Check if the robot has reached the finish position
            if (gridPosition == gridGenerator.finishGridPosition)
            {
                OnGameCompleted();
            }
        }
        else
        {
            // Perform a bump animation to indicate blocked movement
            yield return StartCoroutine(BumpForward());
        }

        isMoving = false;
    }

    public IEnumerator TurnLeft()
    {
        if (isMoving) yield break;
        isMoving = true;

        currentDirectionIndex = (currentDirectionIndex + 3) % 4; // Turn left
        yield return RotateByAngle(-90);
        isMoving = false;
    }

    public IEnumerator TurnRight()
    {
        if (isMoving) yield break;
        isMoving = true;

        currentDirectionIndex = (currentDirectionIndex + 1) % 4; // Turn right
        yield return RotateByAngle(90);
        isMoving = false;
    }

    private IEnumerator RotateByAngle(float angle)
    {
        float targetAngle = transform.eulerAngles.y + angle;
        float elapsedTime = 0f;
        float duration = Mathf.Abs(angle) / rotationSpeed;

        while (elapsedTime < duration)
        {
            float step = rotationSpeed * Time.deltaTime * Mathf.Sign(angle);
            transform.Rotate(Vector3.up, step);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Vector3 snappedRotation = transform.eulerAngles;
        snappedRotation.y = Mathf.Round(targetAngle / 90f) * 90f;
        transform.eulerAngles = snappedRotation;
    }

    private Vector2Int GetGridOffset()
    {
        switch (currentDirectionIndex)
        {
            case 0: return Vector2Int.up;     // Forward (Z+)
            case 1: return Vector2Int.right;  // Right (X+)
            case 2: return Vector2Int.down;   // Backward (Z-)
            case 3: return Vector2Int.left;   // Left (X-)
            default: return Vector2Int.zero;
        }
    }

    // Bump animation when the robot can't move forward
    private IEnumerator BumpForward()
    {
        Vector3 bumpDirection = directions[currentDirectionIndex];
        Vector3 bumpTarget = transform.position + bumpDirection * bumpDistance;
        Vector3 originalPosition = transform.position;

        // Move forward slightly
        while (Vector3.Distance(transform.position, bumpTarget) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, bumpTarget, bumpSpeed * Time.deltaTime);
            yield return null;
        }

        // Move back to original position
        while (Vector3.Distance(transform.position, originalPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, bumpSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private bool IsWithinGridBounds(Vector2Int position)
    {
       //return true; // Disable grid bounds check for now
        return position.x >= 0 && position.x < gridGenerator.gridWidth &&
               position.y >= 0 && position.y < gridGenerator.gridHeight &&
           !gridGenerator.obstaclePositions.Contains(position); // Check for obstacles
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + Vector3.up * 0.1f, 0.1f);
    }

    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }

    private void OnGameCompleted()
    {
        CommandRunner.Instance.OnGameCompleted();
    }
}
