using UnityEngine;
using TMPro;

[System.Serializable]
public class CommandData
{
    public CommandType commandType;
    public int repeatCount;

    public CommandData(CommandType type, int repeats)
    {
        commandType = type;
        repeatCount = repeats;
    }
}

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

    public TMP_Dropdown repeatDropdown;
    public int repeatCount = 1;

    private void Start()
    {
        UpdateCommandTextAndColor();

        // Ensure dropdown is linked and set up the listener
        if (repeatDropdown != null)
        {
            repeatDropdown.onValueChanged.AddListener(OnRepeatCountChanged);
            repeatCount = int.Parse(repeatDropdown.options[repeatDropdown.value].text); // Initialize
        }
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

    public void OnRepeatCountChanged(int index)
    {
        repeatCount = int.Parse(repeatDropdown.options[index].text);
        CommandRunner.Instance.SaveCommands();
    }
}
