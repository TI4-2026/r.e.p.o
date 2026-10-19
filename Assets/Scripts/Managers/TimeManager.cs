using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    private readonly List<ITimeAffectable> targets = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        RegisterSceneObjects();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RegisterSceneObjects();
    }

    private void RegisterSceneObjects()
    {
        targets.Clear();
        foreach (MonoBehaviour behaviour in FindObjectsByType<MonoBehaviour>
            (FindObjectsSortMode.None))
        {
            if (behaviour is ITimeAffectable target)
            {
                targets.Add(target);
            }
        }
    }

    public void Register(ITimeAffectable timeAffectable)
    {
        targets.Add(timeAffectable);
    }

    public void Unregister(ITimeAffectable timeAffectable)
    {
        targets.Remove(timeAffectable);
    }

    public void SetTimeMultiplier(float multiplier)
    {
        Debug.Log(multiplier);
        foreach (ITimeAffectable target in targets)
        {
            target.timeMultiplier = multiplier;
        }
    }

    private void OnDestroy()
    {
        if (Instance != this)
            return;

        SceneManager.sceneLoaded -= OnSceneLoaded;
        Instance = null;
    }
}