using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTimeControl : MonoBehaviour
{

    TimeManager timeManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeManager = TimeManager.Instance;
    }

    public void TimeStop(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            timeManager.SetTimeMultiplier(0f);
            StartCoroutine(ChangeBack(5f));
        }
    }

    public void SnailTime(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            timeManager.SetTimeMultiplier(0.3f);
            StartCoroutine(ChangeBack(10f));
        }
    }

    IEnumerator ChangeBack(float time)
    {
        yield return new WaitForSeconds(time);
        timeManager.SetTimeMultiplier(1f);
    }
}
