using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bull : MonoBehaviour
{
    [SerializeField] float bulletspeed;
    [SerializeField] float bulletdamage;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] sword sw;
    [SerializeField] bool gothruwalls;
    [SerializeField] float lifetime;

    public bool isHit;
    public bool SsSwinging;
    public float currenttime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        currenttime = lifetime;
    }

    // Update is called once per frame
    void Update()
    {
        currenttime -= Time.deltaTime;
        if (isHit)
        {
            rb.velocity = -transform.right * (bulletspeed * 2);
        }
        else
        {
            rb.velocity = transform.right * bulletspeed;
        }

        SsSwinging = sword.isSwinging;

        if (currenttime <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D hitinfo)
    {
        if(hitinfo.gameObject.tag == "sword" && !isHit && SsSwinging)
        {
            isHit = true;
        }
    }

    void OnTriggerEnter2D(Collider2D hitinfo)
    {
        if (hitinfo.gameObject.tag == "sword" && !isHit && SsSwinging)
        {
            isHit = true;
        }

        if (hitinfo.gameObject.tag == "enemy" && isHit)
        {
            enemyshooter nme_script = hitinfo.gameObject.GetComponent<enemyshooter>();
            nme_script.TakeDamage(bulletdamage);
            Destroy(this.gameObject);
        }
        else if (hitinfo.gameObject.tag == "sniper" && isHit)
        {
            Debug.Log("workpls");
            enemysniper nme_script = hitinfo.gameObject.GetComponent<enemysniper>();
            nme_script.TakeDamage(bulletdamage);
            Destroy(this.gameObject);
        }

        if (hitinfo.gameObject.tag == "player")
        {
            playermorale player = hitinfo.gameObject.GetComponent<playermorale>();
            player.TakeDamage(bulletdamage);
            Destroy(this.gameObject);
        }
        if (hitinfo.gameObject.layer == 6 && !gothruwalls)
        {
            Destroy(this.gameObject);
        }
    }
}
