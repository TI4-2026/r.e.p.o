using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public abstract class TutorialTrigger : MonoBehaviour
{
    [Header("Persistência")]
    [SerializeField] private int tutorialId;

    [Header("Vinheta")]
    public RectTransform tip;
    public Vector2 maxSize = new Vector2(10, 10);

    [SerializeField] private Volume globalVolume;
    [SerializeField] private Color vignetteColor = Color.blue;
    [SerializeField] private float minIntensity = 0f;
    [SerializeField] private float maxIntensity = 0.45f;
    [SerializeField] private float pulseDuration = 0.5f;

    private Vignette vignette;

    private const string PrefsPrefix = "TutorialDone_";

    protected virtual void Awake()
    {
        /*if (PlayerPrefs.GetInt(PrefsPrefix + tutorialId, 0) == 1)
        {
            gameObject.SetActive(false);
            return;
        }*/

        globalVolume.profile = Instantiate(globalVolume.sharedProfile);

        if (!globalVolume.profile.TryGet(out vignette))
        {
            Debug.LogError("Cade a vinheta?");
            return;
        }

        vignette.color.value = vignetteColor;
        vignette.intensity.value = minIntensity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        tip.gameObject.SetActive(true);
        Time.timeScale = 0.05f;

        LeanTween.scale(tip, maxSize, 1f)
            .setLoopPingPong().setIgnoreTimeScale(true).setEaseInOutCirc();

        LeanTween.value(gameObject, minIntensity, maxIntensity, pulseDuration)
            .setEaseInOutSine().setLoopPingPong().setIgnoreTimeScale(true)
            .setOnUpdate(value => vignette.intensity.value = value);
    }

    public void TutorialComplete(InputAction.CallbackContext context)
    {
        if (!LeanTween.isTweening(tip)) return;

        LeanTween.cancel(gameObject);
        LeanTween.cancel(tip);
        vignette.intensity.value = 0.2f;
        Time.timeScale = 1f;

        OnTutorialCompleted();

        /*PlayerPrefs.SetInt(PrefsPrefix + tutorialId, 1);
        PlayerPrefs.Save();*/
    }

    protected virtual void OnTutorialCompleted() { }
}