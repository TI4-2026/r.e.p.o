using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToBunker : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("Bunker");
        }
    }
}
