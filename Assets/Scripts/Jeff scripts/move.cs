using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class move : MonoBehaviour
{
    [SerializeField] float speed;
    float gowherex;
    float gowherey;
    [SerializeField] Rigidbody2D rb2d;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        gowherex = Input.GetAxis("Horizontal");
        gowherey = Input.GetAxis("Vertical");

        transform.Translate(gowherex * speed * Time.deltaTime, 0, 0);
        transform.Translate(0, gowherey * speed * Time.deltaTime, 0);

        
        
    }

}
