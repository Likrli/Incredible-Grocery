using UnityEngine;

public class CameraConstantWidth : MonoBehaviour
{
    [SerializeField] private Vector2 DefaultResolution = new Vector2(1920, 1080);
    [Range(0f, 1f)] [SerializeField] private float WidthOrHeight = 0;

    private Camera _componentCamera;

    private float _initialSize;
    private float _targetAspect;

    private void Start()
    {
        _componentCamera = GetComponent<Camera>(); 

        _initialSize = _componentCamera.orthographicSize;
        _targetAspect = DefaultResolution.x / DefaultResolution.y;

        float p_constantWidthSize = _initialSize * (_targetAspect / _componentCamera.aspect);
        _componentCamera.orthographicSize = Mathf.Lerp(p_constantWidthSize, _initialSize, WidthOrHeight);
    }
}
