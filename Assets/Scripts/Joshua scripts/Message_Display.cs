using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Message_Display : MonoBehaviour
{
    public GameObject messageBG;
    public GameObject messageBorder;

    TextMeshProUGUI textDisplay;
    GameObject fade;
    GameObject button;
    Image fadeImage;
    Image panelImage;

    void Start()
    {
        textDisplay = GetComponentInChildren<TextMeshProUGUI>();
        fade = GameObject.Find("Fade");
        button = GameObject.Find("Hide Message");
        fadeImage = fade.GetComponent<Image>();
        panelImage = GetComponent<Image>();

        textDisplay.enabled = false;
        panelImage.enabled = false;
        messageBG.SetActive(false);
        messageBorder.SetActive(false);
        button.SetActive(false);
    }

    public void showMessage()
    {
        panelImage.enabled = true;
        button.SetActive(true);
        textDisplay.enabled = true;
        messageBG.SetActive(true);
        messageBorder.SetActive(true);
        fadeImage.color = new Color(0f, 0f, 0f, 0.8f);
        Time.timeScale = 0f;
    }

    public void hideMessage()
    {
        panelImage.enabled = false;
        button.SetActive(false);
        textDisplay.enabled = false;
        messageBG.SetActive(false);
        messageBorder.SetActive(false);
        fadeImage.color = new Color(0f, 0f, 0f, 0f);
        Time.timeScale = 1f;
    }

    public void parryAcquired()
    {
        textDisplay.text = ("Parry upgrade acquired. Press SPACE to deploy a protective shield.");
        showMessage();
    }

    public void dashAcquired()
    {
        textDisplay.text = ("Dash upgrade acquired. Press SHIFT to quickly dash in your current direction.");
        showMessage();
    }

    public void allDone()
    {
        textDisplay.text = ("All ship components have been recovered! Return to your ship to leave the planet!");
        showMessage();
    }
}
