using UnityEngine;

public class LightController : MonoBehaviour
{
    public bool isPowered = false;

    public void TurnOn()
    {
        if (!isPowered)
        {
            isPowered = true;
            GetComponent<SpriteRenderer>().color = Color.yellow; // On
        }
    }

    public void TurnOff()
    {
        if (isPowered)
        {
            isPowered = false;
            GetComponent<SpriteRenderer>().color = Color.gray; // Off
        }
    }
}