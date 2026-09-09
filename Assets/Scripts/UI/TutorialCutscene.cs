using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TutorialCutscene : MonoBehaviour
{
    public RectTransform tip;
    public Vector2 maxSize = new Vector2(10, 10);

    [SerializeField] private Volume globalVolume;

    [Header("Vinheta")]
    [SerializeField] private Color vignetteColor = Color.blue;
    [SerializeField] private float minIntensity = 0f;
    [SerializeField] private float maxIntensity = 0.45f;
    [SerializeField] private float pulseDuration = 0.5f;

    private Vignette vignette;

    void Awake()
    {
        globalVolume.profile = Instantiate(globalVolume.sharedProfile);

        if (!globalVolume.profile.TryGet(out vignette))
        {
            Debug.LogError("Cade a vinheta?");
            return;
        }

        vignette.color.value = vignetteColor;
        vignette.intensity.value = minIntensity;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tip.gameObject.SetActive(true);
            Time.timeScale = 0.05f;
            LeanTween.scale(tip, maxSize, 1f).setLoopPingPong().setIgnoreTimeScale(true).setEaseInOutCirc();
            LeanTween.value(gameObject, minIntensity, maxIntensity, pulseDuration).setEaseInOutSine().setLoopPingPong()
            .setIgnoreTimeScale(true)
            .setOnUpdate(value =>
            {
                vignette.intensity.value = value;
            });

        }
    }

    public void TutorialComplete(InputAction.CallbackContext context)
    {
        if (LeanTween.isTweening(tip))
        {
            LeanTween.cancel(gameObject);
            vignette.intensity.value = 0.2f;
            Time.timeScale = 1f;
            LeanTween.cancel(tip);
            Destroy(tip.gameObject);
            Destroy(gameObject);
        }
    }
}
