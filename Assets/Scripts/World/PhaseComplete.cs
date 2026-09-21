using UnityEngine;
using UnityEngine.SceneManagement;

public class PhaseComplete : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            MusicManager.instance.PlayMusic(0);
            SceneManager.LoadScene("museu");
        }
    }
}
