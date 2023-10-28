using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playermorale : MonoBehaviour
{
    [SerializeField] float morale;
    [SerializeField] float moraleratedecrease;
    [SerializeField] float moraletogrey;
    [SerializeField] float moraletohell;
    [SerializeField] GameObject moraleui;

    bool greyscale;
    bool hellscape;

    [SerializeField] GameObject gs;
    [SerializeField] GameObject hell;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        morale -= moraleratedecrease * Time.deltaTime;
        float remaininghealth = Mathf.Clamp((morale) / 100f, 0, 1f);
        moraleui.transform.localScale = new Vector3(1, remaininghealth, 1);
        if (morale <= moraletogrey | morale > moraletohell)
        {
            greyscale = true;
        }
        else
        {
            greyscale = false;
        }

        if (morale <= moraletohell)
        {
            hellscape = true;
        }
        else
        {
            hellscape = false;
        }

        if (greyscale)
        {
            gs.SetActive(true);
            hell.SetActive(false);
        }
        if (hellscape)
        {
            hell.SetActive(true);
            gs.SetActive(false);
        }
        else
        {
            gs.SetActive(false);
            hell.SetActive(false);
        }

        if (hellscape)
        {
            GameObject[] shootercheck = GameObject.FindGameObjectsWithTag("enemy");
            GameObject[] snipercheck = GameObject.FindGameObjectsWithTag("sniper");
            GameObject player = GameObject.Find("protoplayer");
            player.GetComponent<movement>().changetohell();
            foreach(GameObject shooter in shootercheck)
            {
                shooter.GetComponent<enemyshooter>().changehell();
            }
            foreach(GameObject sniper in snipercheck)
            {
                sniper.GetComponent<enemysniper>().changehell();
            }
        }
        else
        {
            GameObject [] shootercheck = GameObject.FindGameObjectsWithTag("enemy");
            GameObject player = GameObject.Find("protoplayer");
            player.GetComponent<movement>().changenorm();
            foreach (GameObject shooter in shootercheck)
            {
                shooter.GetComponent<enemyshooter>().changenorm();
            }
        }

        if (morale <= 0)
        {
            SceneManager.LoadScene(3);
        }
    }

    public void TakeDamage(float bulletdamage)
    {
        morale -= bulletdamage;
    }
    
    public void AddCourage()
    {
        morale += 10f;
        if (morale > 100f)
        {
            morale = 100f;
        }
    }
}
