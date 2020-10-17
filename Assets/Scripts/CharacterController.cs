using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    //global variables
    public int speed = 3;
    public Rigidbody2D rb;
    public int jumpForce = 7;
    
    private bool facingRight = true;

    [SerializeField]
    public bool isGroundedRoad;
    public bool isGroundedBlock;

    public Transform groundCheckOrigin;
    public float checkRadius = .2f;
    public LayerMask groundLayer;
    public LayerMask platformLayer;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        isGroundedRoad = Physics2D.OverlapCircle(groundCheckOrigin.position, checkRadius, groundLayer);
        isGroundedBlock = Physics2D.OverlapCircle(groundCheckOrigin.position, checkRadius, platformLayer);

        if ((isGroundedRoad || isGroundedBlock) && Input.GetButtonDown("Jump"))
        {
            rb.velocity = Vector2.up * jumpForce;
        }
        
    }

    private void FixedUpdate()
    {
        float x_dir = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(x_dir * speed, rb.velocity.y);

        animator.SetFloat("Speed", Mathf.Abs(x_dir));

        if(facingRight && x_dir < 0)
        {
            Flip();
        } else if(!facingRight && x_dir > 0)
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        var scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }


}
