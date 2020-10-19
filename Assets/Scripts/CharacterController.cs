using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterController : MonoBehaviour
{
    //character movement
    public int speed = 3;
    public Rigidbody2D rb;
    public int jumpForce = 7; 
    private bool facingRight = true;

    //check if player is on ground
    [SerializeField]
    public bool isGroundedRoad;
    public bool isGroundedBlock;

    //ground layers/checks
    public Transform groundCheckOrigin;
    public float checkRadius = .2f;
    public LayerMask groundLayer;
    public LayerMask platformLayer;

    public Animator animator;

    public GameObject gameOverText, restartButton;

    public float numMasks = 0;
    public TextMeshProUGUI maskCount;

    // Start is called before the first frame update
    void Start()
    {
        gameOverText.SetActive(false);
        restartButton.SetActive(false);

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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Virus"))
        {
            if(numMasks == 0)
            {
                gameOverText.SetActive(true);
                restartButton.SetActive(true);
                gameObject.SetActive(false);
            } 
            else
            {
                numMasks--;
                maskCount.text = numMasks.ToString();
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Mask"))
        {
            print("collected mask");
            numMasks++;
            maskCount.text = numMasks.ToString();

            Destroy(collision.gameObject);
        }
    }

}
