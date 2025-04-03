using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableBlock : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            Projectile projScript = collision.gameObject.GetComponent<Projectile>();
            if (projScript.invulOnSpawn < 0.06f) return;
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
