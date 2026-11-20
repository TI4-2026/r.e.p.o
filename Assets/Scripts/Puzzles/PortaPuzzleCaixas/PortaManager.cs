using UnityEngine;

public class PortaManager : MonoBehaviour
{
    public Animation animacaoPorta;
    public int placasNecessarias = 4;

    private int placasAtivadas = 0;
    bool portaAberta = false;

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
            portaAberta = true;
        }
        else if (placasAtivadas < placasNecessarias && portaAberta)
        {
            animacaoPorta.Play(animacaoPorta.GetClip("PortaFechando").name);
            portaAberta = false;
        }
    }
}