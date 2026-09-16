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

    private void OnEnable()
    {
        TimeTravel.OnTimeChange += UpdatePair;
    }

    private void OnDisable()
    {
        TimeTravel.OnTimeChange -= UpdatePair;
    }

    void UpdatePair(bool time)
    {
        parentPair.ChangePos(transform);
    }
}
