using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class winscreen : MonoBehaviour
{
    [SerializeField] Button btn;
    // Start is called before the first frame update
    void Start()
    {
        btn.onClick.AddListener(back);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void back()
    {
        SceneManager.LoadScene(0);
    }
}
