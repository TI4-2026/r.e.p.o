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
            StartCoroutine(I_SceneTransition());
        }
    }
    
    private IEnumerator I_SceneTransition()
    {
        yield return SceneManager.LoadSceneAsync(nextSceneName);
    }
}
