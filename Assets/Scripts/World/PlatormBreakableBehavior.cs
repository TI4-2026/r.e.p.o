
using UnityEngine;
using System.Collections;

public class PlatormBreakableBehavior : PlatformBehavior, ITimeAffectable
{
    [Header("Breakable Attributes")]
    [SerializeField] private float secondsToBreak = 1f;
    [SerializeField] private float secondsToFall = 2f;
    [SerializeField] private float secondsToReset = 3f;
    [SerializeField] private float secondsToReturn = 2f;
    [SerializeField] private bool isActive = true;

    [Header("Fall Settings")]
    [SerializeField] private float fallDistance = 10f;

    private Vector3 pointA;
    private Vector3 pointB;

    private bool isBreaking = false;

    protected override void Awake()
    {
        base.Awake();
        TimeTravel.OnTimeChange.AddListener(ResetPlatform);
        pointA = transform.position;
        pointB = pointA + Vector3.down * fallDistance;
    }

    protected override void Movement()
    {
    }

    public void OnPlayerCollision()
    {
        if (isBreaking || !isActive) return;

        StartCoroutine(I_BreakPlatform());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerCollision();
        }
    }

    private IEnumerator I_BreakPlatform()
    {
        isBreaking = true;

        float tempoAtual = 0f;

        while (tempoAtual < secondsToBreak)
        {
            tempoAtual += Time.deltaTime * timeMultiplier;
            yield return null;
        }

        tempoAtual = 0f;

        while (tempoAtual < secondsToFall)
        {
            tempoAtual += Time.deltaTime * timeMultiplier;

            float t = Mathf.Clamp01(tempoAtual / secondsToFall);

            transform.position = Vector3.Lerp(pointA, pointB, t);

            yield return null;
        }

        transform.position = pointB;

        tempoAtual = 0f;

        while (tempoAtual < secondsToReset)
        {
            tempoAtual += Time.deltaTime * timeMultiplier;
            yield return null;
        }

        tempoAtual = 0f;

        while (tempoAtual < secondsToReturn)
        {
            tempoAtual += Time.deltaTime * timeMultiplier;

            float t = Mathf.Clamp01(tempoAtual / secondsToReturn);

            transform.position = Vector3.Lerp(pointB, pointA, t);

            yield return null;
        }

        transform.position = pointA;

        isBreaking = false;
    }

    private void ResetPlatform()
    {
        StopAllCoroutines();
        transform.position = pointA;
    }
}
