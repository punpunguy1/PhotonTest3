using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class MovementScript : MonoBehaviour
{
    
    //respawn
    public GameObject respawnmenu;
    public bool alive;


    //movement
    public float moveSpeed;
    public Vector2 moveDirection;
    public Rigidbody2D rb;

    //fire
    public float bulletForce;
    private Vector3 spread;
    public float bulletspread;
    public Transform firepoint;
    public GameObject pfbullet;
    private bool fireReady;
    private float wait;
    public float firedelay;
    

    //aim calc
    public Rigidbody2D gun;
    private Vector2 mouseP;
    private Vector2 playerP;
    public float rotation;
    private Vector2 lookDir;
    private Vector3 gunpos;
    public float gundis;

    //camera
    public Rigidbody2D playersprite;
    public GameObject playercam;
    public Camera cam;
    public float zshift;
    private Vector3 camshift;

    //photon
    public PhotonView view;


    private void Start()
    {
        view = GetComponent<PhotonView>();
        if (view.IsMine)
        {
            playercam.SetActive(true);
        }
        alive = true;
    }
    public void die()
    {
        alive = false;
    }
    public void respawn()
    {
        alive = true;
    }
    private void FixedUpdate()
    {
        
        if (view.IsMine)
        {
            if (alive)
            {
                Look();
                GunMechanics();
                ProcessInputs();
                Move();
            }
            
            MoveCam();
            
        } 
    }
  
    void Look()
    {
        lookDir = mouseP - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - (rotation * 90);
        playersprite.rotation = angle;
        gun.rotation = angle;
    }
    public void Respawn()
    {
        //respawnmenu.SetActive(false);
    }
    void GunMechanics()
    {
        gunpos = lookDir.normalized * gundis;
        gunpos[0] = gunpos[0] + view.transform.position.x;
        gunpos[1] = gunpos[1] + view.transform.position.y;
        gun.position = gunpos;
    }
    void firebullet()
    {
        GameObject bullet = PhotonNetwork.Instantiate(pfbullet.name, firepoint.position, firepoint.rotation);
        Rigidbody2D rbullet = bullet.GetComponent<Rigidbody2D>();

        spread = firepoint.up;
        spread.x = spread.x + (Random.Range(-1, 1) * bulletspread);
        spread.y = spread.y + (Random.Range(-1, 1) * bulletspread);
        Debug.Log(spread);
        rbullet.AddForce(firepoint.up.normalized * bulletForce, ForceMode2D.Force);
        Debug.Log("pew");
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
    void ProcessInputs()
    {
        Fire1();
        //mouse
        mouseP = cam.ScreenToWorldPoint(Input.mousePosition);
        playerP = rb.transform.position;

        //move
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX, moveY).normalized;

    }
    void Move()
    {
        Vector2 move;
        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");
        rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);
        //rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }
    void MoveCam()
    {
        if (view.IsMine)
        {
            camshift = view.transform.position;
            playersprite.position = view.transform.position;
            camshift[2] = zshift;
            playercam.transform.position = camshift;
            playercam.transform.rotation = Quaternion.identity;
        }
        
    }
}