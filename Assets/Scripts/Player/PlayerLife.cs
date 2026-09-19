using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private float maxLife = 100f;
    private float life;

    //OnDamage
    private float inviciTime = 0.5f;
    private float inviciEnd = -1f;
    private bool isKnockedBack;
    private CharacterController characterController;
    private int healthProgression = 0;

    private void Start()
    {
        life = maxLife;
        characterController = GetComponent<CharacterController>();
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
    public void TakeDamage(float damage)
    {
        if (Time.time <= inviciEnd)
            return;

        life -= damage;
        healthBar.fillAmount = life/maxLife;
        inviciEnd = Time.time + inviciTime;

        if (life <= maxLife / 2 && life > maxLife / 4&&healthProgression==0)
        {
            healthProgression = 1;
            ChangeHealthBar(Color.yellow);
        } else if (life <= maxLife / 4 && life > 0&&healthProgression==1)
        {
            healthProgression = 2;
            ChangeHealthBar(Color.red);
        } else if (life <= 0)
        {
            Die();
        }
    }

    public void ApplyKnockback(Vector3 sourcePosition, float force)
    {
        if (isKnockedBack)
            return;
        Vector3 direction = (transform.position - sourcePosition);
        direction.Normalize();

        StartCoroutine(KnockbackRoutine(direction, force, inviciTime));
    }

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

    void ChangeHealthBar(Color color)
    {
        LeanTween.value(gameObject, healthBar.color, color, 1f).setEaseInOutSine().setIgnoreTimeScale(true)
        .setOnUpdate(value => healthBar.color = value);
    }

    void Die()
    {
        SceneManager.LoadScene("menu");//provisório
    }
}
