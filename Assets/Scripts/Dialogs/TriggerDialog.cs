using UnityEngine;

public class TriggerDialog : PlayerCollisionObject
{
    bool dialogLido = false;
    public DialogData dialogo;
    void Start()
    {
        
    }
    protected override void OnPlayerTriggerEnter(GameObject player)
    {
        if (dialogLido)
            return;

        DialogManager dialogManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DialogManager>();
        dialogLido = true;
        dialogManager.IniciarDialogo(dialogo);
    }
}
