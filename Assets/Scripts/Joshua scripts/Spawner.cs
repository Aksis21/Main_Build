using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject projectile;
    public float projectileSpeed;
    public Vector3 direction;

    float timer = float.PositiveInfinity;
    public float spawnTime;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnTime)
        {
            GameObject newProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
            Projectile setProjectile = newProjectile.GetComponent<Projectile>();
            setProjectile.moveSpeed = projectileSpeed;

            if (direction != Vector3.zero)
            {
                setProjectile.targetPlayer = false;
                setProjectile.direction = direction;
            }

            timer = 0f;
        }
    }
}
