using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] private float maxLife = 100f;
    private float life;

    // OnDamage
    [SerializeField] private float inviciTime = 0.5f;
    private float inviciEnd = -1f;
    private bool isKnockedBack;
    private Hud hud;
    private CharacterController characterController;
    private PlayerCamera playerCamera;

    // ----------- Unity Methods -----------

    private void Start()
    {
        life = maxLife;
        characterController = GetComponent<CharacterController>();
        playerCamera = GetComponent<PlayerCamera>();
        hud = GameManager.Instance.Hud;

        UpdateHudHealth();
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

        UpdateHudHealth();

        if (life <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        life = Mathf.Min(life + amount, maxLife);
        UpdateHudHealth();
    }

    public void ResetHealth()
    {
        life = maxLife;
        UpdateHudHealth();
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

    private void UpdateHudHealth()
    {
        hud.UpdateHealthSlider(life, maxLife);
    }

    private void Die()
    {
        playerCamera.UnlockCursor();
        SceneManager.LoadScene("menu"); // provisório
    }
}
