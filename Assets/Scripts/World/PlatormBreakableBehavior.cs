
using UnityEngine;
using System.Collections;

public class PlatormBreakableBehavior : PlatformBehavior, ITimeAffectable
{
    [Header("Breakable Attributes")]
    [SerializeField] private float secondsToBreak;
    [SerializeField] private float secondsToReset;
    [SerializeField] private bool isActive = true;

    private BoxCollider boxCollider;
    private MeshRenderer meshRenderer;

    private bool isBreaking = false;

    protected override void Awake()
    {
        base.Awake();

        boxCollider = GetComponent<BoxCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    protected override void Movement()
    {
        if (!isActive) return;

        base.Movement();
    }

    protected void OnEnable()
    {
        if (boxCollider != null)
            boxCollider.enabled = true;

        if (meshRenderer != null)
            meshRenderer.enabled = true;
    }
    // -------------------- Public Methods --------------------

    public void OnPlayerCollision()
    {
        if (isBreaking || !isActive) return;

        StartCoroutine(I_BreakPlatform());
    }

    // -------------------- Enumerators Methods --------------------

    private IEnumerator I_BreakPlatform()
    {
        isBreaking = true;

        float tempoAtual = 0f;

        while (tempoAtual < secondsToBreak)
        {
            tempoAtual += Time.deltaTime * timeMultiplier;
            yield return null;
        }

        boxCollider.enabled = false;
        meshRenderer.enabled = false;

        transform.position = startPosition;

        tempoAtual = 0f;

        while (tempoAtual < secondsToReset)
        {
            tempoAtual += Time.deltaTime * timeMultiplier;
            yield return null;
        }

        boxCollider.enabled = true;
        meshRenderer.enabled = true;

        isBreaking = false;
    }
}
