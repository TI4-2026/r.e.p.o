using UnityEngine;

public class MoveSpawn : MonoBehaviour
{
    private ParentPairs parentPair;
    void Start()
    {
        parentPair = GetComponentInParent<ParentPairs>();
    }

    void OnCollisionEnter(Collision collision)
    {
        parentPair.ChangePos(gameObject.transform);
        Debug.Log("Oi");
    }
}
