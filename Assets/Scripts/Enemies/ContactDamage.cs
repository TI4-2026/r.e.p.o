using UnityEngine;

public class ContactDamage : PlayerCollisionObject
{
    public float damage = 10f;
    public float knockbackPower = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent(out PlayerLife playerLife)) 
            {
                playerLife.TakeDamage(damage);
                playerLife.ApplyKnockback(transform.position, knockbackPower);
            }
        }
    }
}