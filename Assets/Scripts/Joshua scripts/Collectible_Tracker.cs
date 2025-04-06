using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Collectible_Tracker : MonoBehaviour
{
    public float collectibleTotal = 4f;

    [Header("DO NOT CHANGE")]
    public float collectibleCount = 0f;
    TextMeshProUGUI textDisplay;

    private void Start()
    {
        textDisplay = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        textDisplay.text = collectibleCount.ToString() + "/" + collectibleTotal.ToString();
    }
}
