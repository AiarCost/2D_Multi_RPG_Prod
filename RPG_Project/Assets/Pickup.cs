using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public enum PickupType
{
    Gold,
    Health
}


public class Pickup : MonoBehaviourPun
{

    public PickupType type;
    public int value;
    public string blockPlayer;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        //A player is picking this up and the player who dropped it is not trying to pick it up
        if (col.CompareTag("Player") && blockPlayer != col.gameObject.GetComponent<PlayerController>().photonPlayer.NickName)
        {
            PlayerController player = col.gameObject.GetComponent<PlayerController>();

            if (type == PickupType.Gold)
                player.photonView.RPC("GiveGold", player.photonPlayer, value);
            else if (type == PickupType.Health)
            {
                player.photonView.RPC("Heal", player.photonPlayer, value);
                StartCoroutine(ChangeActive());
                return;
            }

            PhotonNetwork.Destroy(gameObject);
        }
    }

   IEnumerator ChangeActive()
    {
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;

        yield return new WaitForSeconds(5f);

        gameObject.GetComponent<CircleCollider2D>().enabled = true;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

}
