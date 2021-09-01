using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class enemyrangedai : MonoBehaviour
{
    public string playertagname;
    public Rigidbody2D rb;
    private GameObject player;
    public Vector2 playerpos;
    public float rotation;
    public float moveSpeed;
    private Vector2 lookDir;
    private float dis2player;
    public float aggrodis;
    public Transform firepoint;
    public float firepointrotation;
    public float bulletspread;
    public float bulletForce;
    public GameObject projectile;
    public float firecooldown;
    private float cd;

    void ProcessInputs()
    {

        player = GameObject.FindGameObjectWithTag(playertagname);
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
            Debug.Log(dis2player + "  dis:" + aggrodis);
            rb.rotation = angle;
            Move();
            
            if (Time.time > cd)
            {
                cd = Time.time + firecooldown;
                Fire();
            }
        }

    }

    void Move()
    {
        rb.velocity = new Vector2(lookDir.x * moveSpeed, lookDir.y * moveSpeed);
    }

    void Fire()
    {
        firepoint.localRotation = Quaternion.identity;
        firepoint.Rotate(Vector3.forward, firepointrotation);
        firepoint.Rotate(Vector3.forward, Random.Range(-bulletspread, bulletspread));

        GameObject bullet = PhotonNetwork.Instantiate(projectile.name, firepoint.position, firepoint.rotation);
        Rigidbody2D rbullet = bullet.GetComponent<Rigidbody2D>();

        rbullet.AddForce(firepoint.up.normalized * bulletForce, ForceMode2D.Force);
    }

}
