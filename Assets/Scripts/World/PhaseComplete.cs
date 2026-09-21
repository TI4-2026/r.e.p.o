using UnityEngine;
using UnityEngine.SceneManagement;

public class PhaseComplete : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SceneManager.LoadScene("museu");
        }
    }
}
