using UnityEngine;

public class InteruptorScript : MonoBehaviour
{
    public PortaManager portaManager;
    public Renderer meshRenderer;

    private bool ativada = false;
    private bool temCaixa = false;

    void Start()
    {
        meshRenderer.material.color = Color.red;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Caixa"))
        {
            temCaixa = true;
        }
    }

    void FixedUpdate()
    {
        if (temCaixa && !ativada)
        {
            ativada = true;
            meshRenderer.material.color = Color.green;
            portaManager.AtualizarPlaca(true);
        }
        else if (!temCaixa && ativada)
        {
            ativada = false;
            meshRenderer.material.color = Color.red;
            portaManager.AtualizarPlaca(false);
        }

        temCaixa = false;
    }
}