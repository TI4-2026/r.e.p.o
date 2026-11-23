using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TransitionSceneTrigger : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    [SerializeField] private bool isExit;
    
    private bool isEnabled;
    
    // ----------------- Default Methods -----------------

    private void Start()
    {
        isEnabled = true;
        if (isExit)
        {
            GameManager.Instance.SetExit(this);
            isEnabled = false;
        }
    }

    // ----------------- Public Methods -----------------
    
    public void SetEnabled(bool value)
    {
        isEnabled = value;
    }
    
    // ----------------- Private Methods -----------------
    
    private void OnTriggerEnter(Collider other)
    {
        if (!isEnabled) return;
        
        if (other.CompareTag("Player"))
        {
            if (MusicManager.instance != null) MusicManager.instance.PlayMusic(0);
            StartCoroutine(I_SceneTransition(other.gameObject));
        }
    }
    
    private IEnumerator I_SceneTransition(GameObject player)
    {
        player.GetComponent<PlayerMovement>().SetMovementEnabled(false);
        player.GetComponent<PlayerInteraction>().UnlockCursor();
        GameManager.Instance.Hud.BlackFadeFinishOnMiddle(duration: 0.75f);

        yield return new WaitForSeconds(1f);
        
        SceneManager.LoadSceneAsync(nextSceneName);
    }
}
