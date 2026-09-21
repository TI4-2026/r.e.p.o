using UnityEngine;
using UnityEngine.SceneManagement;

public class PhaseComplete : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if(MusicManager.instance!=null)
                MusicManager.instance.PlayMusic(0);

            SceneManager.LoadScene("museu");
        }
    }
}
