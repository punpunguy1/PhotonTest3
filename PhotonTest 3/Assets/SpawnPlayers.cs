using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerprefab;
    public GameObject orgiballpf;
    public float minX;
    public float minY;
    public float maxX;
    public float maxY;

    public float ballspawntime;
    private float ballspawncooldown;
    // Start is called before the first frame update
    void Start()
    {
        Vector2 randomPosition = new Vector2(Random.Range(minX, maxX), (Random.Range(minY, maxY)));
        PhotonNetwork.Instantiate(playerprefab.name, randomPosition, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > ballspawncooldown)
        {
            spawnOrgiball();
            ballspawncooldown = Time.time + ballspawntime;
        }
    }
    public void spawnOrgiball()
    {
        Vector2 randomPosition = new Vector2(Random.Range(minX, maxX), (Random.Range(minY, maxY)));
        PhotonNetwork.Instantiate(orgiballpf.name, randomPosition, Quaternion.identity);
    }
}
