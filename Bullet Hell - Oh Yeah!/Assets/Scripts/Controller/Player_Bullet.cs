using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Bullet : MonoBehaviour
{
    //Attach this script to the player's bullet (must have rb, box collider and enemies must be tagged Enemy)
    public float bulletSpeed = 10f;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.right * bulletSpeed;
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (other.tag == "Enemy")
            Destroy(other.gameObject);

        Destroy(gameObject);
    }*/
}
