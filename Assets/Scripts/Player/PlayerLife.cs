using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] private float maxLife = 100f;
    private float life;
    public Image healthBar;

    // OnDamage
    [SerializeField] private float inviciTime = 0.5f;
    private float inviciEnd = -1f;
    private bool isKnockedBack;
    private Hud hud;
    private CharacterController characterController;
    private PlayerCamera playerCamera;
    private int healthProgression = 0;

    // ----------- Unity Methods -----------

    private void Start()
    {
        life = maxLife;
        characterController = GetComponent<CharacterController>();
        playerCamera = GetComponent<PlayerCamera>();
        hud = GameManager.Instance.Hud;

        FlashHealthBar(healthBar.color);
    }

    private void OnControllerColliderHit(ControllerColliderHit collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.TryGetComponent(out ContactDamage contactDamage))
            {
                TakeDamage(contactDamage.damage);
                ApplyKnockback(collision.transform.position, contactDamage.knockbackPower);
            }
        }
    }

    // ----------- Public Methods -----------

    public void TakeDamage(float damage)
    {
        if (Time.time <= inviciEnd)
            return;

        life = Mathf.Max(life - damage, 0f);
        inviciEnd = Time.time + inviciTime;

        FlashHealthBar(healthBar.color);

        if (life <= maxLife / 2 && life > maxLife / 4 && healthProgression == 0)
        {
            healthProgression = 1;
            ChangeHealthBar(Color.yellow);
        }
        else if (life <= maxLife / 4 && life > 0 && healthProgression == 1)
        {
            healthProgression = 2;
            ChangeHealthBar(Color.red);
        }
        else if (life <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        life = Mathf.Min(life + amount, maxLife);
        FlashHealthBar(healthBar.color);
    }

    public void ResetHealth()
    {
        life = maxLife;
        FlashHealthBar(healthBar.color);
    }

    public void ApplyKnockback(Vector3 sourcePosition, float force)
    {
        if (isKnockedBack)
            return;
        Vector3 direction = (transform.position - sourcePosition);
        direction.Normalize();

        StartCoroutine(KnockbackRoutine(direction, force, inviciTime));
    }

    // ----------- Private Methods -----------

    private IEnumerator KnockbackRoutine(Vector3 direction, float force, float duration)
    {
        isKnockedBack = true;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float currentForce = Mathf.Lerp(force, 0f, t);
            characterController.Move(direction * currentForce * Time.deltaTime);
            elapsed += Time.deltaTime;

            yield return null;
        }

        isKnockedBack = false;
    }

    void FlashHealthBar(Color color)
    {
        LeanTween.value(healthBar.gameObject, color, Color.white, 0.1f)
        .setIgnoreTimeScale(true).setEaseInOutSine()
        .setOnUpdate(value => healthBar.color = value)
        .setOnComplete(() =>
        {
            LeanTween.value(healthBar.gameObject, Color.white, color, 0.1f)
            .setIgnoreTimeScale(true).setEaseInOutSine()
            .setOnUpdate(value => healthBar.color = value);
        });

        RectTransform rect = healthBar.rectTransform;
        Vector3 originalPosition = rect.anchoredPosition;

        LeanTween.move(rect, originalPosition + new Vector3(10f, 5f, 0f), 0.05f)
            .setIgnoreTimeScale(true).setLoopPingPong(5).setEaseInOutSine()
            .setOnComplete(() =>
            {
                healthBar.fillAmount = life / maxLife;
                rect.anchoredPosition = originalPosition;
            });
    }

    void ChangeHealthBar(Color color)
    {
        LeanTween.value(gameObject, healthBar.color, color, 1f).setEaseInOutSine().setIgnoreTimeScale(true)
        .setOnUpdate(value => healthBar.color = value);
    }

    private void Die()
    {
        playerCamera.UnlockCursor();
        SceneManager.LoadScene("menu"); // provisório
    }
}
