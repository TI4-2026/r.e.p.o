
using UnityEngine;

public class SpawnadorCaixa : MonoBehaviour, ITimeAffectable
{
    [Header("Spawn")]
    public GameObject caixa;
    public float cooldown = 10f;

    public float timeMultiplier { get; set; } = 1f;

    public float tempoAtual = 0f;

    void Update()
    {
        tempoAtual += Time.deltaTime * timeMultiplier;

        if (tempoAtual >= cooldown)
        {
            Instantiate(caixa, transform.position, transform.rotation, transform);
            tempoAtual = 0f;
        }
    }
}

