using System;
using System.Collections;
using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
    [SerializeField] private GameObject keyModel;
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
            other.gameObject.GetComponent<Animator>().SetTrigger("CollectItem");
            other.gameObject.GetComponent<PlayerInteraction>().EnableKeyOnAliceHand();
            other.gameObject.GetComponent<PlayerMovement>().SetMovementEnabled(false);
            keyModel.SetActive(false);

            StartCoroutine(I_OnTriggerEnter());
        }
    }

    private IEnumerator I_OnTriggerEnter()
    {
        yield return new WaitForSeconds(1.5f);
        DialogManager.Instance.IniciarDialogo(dialog);
        Destroy(gameObject);
    }
}
