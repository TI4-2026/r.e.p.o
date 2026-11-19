using UnityEngine;

public class InteruptorScript : MonoBehaviour
{
    public PortaManager portaManager;
    public Renderer meshRenderer;

    [Header("Cores da Emissão")]
    public Color corDesativada = Color.red;
    public Color corAtivada = Color.cyan;

    [Header("Intensidade da Emissão")]
    [Range(0f, 10f)]
    public float intensidadeEmissao = 2f;

    private bool ativada = false;
    private bool temCaixa = false;

    private Material material;

    void Start()
    {
        material = meshRenderer.material;
        material.EnableKeyword("_EMISSION");
        AtualizarEmissao(corDesativada);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Caixa"))
        {
            temCaixa = true;
        }
    }

    void AtualizarEmissao(Color cor)
    {
        material.SetColor("_EmissionColor", cor * intensidadeEmissao);
    }

    void FixedUpdate()
    {
        if (temCaixa && !ativada)
        {
            ativada = true;
            AtualizarEmissao(corAtivada);
            portaManager.AtualizarPlaca(true);
        }
        else if (!temCaixa && ativada)
        {
            ativada = false;
            AtualizarEmissao(corDesativada);
            portaManager.AtualizarPlaca(false);
        }

        temCaixa = false;
    }
}