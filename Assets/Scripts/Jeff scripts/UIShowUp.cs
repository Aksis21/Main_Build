using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIShowUp : MonoBehaviour
{
    public GameObject menu;
    public GameObject setting;
    public UnityEvent PausedAll;
    public UnityEvent ResumAll;
    bool isPaused;
    // Start is called before the first frame update
    void Start()
    {
        menu.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PressedToPlay();
        }
    }
    public void PressedToPlay()
    {
        menu.SetActive(!menu.activeSelf);
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0f;
            PausedAll.Invoke();
        }
        else
        {
            Time.timeScale = 1f;
            ResumAll.Invoke();
        }
    }

    

}
