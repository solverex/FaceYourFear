using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class sword : MonoBehaviour
{
    [SerializeField] InputAction FacingDirection;
    [SerializeField] InputAction Swordinput;
    [Header("Variables")]
    public float positionoffset;
    public float swordtimer;
    public float cooldowntimer;
    public float sworddamage;

    [SerializeField] Animator anim;
    [SerializeField] AudioSource aS;
    [SerializeField] AudioClip swordsound;

    public float timer;
    public static bool isSwinging;
    private float recsword;

    void OnEnable()
    {
        FacingDirection.Enable();
        Swordinput.Enable();
    }

    void OnDisable()
    {
        FacingDirection.Disable();
        Swordinput.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        changedir();
        swordactive();
    }

    void changedir()
    {
        float xdir = FacingDirection.ReadValue<Vector2>().x;
        float ydir = FacingDirection.ReadValue<Vector2>().y;
        if (xdir > 0 && ydir == 0 && recsword == 0 && !isSwinging)
        {
            transform.localPosition = new Vector2(positionoffset, 0);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (xdir < 0 && ydir == 0 && recsword == 0 && !isSwinging)
        {
            transform.localPosition = new Vector2(-positionoffset, 0);
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        if (ydir > 0 && recsword == 0 && !isSwinging)
        {
            transform.localPosition = new Vector2(0, positionoffset);
            transform.rotation = Quaternion.Euler(0, 0, 270);
        }
        if (ydir < 0 && recsword == 0 && !isSwinging)
        {
            transform.localPosition = new Vector2(0, -positionoffset);
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
    }

    void swordactive()
    {
        recsword = Swordinput.ReadValue<float>();
        if (recsword > 0 && !isSwinging && timer <= 0)
        {
            anim.SetBool("swinging", true);
            aS.PlayOneShot(swordsound);
            timer = swordtimer + cooldowntimer;
            isSwinging = true;
        }

        timer -= Time.deltaTime;

        if (timer <= cooldowntimer)
        {
            anim.SetBool("swinging", false);
            isSwinging = false;
        }
    }

    void OnTriggerStay2D(Collider2D hitinfo)
    {
        if (hitinfo.gameObject.tag == "enemy" && isSwinging)
        {
            enemyshooter nme_script = hitinfo.gameObject.GetComponent<enemyshooter>();
            nme_script.Swordhit(sworddamage);
        }
        if (hitinfo.gameObject.tag == "sniper" && isSwinging)
        {
            enemysniper nme_script = hitinfo.gameObject.GetComponent<enemysniper>();
            nme_script.Swordhit(sworddamage);
        }
    }
}
