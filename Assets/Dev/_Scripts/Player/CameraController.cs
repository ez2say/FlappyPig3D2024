using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _mainCamera;
    [SerializeField] private CinemachineVirtualCamera _2DCamera;

    private bool _is2DView = false;

    public void SwitchCameraView(bool to2D)
    {
        _is2DView = to2D;
        if (to2D)
        {
            _mainCamera.Priority = 0;
            _2DCamera.Priority = 1;
        }
        else
        {
            _mainCamera.Priority = 1;
            _2DCamera.Priority = 0;
        }
    }

    public bool Is2DView()
    {
        return _is2DView;
    }
}