using UnityEngine;

public class TotemEvent : MonoBehaviour
{
    public GameObject obj;
    public void FireEvent()
    {
        obj.SetActive(false);
        Debug.Log("fiz");
    }
}
