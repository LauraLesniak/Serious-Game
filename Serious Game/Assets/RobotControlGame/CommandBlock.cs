using UnityEngine;

public enum CommandType
{
    MoveForward,
    TurnLeft,
    TurnRight
}

public class CommandBlock : MonoBehaviour
{
    public CommandType commandType;
}
