using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class howtoplay : MonoBehaviour
{
    [SerializeField] Button btn;
    // Start is called before the first frame update
    void Start()
    {
        btn.onClick.AddListener(Startgame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Startgame()
    {
        SceneManager.LoadScene(4);
    }
}
