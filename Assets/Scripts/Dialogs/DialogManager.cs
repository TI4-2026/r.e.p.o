using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;
public class DialogManager : MonoBehaviour
{
    public TMP_Text texto;
    public DialogData dialog;
    public GameObject dialogBox;
    PlayerMovement controller;

    float tempoPorLetra = 0.03f;

    bool animandoTexto;
    string textoAtualCompleto;
    LTDescr tweenAtual;
    bool dialogoAberto = false;
    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void IniciarDialogo(DialogData dialogoInicial)
    {
        controller.SetMovementEnabled(false);
        dialogBox.SetActive(true);
        dialogBox.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        LeanTween.scale(dialogBox, Vector3.one, 0.5f)
            .setEase(LeanTweenType.easeOutBack)
            .setIgnoreTimeScale(true)
            .setDelay(1 * 0.3f);
        dialog = dialogoInicial;
        if (dialog == null)
        {
            Debug.LogError("Dialogo inicial é nulo");
            return;
        }
        Debug.Log("Texto recebido: " + dialog.text);
        dialogoAberto = true;   
        MostrarTextoAnimado(dialog.text);
    }
    public void Proximo(InputAction.CallbackContext context)
    {
        if (dialogoAberto == false)
            return;
        if (context.performed)
        {     
        if (animandoTexto)
        {
            MostrarTextoInstantaneo();
            return;
        }
        if (dialog.nextDialog[0] == null)
        {
            dialogBox.SetActive(false);
            controller.SetMovementEnabled(true);
            dialogoAberto = false;  
        }
        else
        {
            dialog = dialog.nextDialog[0];
            MostrarTextoAnimado(dialog.text);
        }
        }
    }

    public void Voltar(InputAction.CallbackContext context)
    {
        if (dialogoAberto == false)
            return;
        if (context.performed)
        {        
        if (animandoTexto)
        {
            MostrarTextoInstantaneo();
            return;
        }
        dialog = dialog.nextDialog[1];
        MostrarTextoAnimado(dialog.text);
        }
    }
     void MostrarTextoAnimado(string novoTexto)
    {
        if (tweenAtual != null)
            LeanTween.cancel(gameObject);

        StopAllCoroutines();

        textoAtualCompleto = novoTexto;
        animandoTexto = true;

        texto.text = novoTexto;
        texto.maxVisibleCharacters = 0;

        int totalCaracteres = novoTexto.Length;
        float duracao = totalCaracteres * tempoPorLetra;

        tweenAtual = LeanTween.value(gameObject, 0, totalCaracteres, duracao)
            .setEase(LeanTweenType.linear)
            .setOnUpdate((float valor) =>
            {
                int letrasVisiveis = Mathf.FloorToInt(valor);

                if (letrasVisiveis != texto.maxVisibleCharacters)
                {
                    texto.maxVisibleCharacters = letrasVisiveis;

                    if (letrasVisiveis > 0)
                    {
                        StartCoroutine(PularLetra(letrasVisiveis - 1));
                    }
                }
            })
            .setOnComplete(() =>
            {
                animandoTexto = false;
                texto.maxVisibleCharacters = totalCaracteres;
                texto.ForceMeshUpdate();
                texto.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
            });
    }
    void MostrarTextoInstantaneo()
    {
        if (tweenAtual != null)
            LeanTween.cancel(gameObject);

        StopAllCoroutines();

        animandoTexto = false;
        texto.text = textoAtualCompleto;
        texto.maxVisibleCharacters = textoAtualCompleto.Length;
        texto.ForceMeshUpdate();
        texto.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
    }

    public IEnumerator PularLetra(int index)
    {
        texto.ForceMeshUpdate();

        TMP_TextInfo textInfo = texto.textInfo;

        if (index >= textInfo.characterCount) yield break;
        if (!textInfo.characterInfo[index].isVisible) yield break;

        TMP_CharacterInfo charInfo = textInfo.characterInfo[index];
        int materialIndex = charInfo.materialReferenceIndex;
        int vertexIndex = charInfo.vertexIndex;

        Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;
        Vector3[] originalVertices = new Vector3[4];

        for (int i = 0; i < 4; i++)
            originalVertices[i] = vertices[vertexIndex + i];

        float duracao = tempoPorLetra;
        float tempo = 0f;

        while (tempo < duracao)
        {
            tempo += Time.deltaTime;

            float offsetY = Mathf.Sin((tempo / duracao) * Mathf.PI) * 16.67f;

            for (int i = 0; i < 4; i++)
                vertices[vertexIndex + i] = originalVertices[i] + new Vector3(0, offsetY, 0);

            texto.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
            yield return null;
        }

        for (int i = 0; i < 4; i++)
            vertices[vertexIndex + i] = originalVertices[i];

        texto.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
    }
}
