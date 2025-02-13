using UnityEngine;

public class KeyboardInputController : IInputController
{
    public bool IsJumpInput()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    public float GetHorizontalInput()
    {
        return Input.GetAxis("Horizontal");
    }
}