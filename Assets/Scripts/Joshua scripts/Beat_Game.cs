using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Beat_Game : MonoBehaviour
{
    public string scenesName;
    [SerializeField] float time;

    GameObject tracker;
    Collectible_Tracker trackerScript;

    private void Start()
    {
        tracker = GameObject.Find("Collectible Tracker");
        trackerScript = tracker.GetComponent<Collectible_Tracker>();
    }

    IEnumerator changeScene()
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(scenesName);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (trackerScript.collectibleCount < trackerScript.collectibleTotal) return;
        if (collision.gameObject.tag == "Player")
            StartCoroutine(changeScene());
    }
}
