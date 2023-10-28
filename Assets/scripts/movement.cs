using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class movement : MonoBehaviour
{
    // input controller for movementl
    [SerializeField] InputAction Movement;
    // input controller for jump
    [SerializeField] InputAction Jump;
    [SerializeField] InputAction Pause;

    [Header("Variables")]
    // base movement speed
    public int movementspeed;
    // force of jump
    public float jumpforce;
    // max jump time
    public float lowjumpmultiplier;
    // maximum time to jump after falling off platform
    public float maxcoyotetime;
    // fall speed rate of increase
    public int deltafallspeed;
    // max fall speed
    public int maxfallspeed;

    [Header("Components")]
    // calls rigidbody
    [SerializeField] Rigidbody2D rb;
    // calls layer for boxcast
    [SerializeField] LayerMask GroundLayer;
    [SerializeField] Camera camera;
    [SerializeField] AudioSource aS;
    [SerializeField] AudioClip tappingsound;
    [SerializeField] AudioClip landingsound;
    [SerializeField] AudioClip wind;
    [SerializeField] AudioClip pausesound;
    [SerializeField] GameObject pausescreen;
    [SerializeField] Button continuebutton;
    [SerializeField] Button quitbutton;


    // checks if player is jumping
    public bool canJump;
    // timer that decreases when player leaves platform
    public float coyotetimetimer;

    GameObject fadeout;
    GameObject fadein;
    bool fadeoutactive;
    bool fadeinactive;
    float timebttaps;
    bool landingplay;
    bool isPaused;

    // Enables input
    void OnEnable()
    {
        Movement.Enable();
        Jump.Enable();
        Pause.Enable();
    }

    // Disables input
    void OnDisable()
    {
        Movement.Disable();
        Jump.Disable();
        Pause.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        // calls rigidbody
        rb = GetComponent<Rigidbody2D>();
        canJump = true;
        continuebutton.onClick.AddListener(continuestage);
        quitbutton.onClick.AddListener(quitstage);
    }

    // Update is called once per frame
    void Update()
    {
        float recpause = Pause.ReadValue<float>();
        if (recpause > 0 && !isPaused)
        {
            aS.PlayOneShot(pausesound);
            isPaused = true;
        }

        if (isPaused)
        {
            Time.timeScale = 0f;
            pausescreen.SetActive(true);
        }
        if (!isPaused)
        {
            Time.timeScale = 1f;
            pausescreen.SetActive(false);
            horizontalmovement();
            jumping();
            fastfall();
            coyotetimecheck();

            // checks if grounded (bug fix)
            if (isGrounded())
            {
                canJump = true;
            }

            if (fadeoutactive)
            {
                Color objectColor = fadeout.GetComponent<SpriteRenderer>().color;
                float fadeAmount = objectColor.a - (1f * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                fadeout.GetComponent<SpriteRenderer>().color = objectColor;

                if (fadeAmount <= 0)
                {
                    fadeoutactive = false;
                }
            }

            if (fadeinactive)
            {
                Color objectColor = fadein.GetComponent<SpriteRenderer>().color;
                float fadeAmount = objectColor.a + (1f * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                fadein.GetComponent<SpriteRenderer>().color = objectColor;

                if (fadeAmount >= 1)
                {
                    fadeinactive = false;
                }
            }

            if (isGrounded() && landingplay)
            {
                aS.PlayOneShot(landingsound);
                landingplay = false;
            }

            if (!aS.isPlaying)
            {
                aS.PlayOneShot(wind);
            }

        }

        camera.transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
    }

    void horizontalmovement()
    {
        timebttaps -= Time.deltaTime;
        // recieves input and calculates movement
        float recmove = Movement.ReadValue<float>();
        float calcmove = recmove * movementspeed;

        // moving along
        if (recmove != 0)
        {
            rb.velocity = new Vector2(calcmove, rb.velocity.y);
            if (timebttaps <= 0 && isGrounded())
            {
                aS.PlayOneShot(tappingsound);
                timebttaps = 0.5f;
            }
        }
        else
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
    }

    void jumping()
    {
        // recieves input of jump
        float recjump = Jump.ReadValue<float>();
        // ALL OF THESE CHECKS ARE REQUIRED
        if (recjump > 0 && (isGrounded() || coyotetimetimer >= 0) && canJump)
        {
            // resets velocity
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            canJump = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
        }
        else if (recjump <= 0 && !isGrounded() && !canJump)
        {
            if (rb.velocity.y > (jumpforce * lowjumpmultiplier))
            {
                rb.velocity = new Vector2(rb.velocity.x, (jumpforce * lowjumpmultiplier));
            }
        }
    }

    private bool isGrounded()
    {
        // boxcast to check grounded state
        if (Physics2D.BoxCast(transform.position, transform.localScale / 2, 0, Vector3.down, 1.5f, GroundLayer))
        {
            coyotetimetimer = maxcoyotetime;
            return true;
        }
        else
        {
            landingplay = true;
            return false;
        }
    }

    void fastfall()
    {
        // checks if player is falling so that they fast fall
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = Mathf.Clamp(1 * (deltafallspeed / Time.deltaTime), 1, maxfallspeed);
        }
        else
        {
            rb.gravityScale = 1f;
        }
    }

    void coyotetimecheck()
    {
        // coyote timer after leaving platform
        if (!isGrounded())
        {
            coyotetimetimer -= Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D hitinfo)
    {
        if (hitinfo.gameObject.tag == "levelend")
        {
            SceneManager.LoadScene(5);
        }
    }

    void OnTriggerStay2D(Collider2D hitinfo)
    {
        if (hitinfo.gameObject.tag == "darkness")
        {
            fadeout = hitinfo.gameObject;
            fadeoutactive = true;
        }
    }

   void OnTriggerExit2D(Collider2D hitinfo)
    {
        if (hitinfo.gameObject.tag == "darkness")
        {
            fadein = hitinfo.gameObject;
            fadeinactive = true;
        }
    }

    public void changetohell()
    {
        aS.pitch = 0.2f;
    }

    public void changenorm()
    {
        aS.pitch = 1f;
    }

    public void changegrey()
    {
        aS.pitch = 0.6f;
    }

    public void continuestage()
    {
        isPaused = false;
        aS.PlayOneShot(pausesound);
    }

    public void quitstage()
    {
        SceneManager.LoadScene(0);
    }
}
