using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableBlock : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
