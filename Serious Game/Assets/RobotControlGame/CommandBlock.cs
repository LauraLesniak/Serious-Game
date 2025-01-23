using UnityEngine;
using TMPro;

public enum CommandType
{
    MoveForward,
    TurnLeft,
    TurnRight
}

public class CommandBlock : MonoBehaviour
{
    public CommandType commandType;
    public TMP_Text commandText;
    public Color moveForwardColor = Color.green;
    public Color turnLeftColor = Color.blue;
    public Color turnRightColor = Color.red;

    private void Start()
    {
        UpdateCommandTextAndColor();
    }

    private void UpdateCommandTextAndColor()
    {
        switch (commandType)
        {
            case CommandType.MoveForward:
                commandText.text = "moveForward()";
                commandText.color = moveForwardColor;
                break;
            case CommandType.TurnLeft:
                commandText.text = "turnLeft()";
                commandText.color = turnLeftColor;
                break;
            case CommandType.TurnRight:
                commandText.text = "turnRight()";
                commandText.color = turnRightColor;
                break;
        }
    }
}
