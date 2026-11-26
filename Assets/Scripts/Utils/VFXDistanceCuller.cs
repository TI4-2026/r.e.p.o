using UnityEngine;

/// <summary>
/// Desativa objetos de VFX quando o jogador ou a câmera está além de uma distância máxima,
/// reduzindo custo de simulação e overdraw em barreiras distantes.
/// </summary>
public class VFXDistanceCuller : MonoBehaviour
{
    [Header("Targets")]
    [SerializeField] private GameObject[] targetRoots;

    [Header("Distance Culling")]
    [SerializeField, Min(1f)] private float maxDistance = 45f;
    [SerializeField, Min(0f)] private float hysteresis = 5f;
    [SerializeField, Min(0.05f)] private float checkInterval = 0.25f;

    private Transform referenceTransform;
    private float nextCheckTime;
    private bool isWithinRange = true;

    private void OnEnable()
    {
        nextCheckTime = 0f;
        EvaluateDistance();
    }

    private void Update()
    {
        if (Time.unscaledTime < nextCheckTime)
            return;

        nextCheckTime = Time.unscaledTime + checkInterval;
        EvaluateDistance();
    }

    private void EvaluateDistance()
    {
        Transform target = GetReferenceTransform();
        if (target == null)
            return;

        float threshold = isWithinRange ? maxDistance : Mathf.Max(0f, maxDistance - hysteresis);
        float sqrDist = (target.position - transform.position).sqrMagnitude;
        bool shouldBeVisible = sqrDist <= threshold * threshold;

        if (shouldBeVisible == isWithinRange)
            return;

        isWithinRange = shouldBeVisible;
        ApplyTargetsActive(isWithinRange);
    }

    private Transform GetReferenceTransform()
    {
        if (referenceTransform != null && referenceTransform.gameObject.activeInHierarchy)
            return referenceTransform;

        if (GameManager.Instance != null && GameManager.Instance.player != null)
        {
            referenceTransform = GameManager.Instance.player.transform;
            return referenceTransform;
        }

        Camera mainCam = Camera.main;
        if (mainCam != null)
            referenceTransform = mainCam.transform;

        return referenceTransform;
    }

    private void ApplyTargetsActive(bool active)
    {
        if (targetRoots == null)
            return;

        for (int i = 0; i < targetRoots.Length; i++)
        {
            GameObject root = targetRoots[i];
            if (root != null && root.activeSelf != active)
                root.SetActive(active);
        }
    }
}
