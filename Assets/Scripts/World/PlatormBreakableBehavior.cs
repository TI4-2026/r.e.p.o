using UnityEngine;
using System.Collections;
public class PlatormBreakableBehavior : PlatformBehavior
{
    [Header("Breakable Attributes")]

    [SerializeField] private float secondsToBreak;
    [SerializeField] private float secondsToReset;
    [SerializeField] private bool isActive = true;

    private BoxCollider boxCollider;
    private MeshRenderer meshRenderer;

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

    // -------------------- Public Methods --------------------

    public void OnPlayerCollision(GameObject go)
    {
        StartCoroutine(I_BreakPlatform());
    }

    // -------------------- Enumerators Methods --------------------

    private IEnumerator I_BreakPlatform()
    {
        yield return new WaitForSeconds(secondsToBreak);
        boxCollider.enabled = false;
        meshRenderer.enabled = false;
        transform.position = startPosition;
        yield return new WaitForSeconds(secondsToReset);
        boxCollider.enabled = true;
        meshRenderer.enabled = true;
    }
}
