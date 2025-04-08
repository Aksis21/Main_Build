using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
    public string scenesName;
    [SerializeField] float time;
    private void Start()
    {
        StartCoroutine(changeScene());
    }

    
    
    IEnumerator changeScene()
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(scenesName);
    }
   
}
