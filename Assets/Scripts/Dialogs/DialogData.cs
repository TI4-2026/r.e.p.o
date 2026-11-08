using UnityEngine;

[CreateAssetMenu(fileName = "DialogData", menuName = "Scriptable Objects/DialogData")]
public class DialogData : ScriptableObject
{
    [TextArea(2, 10)] public string text;
    public DialogData[] nextDialog;
}
