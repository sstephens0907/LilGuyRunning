using UnityEngine;

public class StunParticleCollision : MonoBehaviour
{
	public Transform attacker;
	void OnParticleCollision(GameObject other)
	{
		Debug.Log("Particle hit" + other.name);
		if (other.CompareTag("Player"))
		{
			LilGuy lilGuy = other.GetComponent<LilGuy>();

			if (lilGuy != null)
			{
				Debug.Log("Calling LilGuy.Stun()");
				Vector2 attackerPosition = attacker != null ? attacker.position : transform.position;
				lilGuy.Stun(attackerPosition);
			}
		}
	}
}
