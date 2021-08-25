using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    private GameObject player;
    public GameObject pfbean;
    public GameObject pfbullet;
    public float moveSpeed;
    public Rigidbody2D rb;
    public Camera cam;
    public float rotation;
    public float bulletForce = 20f;
    public Transform firepoint;
    public float firedelay;
    public float bulletspread;


    public Vector3 plantP;
    private Vector3 plantPp;
    private Vector3 playerP;
    private Vector2 mouseP;
    private float playerX;
    private float playerY;
    private float cx;
    private float cy;
    private float wait;
    private Vector3 spread;

  
    public bool fireReady;
    public bool fireReady2;
    private Vector2 moveDirection;


    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
       
    }
    void FixedUpdate()
    {
        //Physics
        Move();

        Vector2 lookDir = mouseP - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - (rotation * 90);
        rb.rotation = angle;
        AutoFire();
        //Debug.Log(wait);
    }
    
    void ProcessInputs()
    {
        mouseP = cam.ScreenToWorldPoint(Input.mousePosition);
        playerP = GameObject.Find("playerbox").transform.position;
        Fire1();
        Fire2();

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX, moveY).normalized;
        
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }
    void plantBean()
        
    {
        Instantiate(pfbean, playerP, Quaternion.identity);
        //Debug.Log("planted");
    }
    void plantBean2()
    {
        Instantiate(pfbean, new Vector3(mouseP.x, mouseP.y, 0), Quaternion.identity);
       // Debug.Log(mouseP.x);
        //Debug.Log(mouseP.y);
    }
    void firebullet()
    {
        
       
        GameObject bullet = Instantiate(pfbullet, firepoint.position, firepoint.rotation);
        Rigidbody2D rbullet = bullet.GetComponent<Rigidbody2D>();
        
        spread = firepoint.up;
        spread.x = spread.x + (Random.Range(-1, 1) * bulletspread);
        spread.y = spread.y + (Random.Range(-1, 1) * bulletspread);
        Debug.Log(spread);
        rbullet.AddForce(spread * bulletForce, ForceMode2D.Impulse);
    }
    void Fire1()
    {
        bool fire = Input.GetButton("Fire1");
        if (fire == true)
        {
            if (fireReady == true)
            {
               
                firebullet();
                wait = firedelay;
                fireReady = false;
            }


        }
        else
        {
            fireReady = true;
        }
      
        
    }
    void Fire2()
    {
        bool fire = Input.GetButton("Fire2");
        if (fire == true)
        {
            if (fireReady2 == true)
            {
                plantBean2();
                fireReady2 = false;
            }


        }
        else
        {
            fireReady2 = true;
        }
        

        
    }
    void AutoFire()
    {
        if (fireReady == false)
        {
            wait = wait - 1;
            if (wait < 0)
            {
                fireReady = true;
            }
        }
    }
}
