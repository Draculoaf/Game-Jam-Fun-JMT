using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCont : MonoBehaviour
{
    //Player must have RB, camera must be tagged main camera, amd must make a fire point in front of the player)

    //player movement
    public Rigidbody2D rb;
    public float player_moveSpeed = 3f;

    //Camera must have the main camera tag
    private Camera cam;

    public Transform firePoint;
    public GameObject bullet;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //Player movement
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * player_moveSpeed;

        //https://www.youtube.com/watch?v=xLtLwSgzOEo
        //Mouse tracking
        Vector3 mouse = Input.mousePosition;

        //Player tracking
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);

        //Rotating the character to the mouse position
        Vector2 offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        //Shooting
        if (Input.GetMouseButtonDown(0))
            Instantiate(bullet, firePoint.position, transform.rotation);
    }
}
