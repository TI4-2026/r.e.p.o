
using UnityEngine;

public class DisableOnTargetDestroy : MonoBehaviour
{
    public GameObject target;

    void Update()
    {
        if (target == null)
        {
            gameObject.SetActive(false);
        }
    }
}
