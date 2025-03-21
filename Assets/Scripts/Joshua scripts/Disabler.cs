using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disabler : MonoBehaviour
{
    public bool disable = false;

    private void Start()
    {
        if (disable) Destroy(gameObject);
    }
}
