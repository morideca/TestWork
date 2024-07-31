
using UnityEngine;

public class Player : BaseUnit
{
    private float jumpForce;
    private float moveSpeed;
    private float speedSlide;
    private float jumpWallForce = 7;

    [SerializeField]
    private Transform pointForCheckGround;
    [SerializeField]
    private Transform pointForCheckWallLeft;
    [SerializeField]
    private Transform pointForCheckWallRight;

    [SerializeField]
    GameObject flipGO;

    [SerializeField]
    private LayerMask layerWall;

    private Rigidbody2D rb;

    private bool isSliding;
    private bool canWallJump = false;
    private bool onCooldown = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(float attackSpeed, float jumpForce, float moveSpeed, float speedSLide)
    {
        this.attackSpeed = attackSpeed;
        this.jumpForce = jumpForce;
        this.moveSpeed = moveSpeed;
        this.speedSlide = speedSLide;
    }


    private void OnEnable()
    {
        HealthCell.Damaged += AllowToAttack;
        HealthCell.StoppedBlinking += DisallowToAttack;
        HealthCell.LostHealthPoint += DisallowToAttack;
    }

    private void OnDisable()
    {
        HealthCell.Damaged -= AllowToAttack;
        HealthCell.StoppedBlinking -= DisallowToAttack;
        HealthCell.LostHealthPoint -= DisallowToAttack;
    }

    private void AllowToAttack()
    {
        canAttack = true;
    }

    private void DisallowToAttack()
    {
        canAttack = false;
    }

    private bool IsGrounded()
    {
        var collider = Physics2D.OverlapCircle(pointForCheckGround.position, 0.01f);
        return (collider != null);
    }

    private bool IsWallLeft()
    {
        var collider = Physics2D.OverlapCircle(pointForCheckWallLeft.position, 0.01f, layerWall);
        return (collider != null);
    }

    private bool IsWallRight()
    {
        var collider = Physics2D.OverlapCircle(pointForCheckWallRight.position, 0.01f, layerWall);
        return (collider != null);
    }

    private void Move()
    {
        float move = Input.GetAxis("Horizontal");

        if (IsWallLeft()) move = Mathf.Clamp(move * moveSpeed, 0, float.MaxValue);
        else if (IsWallRight()) move = Mathf.Clamp(move * 3, -float.MaxValue,  0);

        rb.AddForce(new Vector2(move * 5, 0));
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -moveSpeed, moveSpeed), rb.velocity.y);

        if ((Input.GetKeyDown(KeyCode.UpArrow) && IsGrounded()))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && canWallJump)
        {
            WallJump();
        }

    }

    private void WallJump()
    {
        if (IsWallLeft()) rb.AddForce(new Vector2(1,2).normalized * jumpWallForce, ForceMode2D.Impulse);
        else if(IsWallRight()) rb.AddForce(new Vector2(-1, 2).normalized * jumpWallForce, ForceMode2D.Impulse);
    }

    override public void Flip()
    {
        if (Input.GetKey(KeyCode.RightArrow) && !faceRight && !attacking) 
        {
            faceRight = !faceRight;
            flipGO.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && faceRight && !attacking)
        {
            faceRight = !faceRight;
            flipGO.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    public void Slide()
    {
        if (IsWallRight() && !IsGrounded() && Input.GetKey(KeyCode.RightArrow))
        {
            isSliding = true;
        }
        else if (IsWallLeft() && !IsGrounded() && Input.GetKey(KeyCode.LeftArrow))
        {
            isSliding = true;
        }
        else
        {
            isSliding = false;
        }

        if (isSliding)
        {
            canWallJump = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y,
                -speedSlide, float.MaxValue));
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }
    }

    private void CanAttackAgain()
    {
        onCooldown = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !attacking && canAttack && !onCooldown)
        {
            Attack();
            onCooldown = true;
            Invoke("CanAttackAgain", 1 / attackSpeed);
        }

        Slide();
        Move();
        Flip();
    }
}
