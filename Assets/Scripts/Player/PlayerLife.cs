using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] private float maxLife = 100f;
    private float life;
    [SerializeField] private float chances = 3;
    public Image healthBar;
    public AudioClip impactSound;
    public AudioClip strikeSound;

    // OnDamage
    private float inviciTime = 1f;
    private float inviciEnd = -1f;
    private bool isKnockedBack;
    private Hud hud;
    private CharacterController characterController;
    private PlayerInteraction playerInteraction;
    private int healthProgression = 0;

    // ----------- Unity Methods -----------

    private void Awake()
    {
        GameManager.Instance.player = this.gameObject;
    }

    private void Start()
    {
        life = maxLife;
        characterController = GetComponent<CharacterController>();
        playerInteraction = GetComponent<PlayerInteraction>();
        hud = GameManager.Instance.Hud;

        hud.FlashHealthBar(hud.healthBar.color, life, maxLife);
    }

    private void OnControllerColliderHit(ControllerColliderHit collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.TryGetComponent(out ContactDamage contactDamage))
            {
                int random = Random.Range(0, 10);
                if (random % 2 == 0)
                {
                    MusicManager.PlaySound(impactSound, transform.position);
                }
                else
                {
                    MusicManager.PlaySound(strikeSound, transform.position);
                }
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

        hud.FlashHealthBar(hud.healthBar.color, life, maxLife);
            hud.ChangeHealthBar(life, maxLife);
        if (life <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        life = Mathf.Min(life + amount, maxLife);
        hud.FlashHealthBar(healthBar.color, life, maxLife);
    }

    public void ResetHealth()
    {
        life = maxLife;
        hud.FlashHealthBar(hud.healthBar.color, life, maxLife);
        hud.ChangeHealthBar(life, maxLife);
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
            if (characterController != null && characterController.enabled)
            {
                characterController.Move(direction * currentForce * Time.deltaTime);
            }
            elapsed += Time.deltaTime;

            yield return null;
        }

        isKnockedBack = false;
    }

    private void Die()
    {
        StopAllCoroutines();
        isKnockedBack = false;
        GameManager.Instance.KillPlayer(gameObject);
        Time.timeScale = 0f;
        life = maxLife;
        chances -= 1;
        playerInteraction.UnlockCursor();
        hud.LostChance(chances + 1, chances, () =>
        {
            if (chances > 0 && playerInteraction != null)
            {
                playerInteraction.LockCursor();
                ResetHealth();
            }
            else
            {
                playerInteraction.UnlockCursor();
                SceneManager.LoadSceneAsync("Menu");
            }
        });
    }
}
