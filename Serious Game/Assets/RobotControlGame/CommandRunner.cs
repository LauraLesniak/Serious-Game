using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandRunner : MonoBehaviour
{
    public Transform commandGrid;       // The grid where commands are placed
    public RobotController robot;       // Reference to the robot

    public void RunCommands()
    {
        StartCoroutine(ExecuteCommands());
    }

    private IEnumerator ExecuteCommands()
    {
        foreach (Transform commandBlock in commandGrid)
        {
            CommandBlock block = commandBlock.GetComponent<CommandBlock>();

            if (block != null)
            {
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
            }   
        }
    }
}
