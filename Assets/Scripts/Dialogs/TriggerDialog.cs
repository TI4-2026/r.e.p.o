using UnityEngine;

public class TriggerDialog : PlayerCollisionObject
{
    public DialogData dialogo;
    
    private bool dialogLido = false;
    
    // -------------- Default Methods --------------
    protected override void OnPlayerTriggerEnter(GameObject player)
    {
        if (dialogLido)
            return;

        dialogLido = true;
        DialogManager.Instance.IniciarDialogo(dialogo);
    }
}
