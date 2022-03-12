using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float score = 0f;
    public float highScore = 0f;

    public float jumpSpeed = 7f;
    public float moveSpeed = 10f;
    float horizontal;
    Vector3 velocity;

    Rigidbody2D rb;
    public Transform cam;
    BoxCollider2D cold;

    public Transform objectPool;

    bool springShoed = false;
    bool powered = false;
    float prePosY = 0f;
    public float powerupDistance = 20f;

    public GameObject mainmenu;
    public GameObject platform;
    public GameObject lma;

    public Text scoretext;
    public Text highscoretext;

    public bool gameStarted;

    public float startposy = 6f;

    public Slider speed;

    //Audio
    public AudioSource source;
    public AudioClip[] clips;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cold = GetComponent<BoxCollider2D>();
        highscoretext.text = "High Score : 0";
        score = 0f;
    }

    void Update()
    {
        moveSpeed = speed.value;
        score = cam.transform.position.y-startposy;


        horizontal = Input.acceleration.x;
        horizontal = Mathf.Clamp(horizontal, -1, 1);

        if(horizontal < -0.1f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if(horizontal > 0.1f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        if(transform.position.y > prePosY + powerupDistance && powered)
        {

            Destroy(transform.GetChild(0).gameObject);
            rb.gravityScale = 1f;
            powered = false;
            springShoed = false;
        }

        if (transform.position.x >3f )
        {
            transform.position = new Vector3(-2.8f,transform.position.y,transform.position.z);
        }
        else if (transform.position.x < -3f)
        {
            transform.position = new Vector3(2.8f, transform.position.y, transform.position.z);
        }

        if (!gameStarted)
        {
            velocity = rb.velocity;
            velocity.x = 0f;
            rb.velocity = velocity;
        }
    }

    void FixedUpdate()
    {
        if (gameStarted)
        {
            velocity = rb.velocity;
            velocity.x = horizontal * moveSpeed;
            rb.velocity = velocity;
        }
        

    }

    void LateUpdate()
    {

        
        if (transform.position.y > cam.position.y)
        {
            cam.position = new Vector3(cam.position.x, transform.position.y, cam.position.z);
            if (gameStarted)
            {
                score = Mathf.Round(score);
                scoretext.text = "Score : " + score.ToString();
            }

        }

        

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.relativeVelocity.y >= 0f)
        {
            if (col.transform.tag == "Platform")
            {
                jumpSpeed = 7f;
                jumpS();
            }
            else if (col.transform.tag == "Spring")
            {
                springS();
                jumpSpeed = 10;
            }
            else if (col.transform.tag == "SpringShoe" && !powered)
            {
                
                jumpSpeed = 7f;
                prePosY = transform.position.y;
                springShoed = true;
                powered = true;
                col.transform.SetParent(this.transform);
                col.transform.localPosition = new Vector3(0.01f, -0.77f, 0f);
                col.transform.GetComponent<EdgeCollider2D>().enabled = false;
                col.transform.GetComponent<BoxCollider2D>().enabled = true;
            }
            else if(col.transform.tag == "Jetpack" && !powered)
            {
                jetpackS();
                jumpSpeed = 4f;
                prePosY = transform.position.y;
                powered = true;
                col.transform.SetParent(this.transform);
                col.transform.localPosition = new Vector3(-0.25f * this.transform.localScale.x, -0.15f, 0f);
                rb.gravityScale = 0f;
            }
            else if (col.transform.tag == "bamboohopter" && !powered)
            {
                propelS();
                jumpSpeed = 4f;
                prePosY = transform.position.y;
                powered = true;
                col.transform.SetParent(this.transform);
                col.transform.localPosition = new Vector3(0f, 0.22f, 0f);
                rb.gravityScale = 0f;
            }
            else if(col.transform.tag == "Break")
            {
                breakS();
                jumpSpeed = rb.velocity.y;
                col.transform.GetComponent<EdgeCollider2D>().enabled = false;
                Destroy(col.gameObject);
            }
            else if(col.transform.tag == "Weak")
            {
                jumpS();
                jumpSpeed = 7f;
                col.transform.GetChild(0).gameObject.SetActive(false);
                col.transform.GetChild(1).gameObject.SetActive(true);
                col.transform.tag = "Weak2";
            }
            else if (col.transform.tag == "Weak2")
            {
                jumpS();
                jumpSpeed = 7f;
                col.transform.GetChild(1).gameObject.SetActive(false);
                col.transform.GetChild(2).gameObject.SetActive(true);
                col.transform.tag = "JumpBreak";
            }
            else if (col.transform.tag == "JumpBreak")
            {
                breakS();
                jumpSpeed = 7f;
                Destroy(col.gameObject,0.1f);
            }


            if (springShoed)
            {
                springS();
                jumpSpeed += 3f;

            }

            velocity = rb.velocity;
            velocity.y = jumpSpeed;
            rb.velocity = velocity;
        }

        if(col.transform.tag == "Enemy")
        {
            cold.enabled = false;
            GameOver();
        }

    }

    public void GameOver()
    {
        gameoverS();
        gameStarted = false;
        if(score > highScore)
        {
            highScore = score;
        }
        
        //rb.gravityScale = 0f;
        Vector3 newpos = new Vector3(0f, transform.position.y + 80f, 0f);
        lma.GetComponent<LevelManager>().recreate = newpos.y-5f;
        lma.GetComponent<LevelManager>().startposford = newpos.y;
        lma.SetActive(false);
        transform.position = newpos;
        Instantiate(platform, new Vector3(0f,newpos.y-4f,0f), Quaternion.identity);
        mainmenu.SetActive(true);       
        cold.enabled = true;
        startposy = newpos.y;
        highScore = Mathf.Round(highScore);
        highscoretext.text = "High Score : " + highScore.ToString();
        score = 0;
        foreach(Transform child in objectPool)
        {
            Destroy(child.gameObject);
        }
    } 


    //Audio
    void jumpS()
    {
        if (!springShoed)
        {
            source.clip = clips[0];
            source.Play();
        }
    }
    void breakS()
    {
        source.clip = clips[1];
        source.Play();
    }
    void propelS()
    {
        source.clip = clips[2];
        source.Play();
    }
    void jetpackS()
    {
        source.clip = clips[3];
        source.Play();
    }
    void springS()
    {
        source.clip = clips[4];
        source.Play();
    }
    void gameoverS()
    {
        source.clip = clips[5];
        source.Play();
    }








}
