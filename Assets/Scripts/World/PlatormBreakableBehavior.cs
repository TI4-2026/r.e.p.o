
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

    [Header("Positions")]
    public Transform pointA;
    public Transform pointB;

    private bool isBreaking = false;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Movement()
    {
        if (!isActive || isBreaking) return;

        base.Movement();
    }

    public void OnPlayerCollision()
    {
        if (isBreaking || !isActive) return;
        if (pointA == null || pointB == null) return;

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

        Vector3 posicaoInicial = transform.position;
        Vector3 posicaoFinal = pointB.position;

        tempoAtual = 0f;

        while (tempoAtual < secondsToFall)
        {
            tempoAtual += Time.deltaTime * timeMultiplier;

            float t = Mathf.Clamp01(tempoAtual / secondsToFall);

            transform.position = Vector3.Lerp(posicaoInicial, posicaoFinal, t);

            yield return null;
        }

        transform.position = posicaoFinal;

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

            transform.position = Vector3.Lerp(posicaoFinal, posicaoInicial, t);

            yield return null;
        }

        transform.position = posicaoInicial;

        isBreaking = false;
    }
}
