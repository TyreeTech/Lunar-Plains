using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseException : MonoBehaviour
{
    public bool toggle { get; private set; }
    private bool defaultToggle;

    public BaseException (bool defaultToggle)
    {
        this.defaultToggle = defaultToggle;
        toggle = defaultToggle;
    }

    public void FlipToggle()
    {
        toggle = !defaultToggle;
    }
}
