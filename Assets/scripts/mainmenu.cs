using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class mainmenu : MonoBehaviour
{
    [SerializeField] Button btns;
    [SerializeField] Button btnq;
    // Start is called before the first frame update
    void Start()
    {
        btns.onClick.AddListener(playegame);
        btnq.onClick.AddListener(leave);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playegame()
    {
        SceneManager.LoadScene(1);
    }

    public void leave()
    {
        Application.Quit();
    }
}
