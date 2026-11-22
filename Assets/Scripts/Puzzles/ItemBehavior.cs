using System;
using System.Collections;
using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
    [SerializeField] private GameObject keyModel;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool enableExitOnCollect = true;
    
    // --------------- Default Methods ---------------

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
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
        GameManager.Instance.StartKeyDialog();
        Destroy(gameObject);
    }
}
