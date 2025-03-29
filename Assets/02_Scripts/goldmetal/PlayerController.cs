using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    string sceneName;
    //[Header("palyer maxSpeed")]
    float maxSpeed = 5.0f;
    float jumpForce = 11f;

    new Rigidbody2D rigidbody2D;
    SpriteRenderer spriteRenderer;
    Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        // 이동 //////////////////////////////////////
        // - 좌우
        float horizontal = Input.GetAxisRaw("Horizontal");
        rigidbody2D.AddForce(Vector2.right * horizontal, ForceMode2D.Impulse);
        if (rigidbody2D.linearVelocityX == 0)
        {
            animator.SetBool("ANI_RUN", false);
        }
        else
        {
            animator.SetBool("ANI_RUN", true);
        }
        //Debug.Log("ANI_RUN = " + animator.GetCurrentAnimatorStateInfo(0).IsName("ANI_RUN"));

        // - 정지
        if (Input.GetButtonUp("Horizontal") == true)
        {
            //Debug.Log("button up");
            rigidbody2D.linearVelocity = new Vector2(0, rigidbody2D.linearVelocityY);
        }
        // - 점프
        if ((Input.GetKeyDown(KeyCode.UpArrow) == true || Input.GetKeyDown(KeyCode.Space) == true)
            && animator.GetBool("ANI_JUMP") == false)
        {
            //Debug.Log("arrow up");
            rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetBool("ANI_JUMP", true);
            animator.SetBool("ANI_RUN", false);
        }
        //Debug.Log("ANI_JUMP = " + animator.GetCurrentAnimatorStateInfo(0).IsName("ANI_JUMP"));

        // 착지 체크
        // - raycast가 점프를 시작할때 벌써 floor를 인지하므로 y force 값이 없을때만 체크
        //Debug.Log("rigidbody2D.linearVelocityY = " + rigidbody2D.linearVelocityY);        
        if (rigidbody2D.linearVelocityY <= 0)
        {
            Debug.DrawRay(rigidbody2D.position, Vector3.down, Color.green);
            RaycastHit2D raycastHit2D = Physics2D.Raycast(rigidbody2D.position, Vector2.down, 1, LayerMask.GetMask("Floor"));
            // layer=floor랑 충돌했을때
            if (raycastHit2D.collider != null)
            {
                //Debug.Log("raycastHit2D.distance = " + raycastHit2D.distance);
                //if (raycastHit2D.distance <= 0.5f)
                if (animator.GetBool("ANI_JUMP") == true)
                {
                    //Debug.Log("raycastHit2D = " + raycastHit2D.collider.name);
                    animator.SetBool("ANI_JUMP", false);
                    animator.SetBool("ANI_RUN", false);
                }
            }
        }
        //Debug.Log("ANI_RUN = " + animator.GetCurrentAnimatorStateInfo(0).IsName("ANI_RUN"));
        //Debug.Log("ANI_JUMP = " + animator.GetCurrentAnimatorStateInfo(0).IsName("ANI_JUMP"));

        // 속도제한
        if (rigidbody2D.linearVelocityX >= maxSpeed || rigidbody2D.linearVelocityX <= -maxSpeed)
        {
            if (rigidbody2D.linearVelocityX > 0)
            {
                // 오른쪽
                rigidbody2D.linearVelocity = new Vector2(maxSpeed, rigidbody2D.linearVelocityY);
            }
            else if (rigidbody2D.linearVelocityX < 0)
            {
                // 왼쪽
                rigidbody2D.linearVelocity = new Vector2(-maxSpeed, rigidbody2D.linearVelocityY);
            }
        }

        // 애니메이션 //////////////////////////////////////
        // 이동방향
        //if (rigidbody2D.linearVelocityX == 0)
        //{
        //    animator.SetBool("ANI_RUN", false);
        //}
        //else
        //{
        //    animator.SetBool("ANI_RUN", true);
        //}

        // 방향에 맞는 이미지 적용 //////////////////////////////////////
        if (rigidbody2D.linearVelocityX > 0)
        {
            // 오른쪽
            spriteRenderer.flipX = false;
        }
        else if (rigidbody2D.linearVelocityX < 0)
        {
            // 왼쪽
            spriteRenderer.flipX = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ReStart();
    }

    void ReStart()
    {
        if(transform.position.y <= -10f)
        {
            SceneManager.LoadScene(sceneName);
        }
    }

}
