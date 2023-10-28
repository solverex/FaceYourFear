using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameover : MonoBehaviour
{
    float fadeAmount;
    bool fadeoutactive;
    [SerializeField] Button resetbutton;
    [SerializeField] Button quitbutton;
    // Start is called before the first frame update
    void Update()
    {
        if (fadeoutactive)
        {
            Color objectColor = GetComponent<SpriteRenderer>().color;
            float fadeAmount = objectColor.a - (1f * Time.deltaTime);

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            GetComponent<SpriteRenderer>().color = objectColor;

            if (fadeAmount <= 0)
            {
                fadeoutactive = false;
            }
        }
    }

    // Update is called once per frame
    void Start()
    {
        resetbutton.onClick.AddListener(restart);
        quitbutton.onClick.AddListener(quit);
        fadeoutactive = true;
    }

    public void restart()
    {
        SceneManager.LoadScene(4);
    }

    public void quit()
    {
        SceneManager.LoadScene(0);
    }
}
