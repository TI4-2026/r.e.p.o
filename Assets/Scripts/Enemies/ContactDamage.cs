using UnityEngine;

public class ContactDamage : PlayerCollisionObject
{
    public float damage = 10f;
    public float knockbackPower = 10f;
    public AudioClip impactSound;
    public AudioClip strikeSound;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (impactSound != null)
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
            }
            
            if (collision.gameObject.TryGetComponent(out PlayerLife playerLife))
            {
                playerLife.TakeDamage(damage);
                playerLife.ApplyKnockback(transform.position, knockbackPower);
            }
        }
    }
}