using Unity.Multiplayer.Center.Common.Analytics;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public bool isInReach;
    public UnityEvent inetractAction;


    void Update()
    {
        if (isInReach)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                inetractAction.Invoke();
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInReach = true;
        }
    }
}
