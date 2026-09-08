using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TutorialCutscene : MonoBehaviour
{
    public RectTransform tip;
    public Vector2 maxSize = new Vector2(10, 10);

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tip.gameObject.SetActive(true);
            Time.timeScale = 0.05f;
            LeanTween.scale(tip, maxSize, 1f).setLoopPingPong().setIgnoreTimeScale(true);

        }
    }

    public void TutorialComplete(InputAction.CallbackContext context)
    {
        if (LeanTween.isTweening(tip))
        {
            Time.timeScale = 1f;
            LeanTween.cancel(tip);
            Destroy(tip.gameObject);
            Destroy(gameObject);
        }
    }
}
