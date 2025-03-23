using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //[Header("palyer maxSpeed")]
    float maxSpeed = 5.0f;
    float jumpForce = 11f;

    new Rigidbody2D rigidbody2D;
    SpriteRenderer spriteRenderer;
    Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        // 이동
        float horizontal = Input.GetAxisRaw("Horizontal");
        rigidbody2D.AddForce(Vector2.right * horizontal, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        // 점프
        if (Input.GetKeyDown(KeyCode.UpArrow) == true || Input.GetKeyDown(KeyCode.Space) == true)
        {
            //Debug.Log("arrow up");
            rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // 정지
        if (Input.GetButtonUp("Horizontal") == true)
        {
            //Debug.Log("button up");
            rigidbody2D.linearVelocity = new Vector2(0, rigidbody2D.linearVelocityY);
        }

        // 이동방향
        if (rigidbody2D.linearVelocityX == 0)
        {
            animator.SetBool("ANI_RUN", false);
        }
        else
        {
            animator.SetBool("ANI_RUN", true);

            // 속도제한
            if (rigidbody2D.linearVelocityX >= maxSpeed || rigidbody2D.linearVelocityX <= -maxSpeed)
            {
                if (rigidbody2D.linearVelocityX > 0)
                {
                    // 오른쪽
                    rigidbody2D.linearVelocity = new Vector2(maxSpeed, rigidbody2D.linearVelocityY);
                    spriteRenderer.flipX = false;
                }
                else if (rigidbody2D.linearVelocityX < 0)
                {
                    // 왼쪽
                    rigidbody2D.linearVelocity = new Vector2(-maxSpeed, rigidbody2D.linearVelocityY);
                    spriteRenderer.flipX = true;
                }
            }
        }
    }




}
