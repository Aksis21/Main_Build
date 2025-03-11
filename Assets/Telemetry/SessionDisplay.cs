using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SessionDisplay : MonoBehaviour
{
    public void OnConnectionSuccess(int SessionID)
    {
        TryGetComponent(out TextMeshProUGUI display);
        if (SessionID < 0)
            display.text = $"Offline session: {SessionID}";
        else
            display.text = $"Connected to server - session ID: {SessionID}";
    }

    public void OnConnectionFail(string error)
    {
        TryGetComponent(out TextMeshProUGUI display);
        display.text = $"Error connecting to server: {error}";
    }
}
