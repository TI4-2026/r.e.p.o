using UnityEngine;

public class MoveSpawn : MonoBehaviour
{
    private ParentPairs parentPair;
    private Vector3 lastPos;
    public bool isMoving;
    void Start()
    {
        parentPair = GetComponentInParent<ParentPairs>();
        lastPos = transform.position;
    }
    

    void Update()
    {
        if(Vector3.Distance(transform.position, lastPos) >= 0.001f)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        if (isMoving)
            parentPair.ChangePos(transform);
    }
}
