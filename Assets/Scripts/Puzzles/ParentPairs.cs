using UnityEngine;

public class ParentPairs : MonoBehaviour
{
    private Transform spawnPos;
    public Transform futurePos;
    public Transform pastPos;
    void Start()
    {
        spawnPos = gameObject.transform;
    }

    public void ChangePos(Transform target)
    {
        Vector3 offset = target.position - gameObject.transform.position;
        spawnPos.position += offset;
        futurePos.localPosition = Vector3.zero;
        pastPos.localPosition = Vector3.zero;
        Debug.Log("tchau");
    }
}
