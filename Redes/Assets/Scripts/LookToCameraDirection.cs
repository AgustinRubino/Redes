using UnityEngine;

public class LookToCameraDirection : MonoBehaviour
{
    Camera _camera;

    private void OnEnable()
    {
        _camera = Camera.main;
    }

    private void LateUpdate()
    {
        if (_camera == null) return;
        transform.forward = _camera.transform.forward.With(y: 0).normalized;
    }
}
