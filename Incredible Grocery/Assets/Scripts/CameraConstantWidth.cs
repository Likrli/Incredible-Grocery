using UnityEngine;

public class CameraConstantWidth : MonoBehaviour
{
    public Vector2 DefaultResolution = new Vector2(1920, 1080);
    [Range(0f, 1f)] public float WidthOrHeight = 0;

    private Camera p_componentCamera;

    private float p_initialSize;
    private float p_targetAspect;

    private void Start()
    {
        p_componentCamera = GetComponent<Camera>(); 

        p_initialSize = p_componentCamera.orthographicSize;
        p_targetAspect = DefaultResolution.x / DefaultResolution.y;

        float p_constantWidthSize = p_initialSize * (p_targetAspect / p_componentCamera.aspect);
        p_componentCamera.orthographicSize = Mathf.Lerp(p_constantWidthSize, p_initialSize, WidthOrHeight);
    }
}
