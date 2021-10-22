using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;


public class GameManager : MonoBehaviourPun
{

    [Header("Players")]
    public string playerPrefavPath;
    public Transform[] spawnPoints;
    public float respawnTime;

    private int playersInGame;

    public PlayerController[] players;

    //instance
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        players = new PlayerController[PhotonNetwork.PlayerList.Length];
        photonView.RPC("ImInGame", RpcTarget.AllBuffered);
    }


    [PunRPC]
    void ImInGame()
    {
        playersInGame++;

        if(playersInGame == PhotonNetwork.PlayerList.Length)
        {
            SpawnPlayer();
        }
    }

    void SpawnPlayer()
    {
        GameObject playerObj = PhotonNetwork.Instantiate(playerPrefavPath, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);

        // Initialize the player
        playerObj.GetComponent<PhotonView>().RPC("Initialize", RpcTarget.All, PhotonNetwork.LocalPlayer);

    }

}
