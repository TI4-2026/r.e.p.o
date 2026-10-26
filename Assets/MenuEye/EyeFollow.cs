using UnityEngine;

public class EyeFollow : MonoBehaviour
{
    public RectTransform pupil;    
    public RectTransform area;    
    public Canvas canvas;           
    public float Intensity = 40f;   
    public float Speed = 15f;

    void Update()
    {
        if (pupil == null || area == null || canvas == null) return;

        Camera cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay
            ? null
            : canvas.worldCamera;

        Vector2 localMouse;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            area, Input.mousePosition, cam, out localMouse);

        Vector2 target = Vector2.ClampMagnitude(localMouse, Intensity);

        pupil.anchoredPosition = Vector2.Lerp(
            pupil.anchoredPosition, target, Speed * Time.deltaTime);
    }
}