using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShowUITab : MonoBehaviour
{

    public GameObject setting;

    // Start is called before the first frame update
    void Start()
    {
        setting.SetActive(false);

    }
    // Update is called once per frame
    public void SettingToPlay()
    {
        setting.SetActive(!setting.activeSelf);
    }
}
