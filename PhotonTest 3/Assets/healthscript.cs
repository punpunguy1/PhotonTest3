using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;



public class healthscript : MonoBehaviour
{
    public Rigidbody2D enemyrb;
    public float hp = 100f;
    private float hpi;
    public Vector3 impulse;
    private bool dead = false;
    public GameObject mainenemy;
    public GameObject enemy;
    public GameObject respawnmenu;
    public Text hpnumber;
    public Text maxhp;
    public PhotonView pv;

    //audio
    public bool mute;
    public AudioSource hit1;
    public AudioSource hit2;
    public AudioSource hit3;
    

    //UI
    public Slider hpbar;
    

    private void Awake()
    {
        hpbar.maxValue = hp;
        hpi = hp;
        maxhp.text = Mathf.RoundToInt(hp).ToString();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "bullet")
        {

            hpi = hpi - collision.relativeVelocity.magnitude;
            hit();
            
            //Debug.Log(hpnumber.text);
        }
        if (collision.gameObject.tag == "collectible")
        {
            gameObject.GetComponent<MovementScript>().PickUpBall();
        }
    }
    public void hit()
    {
        if (!mute)
        {
            if (!hit1.isPlaying)
            {
                hit1.Play();
            }
            if (hit1.isPlaying)
            {
                if (!hit2.isPlaying)
                hit2.Play();
            }
            if (hit2.isPlaying)
            {
                if (!hit3.isPlaying)
                {
                    hit3.Play();
                }
            }
        }
    }
    private void FixedUpdate()
    {
        if (pv.IsMine)
        {
            hpnumber.text = Mathf.RoundToInt(hpi).ToString();
            hpbar.value = hpi;
            if (hpi < 0)
            {
                dead = true;

            }
            if (dead)
            {
                respawnmenu.SetActive(true);
                hpi = hp;
                mainenemy.GetComponent<MovementScript>().die();
                //Debug.Log("dead");
            }
        }
        
    }
    public void undie()
    {
        dead = false;
        mainenemy.GetComponent<MovementScript>().respawn();
        respawnmenu.SetActive(false);
        
    }
}
