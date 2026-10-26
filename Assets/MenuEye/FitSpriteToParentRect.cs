using UnityEngine;

//Codigo para fazer o cabelo se comportar que nem os outros elementos, pq ele deforma estranho se n tiver isso
[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public class FitSpriteToParentRect : MonoBehaviour
{
    SpriteRenderer sr;
    RectTransform parentRect;

    void LateUpdate()
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        if (parentRect == null) parentRect = transform.parent as RectTransform;
        if (sr.sprite == null || parentRect == null) return;

        Vector2 spriteSize = sr.sprite.bounds.size;
        Vector2 rectSize = parentRect.rect.size;

        Vector3 scale = new Vector3(rectSize.x / spriteSize.x, rectSize.y / spriteSize.y, 1f);
        transform.localScale = scale;

        Vector2 offset = Vector2.Scale(sr.sprite.bounds.center, scale);
        transform.localPosition = new Vector3(
            parentRect.rect.center.x - offset.x,
            parentRect.rect.center.y - offset.y,
            transform.localPosition.z);
    }
}