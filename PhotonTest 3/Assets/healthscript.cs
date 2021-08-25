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
    public PhotonView pv;

    private void Awake()
    {
        hpi = hp;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "bullet")
        {

            hpi = hpi - collision.relativeVelocity.magnitude;
            hpnumber.text = hpi.ToString();
            //Debug.Log(hpnumber.text);
        }
    }
    private void FixedUpdate()
    {
        if (pv.IsMine)
        {
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
        hpnumber.text = "full";
    }
}
