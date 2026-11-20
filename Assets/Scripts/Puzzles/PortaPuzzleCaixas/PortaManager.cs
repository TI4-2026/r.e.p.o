using UnityEngine;

public class PortaManager : MonoBehaviour
{
    public Animation animacaoPorta;
    public int placasNecessarias = 4;

    private int placasAtivadas = 0;

    void Start()
    {
        animacaoPorta.Play(animacaoPorta.GetClip("PortaFechando").name);
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
            animacaoPorta.Play(animacaoPorta.GetClip("PortaAbrindo").name);
        }
        else
        {
            animacaoPorta.Play(animacaoPorta.GetClip("PortaFechando").name);
        }
    }
}