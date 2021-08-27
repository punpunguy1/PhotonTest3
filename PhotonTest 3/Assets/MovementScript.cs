using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using Photon.Pun;


public class MovementScript : MonoBehaviour
{
    

    //animation
    public Animator animator;
    public GameObject animsprite;

    //audio
    public AudioSource whyhaveu;

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
    private float waitS;
    public float firedelay;
    private bool fireReady2;
    public float specialdelay;

    public Transform specialpoint;
    public GameObject pfspecial;
    public float specialforce;
    public float spermAmmo;
    private float spermCount;
    public float spermRate;
    public float orgiballValue;
    private int orgiballCount;

    //ui
    public Slider cumbar;
    public Text spermcountnum;
    public Text spermammonum;
    
    public Text orgiballcounter;

   

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

    //skills
    int currentindex;
    public Slider[] abilitysliders;
    private float[] skilltimer;
    public int skillcount;
    public float[] skilltimes;
    public bool[] skillready;
    private bool doingskill;


    private void Start()
    {
        view = GetComponent<PhotonView>();
        if (view.IsMine)
        {
            doingskill = false;
            skilltimer = new float[skillcount];
            skillready = new bool[skillcount];
            

            orgiballCount = 0;
            playercam.SetActive(true);
            spermCount = spermAmmo;
            cumbar.maxValue = spermAmmo;
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
    private void Update()
    {
        UpdateUI();
    }
    private void FixedUpdate()
    {
        
        if (view.IsMine)
        {
            if (alive)
            {
                Skills();
                
                GunMechanics();
                ProcessInputs();
                if (!doingskill)
                {
                    Move();
                    Look();
                }
                
                AutoRecharge();
                
            }
            
            MoveCam();
            
        } 
    }
            
    void Skills()
    {
        for (int i = 0; i < skillcount; i++)
        {
            currentindex = i;
            foreach (float skilltime in skilltimer)
            {

                //Debug.Log("index" + currentindex + "  ready" + skillready[currentindex] + "  toforskill" + (skilltimer[currentindex]) + "  time" + Time.time);
                //start cooldown if skill is ready
                if (Time.time > skilltime && skillready[currentindex])
                {
                    //fire
                    skilltimer[currentindex] = Time.time + skilltimes[currentindex];
                    //Debug.Log("triggerT" + currentindex);
                    
                }
                if (Time.time > skilltimer[currentindex] && !skillready[currentindex])
                {
                    skillready[currentindex] = true;
                   // Debug.Log("triggerT" + currentindex);

                }


            }
            
            UpdateSliders();

        }
        

    }
            void UpdateUI()
    {
        spermcountnum.text = Mathf.RoundToInt(spermCount).ToString();
        spermammonum.text = Mathf.RoundToInt(spermAmmo).ToString();
        cumbar.value = spermCount;
        orgiballcounter.text = orgiballCount.ToString();


    }
            void UpdateSliders()
    {
        foreach (Slider slider in abilitysliders)
        {

            abilitysliders[currentindex].maxValue = skilltimes[currentindex];
            abilitysliders[currentindex].value = (skilltimes[currentindex] - (skilltimer[currentindex] - Time.time));

        }
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


    void ProcessInputs()
    {
        Fire1();
        SpecialAttack();
        //mouse
        mouseP = cam.ScreenToWorldPoint(Input.mousePosition);
        playerP = rb.transform.position;

        //move
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX, moveY).normalized;

    }
        void Look()
    {
        lookDir = mouseP - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - (rotation * 90);
        playersprite.rotation = angle;
        gun.rotation = angle;
    }
            void GunMechanics()
    {
        gunpos = lookDir.normalized * gundis;
        gunpos[0] = gunpos[0] + view.transform.position.x;
        gunpos[1] = gunpos[1] + view.transform.position.y;
        gun.position = gunpos;
    }
            
        void Fire1()
    {
        
        bool fire = Input.GetButton("Fire1");
        if (fire == true)
        {
            if (fireReady == true)
            {
                if (skillready[0])
                {
                    firebullet();
                    wait = firedelay;
                    fireReady = false;
                }
                
            }


        }
        else
        {
            fireReady = true;
        }


    }
            void firebullet()
    {
        GameObject bullet = PhotonNetwork.Instantiate(pfbullet.name, firepoint.position, firepoint.rotation);
        Rigidbody2D rbullet = bullet.GetComponent<Rigidbody2D>();


        spread = firepoint.up;
        spread.x = spread.x + (Random.Range(-1, 1) * bulletspread);
        spread.y = spread.y + (Random.Range(-1, 1) * bulletspread);
        //Debug.Log(spread);
        rbullet.AddForce(firepoint.up.normalized * bulletForce, ForceMode2D.Force);
        skillready[0] = false;
        
        animator.SetTrigger("Attack");
        //Debug.Log("pew");
    }


        void SpecialAttack()
    {

        bool fire = Input.GetButton("Fire2");
        if (fire == true)
        {
            if (fireReady2)
            {
                if (skillready[1])
                {
                    animator.SetBool("EnteringSpecial", true);
                    doingskill = true;
                }
                //Debug.Log("specialattack");

                FireSpecial();
                 
            }


        }
        else
        {
            if (!skillready[1])
            {
                animator.SetBool("EnteringSpecial", false);
            }
            doingskill = false;
            fireReady2 = true;
            animator.SetBool("SpecialAttacking", false);
        }


    }
            void FireSpecial()
    {
        if (spermCount > 0)
        {

            animator.SetBool("SpecialAttacking", true);
            if (animsprite.GetComponent<AnimationEventTrigger>().specialattacking)
            {
                animator.SetBool("EnteringSpecial", false);
                //Debug.Log("firespecial");
                GameObject bullet = PhotonNetwork.Instantiate(pfspecial.name, specialpoint.position, specialpoint.rotation);
                Rigidbody2D rbullet = bullet.GetComponent<Rigidbody2D>();
                skillready[1] = false;


                spread = specialpoint.up;
                spread.x = spread.x + (Random.Range(-1, 1) * bulletspread);
                spread.y = spread.y + (Random.Range(-1, 1) * bulletspread);
                //Debug.Log(spread);
                rbullet.AddForce(spread * specialforce, ForceMode2D.Force);

                spermCount = spermCount - 1;
            }
        }

    }
            void AutoRecharge()
    {
        if (spermCount < spermAmmo)
        {
            spermCount = spermCount + spermRate;
        }
    }

            public void PickUpBall()
    {
        spermRate = spermRate + orgiballValue;
        orgiballCount = orgiballCount + 1;
        Debug.Log("picked up ball");
    }
 

}