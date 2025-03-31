using UnityEngine;

public class LilGuy : MonoBehaviour
{
    public Rigidbody2D lilGuyRB;
    public Animator Anim;
    public SpriteRenderer spriteRenderer;

    public float speed;
    public float jumpForce;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius;
    public float jumpTime;

    private float jumpTimeCounter;
    private float input;
    private bool onGround;
    private bool isJumping;

    private bool isStunned = false;
    private float stunTimer = 0f;
    public float stunDuration;

    public Transform throwPoint;
    public GameObject snailPrefab;
    private bool hasSnail = false;
    public float throwForce;

    public PlayerState State = PlayerState.idle;

    public enum PlayerState
    {
        None = 0,
        idle = 1,
        walking = 2,
        jumping = 3,
        stunned = 4,
    }

    void Start()
    {
        if (Anim == null)
            Anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
            {
                isStunned = false;
                lilGuyRB.linearDamping = 0.5f;
                lilGuyRB.gravityScale = 1f;
            }
            else
            {
                SetState(PlayerState.stunned);
                Debug.Log("LilGuy stunned");
                return;
            }
        }
        
        PlayerState s = PlayerState.idle;

        input = Input.GetAxisRaw("Horizontal");

        if (input < 0)
        {
            spriteRenderer.flipX = true;
            s = PlayerState.walking;
        }
        else if (input > 0)
        {
            spriteRenderer.flipX = false;
            s = PlayerState.walking;
        }

        onGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (onGround && Input.GetButtonDown("Jump"))
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            lilGuyRB.linearVelocity = Vector2.up * jumpForce;
            s = PlayerState.jumping;
        }

        if (Input.GetButton("Jump") && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                lilGuyRB.linearVelocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
                s = PlayerState.jumping;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }

        HandleSnail();

        SetState(s);
    }

    void FixedUpdate()
    {
        if (!isStunned)
        {
            lilGuyRB.linearVelocity = new Vector2(input * speed, lilGuyRB.linearVelocity.y);
        }
        else
        {
            lilGuyRB.linearVelocity = new Vector2(0, lilGuyRB.linearVelocity.y);
        }
    }

    public void Stun(Vector2 attackerPosition, float knockbackForce = 800f)
    {
        lilGuyRB.linearDamping = 0f;
        lilGuyRB.gravityScale = 0.5f;
        isStunned = true;
        stunTimer = stunDuration;
        SetState(PlayerState.stunned);
        
        Vector2 direction = (transform.position - new Vector3(attackerPosition.x, attackerPosition.y)).normalized;
        lilGuyRB.linearVelocity = Vector2.zero;
        lilGuyRB.AddForce(direction * knockbackForce);
        Debug.Log("LilGuy knocked back");
    }

    void HandleSnail()
    {
        if (hasSnail && Input.GetKeyDown(KeyCode.F))
        {
            Vector2 spawnPos = throwPoint.position + (spriteRenderer.flipX ? Vector3.left : Vector3.right) * 0.5f;

            GameObject thrownSnail = Instantiate(snailPrefab, spawnPos, Quaternion.identity);
            Rigidbody2D snailRB = thrownSnail.GetComponent<Rigidbody2D>();

            Vector2 throwDirection = spriteRenderer.flipX ? Vector2.left : Vector2.right;
            snailRB.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);

            hasSnail = false;
            Debug.Log("LilGuy threw snail");
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Snail") && !hasSnail)
        {
            hasSnail = true;
            Destroy(collision.gameObject);
            Debug.Log("LilGuy held snail");
        }
    }

    void SetState(PlayerState s)
    {
        if (State == s) return;
        State = s;

        switch (State)
        {
            case PlayerState.idle:
                Anim.Play("idle");
                break;
            case PlayerState.walking:
                Anim.Play("walking");
                break;
            case PlayerState.jumping:
                Anim.Play("jumping");
                break;
            case PlayerState.stunned:
                Anim.Play("stunned");
                break;
        }
    }
}
