using System;
using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool enableExitOnCollect = true;
    private DialogData dialog;
    
    // --------------- Default Methods ---------------

    private void Start()
    {
        GameManager.Instance.SetKey(this);
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
    
    // --------------- Public Methods ---------------

    public void SetDialog(DialogData dialog)
    {
        this.dialog = dialog;
    }
    
    // --------------- Private Methods ---------------

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.CollectKey(enableExit: enableExitOnCollect);
            DialogManager.Instance.IniciarDialogo(dialog);
            Destroy(gameObject);
        }
    }
}
