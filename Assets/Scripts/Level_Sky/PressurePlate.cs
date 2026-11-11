using UnityEngine;
public class PressurePlate : MonoBehaviour
{
    public MeshRenderer rend;
    public GameObject obj;
    void Start()
    {
     rend = gameObject.GetComponent<MeshRenderer>();
    }
    void OnCollisionEnter(Collision collision)
    {
        rend.material.color=Color.green;
        
    }
    void OnCollisionExit(Collision collision)
    {
    rend.material.color=Color.red;    
    }
    
}
