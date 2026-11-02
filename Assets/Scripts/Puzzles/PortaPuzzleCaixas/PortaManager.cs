using UnityEngine;

public class PortaManager : MonoBehaviour
{
    public GameObject portaPrefab;
    public int placasNecessarias = 4;

    public GameObject porta;
    private Vector3 posicaoInicial;
    private Quaternion rotacaoInicial;

    private int placasAtivadas = 0;

    void Start()
    {
        posicaoInicial = porta.transform.position;
        rotacaoInicial = porta.transform.rotation;
    }

    public void AtualizarPlaca(bool ativada)
    {
        if (ativada)
        {
            placasAtivadas++;
        }
        else
        {
            placasAtivadas--;
        }

        if (placasAtivadas >= placasNecessarias)
        {
            if (porta != null)
            {
                Destroy(porta);
                porta = null;
            }
        }
        else
        {
            if (porta == null)
            {
                porta = Instantiate(portaPrefab, posicaoInicial, rotacaoInicial);
            }
        }
    }
}