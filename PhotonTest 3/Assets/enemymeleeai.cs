using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemymeleeai : MonoBehaviour
{
    public string gameobjectname;
    public Rigidbody2D rb;
    private GameObject player;
    public Vector2 playerpos;
    public float rotation;
    public float moveSpeed;
    private Vector2 lookDir;
    private float dis2player;
    public float aggrodis;

    void ProcessInputs()
    {
        
        player = GameObject.Find(gameobjectname);
        playerpos = player.transform.position;
    }

    private void FixedUpdate()
    {
        ProcessInputs();
        lookDir = playerpos - rb.position;
        lookDir = lookDir.normalized;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - (rotation * 90);
        
        dis2player = Vector3.Distance(rb.position, playerpos);
        if (dis2player < aggrodis)
        {
            rb.rotation = angle;
            Move();
        }
        
    }

    void Move()
    {
        rb.velocity = new Vector2(lookDir.x * moveSpeed, lookDir.y * moveSpeed);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
