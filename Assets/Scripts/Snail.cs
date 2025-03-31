using UnityEngine;

public class SnailProjectile : MonoBehaviour
{
    public int damage = 1;

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Snail hit: " + collision.gameObject.name);

        if (collision.collider.CompareTag("Enemy"))
        {
            BadGuyScript badGuy = collision.collider.GetComponent<BadGuyScript>();
            if (badGuy != null)
            {
                badGuy.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
        else
        {
            // Optional: destroy on wall, floor, etc.
            Destroy(gameObject);
        }
    }
}


