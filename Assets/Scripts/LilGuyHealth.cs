using UnityEngine;

public class LilGuyHealth : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("LilGuy took damage! Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("LilGuy has been defeated!");
        // Add respawn, game over, or restart logic here
    }
}
