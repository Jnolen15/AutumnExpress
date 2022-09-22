using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D normal;
    public Texture2D hover;
    public Texture2D pressed;

    public enum State
    {
        Normal,
        Hover,
        Pressed
    }
    public State state;

    private bool holding = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            holding = true;
            state = State.Pressed;
        }
        if (Input.GetMouseButtonUp(0))
        {
            holding = false;
            state = State.Normal;
        }

        switch (state)
        {
            case State.Normal:
                Cursor.SetCursor(normal, Vector3.zero, CursorMode.ForceSoftware);
                break;
            case State.Hover:
                Cursor.SetCursor(hover, Vector3.zero, CursorMode.ForceSoftware);
                break;
            case State.Pressed:
                Cursor.SetCursor(pressed, Vector3.zero, CursorMode.ForceSoftware);
                break;
        }
    }

    public void SetNormal()
    {
        if(!holding)
            state = State.Normal;
    }

    public void SetHover()
    {
        if(!holding)
            state = State.Hover;
    }
}
