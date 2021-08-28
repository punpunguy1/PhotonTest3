using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class selfdestruct : MonoBehaviour
{
    public PhotonView self;
    public float delay;
    public bool destroyoncollsion;
    public string tagname;
    public bool destroyoncontact;
    public string contacttag;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!(collision.gameObject.tag == tagname))
        {
            if (destroyoncollsion)
            {
                PhotonNetwork.Destroy(self);
            }
            
        }
        if ((collision.gameObject.tag == contacttag))
        {
            if (destroyoncontact)
            {
                PhotonNetwork.Destroy(self);
            }

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
            PhotonNetwork.Destroy(self);
        }
    }

}