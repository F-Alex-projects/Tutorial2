using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public float speed;

    public float jumpForce;

    public Text scoreText;

    private int scoreValue = 0;

    public Text livesText;

    private int livesValue = 3;

    private bool facingRight = true;

    private bool isOnGround;

    public Transform groundcheck;

    public float checkRadius;

    public LayerMask allGround;

    public Text gameOverText;

    public AudioClip musicClipOne;

    public AudioClip musicClipTwo;

    public AudioSource musicSource;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rd2d = GetComponent<Rigidbody2D>();
        setScoreText();
        setLivesText();
        gameOverText.text = "";
        musicSource.clip = musicClipOne;
        musicSource.Play();
        musicSource.loop = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);
        
        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }

        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }

        if (vertMovement > 0 && isOnGround == false)
        {
            anim.SetInteger("State", 2);
        }

        if (vertMovement == 0 && isOnGround == true)
        {
            if (hozMovement > 0 || hozMovement < 0)
            {
                anim.SetInteger("State", 1);
            }
            else
            {
                anim.SetInteger("State", 0);
            }
        }



        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            setScoreText();
            levelTeleportCheck();
            setWinText();
            Destroy(collision.collider.gameObject);
        }
        if (collision.collider.tag == "Enemy")
        {
            livesValue -= 1;
            setLivesText();
            setLoseText();
            Destroy(collision.collider.gameObject);
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
        }
    }
    
    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
    
    void setScoreText()
    {
        scoreText.text = "Score: " + scoreValue.ToString();
    }
    
    void levelTeleportCheck()
    {
        if (scoreValue == 4)
        {
            transform.position = new Vector2(46.0f, 1.0f);
            livesValue = 3;
            setLivesText();
            anim.SetInteger("State", 0);
        }
    }

    void setLoseText()
    {
        if (livesValue <= 0)
        {
            gameOverText.text = "You Lose! Game by Alejandro Franquez.";
            speed = 0;
            jumpForce = 0;
        }
    }

    void setWinText()
    {
            if (scoreValue >= 8)
        {
            musicSource.Stop();
            gameOverText.text = "You Win! Game by Alejandro Franquez.";
            musicSource.clip = musicClipTwo;
            musicSource.Play();
            musicSource.loop = false;
        }

    }

    void setLivesText()
    {
        livesText.text = "Lives: " + livesValue.ToString();
    }
}