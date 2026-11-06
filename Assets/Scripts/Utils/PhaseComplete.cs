using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TransitionSceneTrigger : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (MusicManager.instance != null)
            {
                MusicManager.instance.PlayMusic(0);
            }
            
            StartCoroutine(I_SceneTransition());
        }
    }
    
    private IEnumerator I_SceneTransition()
    {
        yield return SceneManager.LoadSceneAsync(nextSceneName);
    }
}
