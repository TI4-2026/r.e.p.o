using UnityEngine;

public class MoveSpawn : MonoBehaviour
{
    //private ParentPairs parentPair;
    private Vector3 lastPos;
    public bool isMoving;
    void Start()
    {
        // parentPair = GetComponentInParent<ParentPairs>();
        lastPos = transform.position;
    }
    /*

    private void OnEnable()
    {
        TimeTravel.OnTimeChange.AddListener(UpdatePair);
    }

    private void OnDisable()
    {
        TimeTravel.OnTimeChange.RemoveListener(UpdatePair);
    }

    void UpdatePair()
    {
        parentPair.AlternateBehavior(TimeTravel.isFuture);
    }
    */
}
