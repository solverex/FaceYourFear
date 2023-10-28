using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LEVELSTARYT : MonoBehaviour
{
    bool fadeoutactive;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        fadeoutactive = true;
    }

    // Update is called once per frame
    void Update()
    {
        Color objectColor = GetComponent<SpriteRenderer>().color;
        float fadeAmount = objectColor.a - (1f * Time.deltaTime);

        objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
        GetComponent<SpriteRenderer>().color = objectColor;

        if (fadeAmount <= 0)
        {
            fadeoutactive = false;
        }

        timer += Time.deltaTime;
        if (timer >= 3f)
        {
            SceneManager.LoadScene(2);
        }
    }
}
