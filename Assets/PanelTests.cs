using System.Collections;
using UnityEngine;

public class PanelTests : MonoBehaviour
{

    // Adds new UI posion (EX: Center)
    Panel panel;
    const string Show = "Show";
    const string Hide = "Hide";
    const string Center = "Center";

    // assigns position to the word "Center"
    void Start()
    {
          panel = GetComponent<Panel>();
          Panel.Position centerPos = new Panel.Position(Center,
          TextAnchor.MiddleCenter, TextAnchor.MiddleCenter);
          panel.AddPosition(centerPos);
    }

    void OnGGUI()
    {
        if(GUI.Button(new Rect(10, 10, 100, 30), Show))
            panel.SetPosition(Show, true);
        if(GUI.Button(new Rect(10, 50, 100, 30), Hide))
            panel.SetPosition(Hide, true);
        if(GUI.Button(new Rect(10, 90, 100, 30), Center))
        {
            Tweener t = panel.SetPosition(Center, true);
            t.easingControl.equation = EasingEquations.EaseInOutBack;
        }
    }
}
