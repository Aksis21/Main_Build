using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
    public string scenesName;

    private void Start()
    {
        StartCoroutine(changeScene());
    }

    
    
    IEnumerator changeScene()
    {
        yield return new WaitForSeconds(18f);
        SceneManager.LoadScene(scenesName);
    }
   
}
