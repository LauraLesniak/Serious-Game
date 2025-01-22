using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandRunner : MonoBehaviour
{
    public Transform commandGrid;       // The grid where commands are placed
    public RobotController robot;       // Reference to the robot
    public Color highlightColor = Color.yellow; // Color to highlight the current command

    public void RunCommands()
    {
        StartCoroutine(ExecuteCommands());
    }

    private IEnumerator ExecuteCommands()
    {
        foreach (Transform commandBlock in commandGrid)
        {
            CommandBlock block = commandBlock.GetComponent<CommandBlock>();
            Image blockImage = commandBlock.GetComponent<Image>();
            Color originalColor = blockImage.color;

            if (block != null)
            {

                // Highlight the current command
                blockImage.color = highlightColor;

                switch (block.commandType)
                {
                    case CommandType.MoveForward:
                        yield return StartCoroutine(robot.MoveForward());
                        break;

                    case CommandType.TurnLeft:
                        yield return StartCoroutine(robot.TurnLeft());
                        break;

                    case CommandType.TurnRight:
                        yield return StartCoroutine(robot.TurnRight());
                        break;
                }
                // Revert the color back to the original
                blockImage.color = originalColor;
            }   
        }
    }
}
