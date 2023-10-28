using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyshooter : MonoBehaviour
{
    [SerializeField] float enemyhealth;
    [SerializeField] float enemysight;
    [SerializeField] float enemystoppoint;
    [SerializeField] float enemymovementoffset;
    [SerializeField] GameObject NME_Bullet;
    [SerializeField] GameObject exploder;
    [SerializeField] GameObject player;
    [SerializeField] float maxbullettime;
    [SerializeField] float minbullettime;
    [SerializeField] float enemyspeed;
    [SerializeField] Sprite inital;
    [SerializeField] Sprite hell;
    [SerializeField] float iframes;

    float bullettime;
    float distancetoplayer;
    public bool isAggro;
    bool whichway;
    float movementrangeplus;
    float movementrangeminus;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, enemysight);
        Gizmos.DrawWireSphere(transform.position, enemystoppoint);
    }
    // Start is called before the first frame update
    void Start()
    {
        bullettime = Random.Range(minbullettime, maxbullettime);
    }

    // Update is called once per frame
    void Update()
    {
        iframes -= Time.deltaTime;
        distancetoplayer = Vector2.Distance(player.transform.position, transform.position);
        Debug.Log(distancetoplayer);
        if (distancetoplayer <= enemysight && !isAggro)
        {
            movementrangeplus = transform.position.x + enemymovementoffset;
            movementrangeminus = transform.position.x - enemymovementoffset;
            isAggro = true;
        }
        else
        {
            isAggro = false;
        }


        if (enemyhealth <= 0)
        {
            playermorale courage = player.GetComponent<playermorale>();
            courage.AddCourage();
            GameObject explod = Instantiate(exploder, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }

        if (isAggro)
        {
            bullettime -= Time.deltaTime;
            Vector2 direction = player.transform.position - transform.position;
            transform.rotation = Quaternion.FromToRotation(Vector3.right, direction);

            if (transform.rotation.z < 0f)
            {
                GetComponent<SpriteRenderer>().flipY = false;
            }
            else
            {
                GetComponent<SpriteRenderer>().flipY = true;
            }

            if (distancetoplayer >= enemystoppoint)
            {
                GetComponent<Rigidbody2D>().velocity = enemyspeed * transform.right;
            }
            else if (distancetoplayer <= enemystoppoint)
            {
                GetComponent<Rigidbody2D>().velocity = enemyspeed * -transform.right;
            }


            if (bullettime <= 0)
            {
                GameObject bullet = Instantiate(NME_Bullet, transform.position, transform.rotation);
                bullettime = Random.Range(minbullettime, maxbullettime);
            }
        }
    }

    public void TakeDamage(float bulletdamage)
    {
        enemyhealth -= bulletdamage * 2;
        StartCoroutine(Death());
    }

    public void Swordhit(float sworddamage)
    {
        if (iframes <= 0)
        {
            enemyhealth -= sworddamage;
            StartCoroutine(Death());
            iframes = 0.5f;
        }
    }

    IEnumerator Death()
    {
        this.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 255f);
        yield return new WaitForSeconds(0.1f);
        this.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 255f);
    }

    public void changehell()
    {
        GetComponent<SpriteRenderer>().sprite = hell;
    }

    public void changenorm()
    {
        GetComponent<SpriteRenderer>().sprite = inital;
    }
}
