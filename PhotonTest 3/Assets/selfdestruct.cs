using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class selfdestruct : MonoBehaviour
{
    public PhotonView bullet;
    public float delay;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!(collision.gameObject.tag == "bullet"))
        {

            PhotonNetwork.Destroy(bullet);
        }
    }
    private void Awake()
    {
        

    }
    private void FixedUpdate()
    {
        delay = delay - 1f;
        if (delay < 0)
        {
            PhotonNetwork.Destroy(bullet);
        }
    }

}