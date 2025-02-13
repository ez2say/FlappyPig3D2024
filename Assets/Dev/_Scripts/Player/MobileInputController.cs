using UnityEngine;

public class MobileInputController : IInputController
{
    private bool _isJumping = false;
    private float _horizontalInput = 0f;

    private void Update()
    {
        HandleTouchInput();
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.position.x < Screen.width / 2)
            {
                _horizontalInput = touch.deltaPosition.x / Screen.width;
            }
            else
            {
                if (touch.phase == TouchPhase.Began)
                {
                    _isJumping = true;
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    _isJumping = false;
                }
            }
        }
        else
        {
            _horizontalInput = 0f;
            _isJumping = false;
        }
    }

    public bool IsJumpInput()
    {
        return _isJumping;
    }

    public float GetHorizontalInput()
    {
        return _horizontalInput;
    }
}