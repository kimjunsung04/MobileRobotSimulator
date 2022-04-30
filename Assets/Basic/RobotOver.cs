using System.Collections;
using UnityEngine;

public class RobotOver : MonoBehaviour
{
    public Material ColorMatrial;
    public Color startColor;
    public Color mouseOverColor;
    public bool mouseOver = false;

    void OnMouseEnter()
    {
        mouseOver = true;
        ColorMatrial.SetColor("_Color", mouseOverColor);
    }

    void OnMouseExit()
    {
        mouseOver = false;
        ColorMatrial.SetColor("_Color", startColor);
    }
}
