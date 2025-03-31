using UnityEngine;

public class BadGuyScript : MonoBehaviour
{
    public Transform lilGuy;
    public float chaseDistance = 5f;
    public float stopDistance = 6f;
    public float attackDistance = 1f;
    public float moveSpeed = 2f;

    public Animator Anim;
    public Rigidbody2D badGuyRB;
    public SpriteRenderer spriteRenderer;
    public float attackCooldown;
    private float lastAttackTime;
    public int damageAmount;

    private enum PlayerState { Idle, Running, Attack, CoolDown }
    private PlayerState currentState = PlayerState.Idle;
    private bool hasAttacked = false;
    public ParticleSystem stunParticles;

    void Start()
    {
        if (Anim == null) Anim = GetComponent<Animator>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (lilGuy == null) return;

        float distance = Vector2.Distance(transform.position, lilGuy.position);

        float directionX = lilGuy.position.x - transform.position.x;
        if (directionX != 0)
            spriteRenderer.flipX = directionX < 0;

        switch (currentState)
        {
            case PlayerState.Idle:
            case PlayerState.Running:
                if (distance <= attackDistance)
                {
                    if (!hasAttacked)
                    {
                        SetState(PlayerState.Attack);
                        hasAttacked = true;
                        lastAttackTime = Time.time;
                    }
                    else
                    {
                        SetState(PlayerState.CoolDown);
                    }
                }
                else if (distance <= chaseDistance)
                {
                    SetState(PlayerState.Running);
                    ChasePlayer();
                }
                else
                {
                    SetState(PlayerState.Idle);
                    hasAttacked = false;
                }
                break;

            case PlayerState.Attack:
            
                if (Time.time - lastAttackTime >= 1)
                {
                    SetState(PlayerState.CoolDown);
                }
                break;

            case PlayerState.CoolDown:
                if (Time.time - lastAttackTime >= attackCooldown)
                {
                    hasAttacked = false;
                    if (distance <= chaseDistance)
                    {
                        SetState(PlayerState.Running);
                    }
                    else
                    {
                        SetState(PlayerState.Idle);
                    }
                }
                break;
        }
    }


    void SetState(PlayerState newState)
    {
        if (currentState == newState) return;

        currentState = newState;
        Debug.Log("BadGuy State changed to: " + newState);

        switch (currentState)
        {
            case PlayerState.Idle:
                Anim.Play("Idle");
                break;
            case PlayerState.Running:
                Anim.Play("Running");
                break;
            case PlayerState.Attack:
                Anim.Play("Attack");
                break;
            case PlayerState.CoolDown:
                Anim.Play("CoolDown");
                break;
        }
    }

    void ChasePlayer()
    {
        Vector2 direction = (lilGuy.position - transform.position).normalized;
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
    }
    public void DoDamage()
    {
        if (lilGuy == null) return;

        float dist = Vector2.Distance(transform.position, lilGuy.position);
        if (dist <= attackDistance)
        {
            LilGuyHealth health = lilGuy.GetComponent<LilGuyHealth>();
            if (health != null)
            {
                health.TakeDamage(damageAmount);
                Debug.Log("Damage applied to LilGuy!");
            }
        }
    }

    public void TriggerStunParticles()
    {
        if (stunParticles != null)
        {
            stunParticles.Play();
        }
    }

    public void TakeDamage(int amount)
    {
        Debug.Log(amount);
    }

}
