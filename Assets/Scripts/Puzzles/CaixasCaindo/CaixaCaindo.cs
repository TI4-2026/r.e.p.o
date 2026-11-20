
using UnityEngine;

public class Caixa : MonoBehaviour, ITimeAffectable
{
    public float tempoDeVida = 20f;

    public float timeMultiplier { get; set; } = 1f;

    private float tempoAtual = 0f;

    public Rigidbody rb;

    private RigidbodyConstraints constraintsOriginais;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        constraintsOriginais = rb.constraints;

        TimeManager.Instance.Register(this);
    }

    void Update()
    {
        tempoAtual += Time.deltaTime * timeMultiplier;

        if (tempoAtual >= tempoDeVida)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        if (timeMultiplier == 0f)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            rb.constraints = constraintsOriginais;
        }
    }

    void OnDestroy()
    {
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.Unregister(this);
        }
    }
}
