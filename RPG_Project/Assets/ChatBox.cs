using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Photon.Pun;

public class ChatBox : MonoBehaviourPun

{
    public TextMeshProUGUI chatLogText;
    public TMP_InputField chatInput;
    public PlayerController player;

    //instance
    public static ChatBox instance;

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (EventSystem.current.currentSelectedGameObject == chatInput.gameObject)
                OnChatInputSend();
            else
                EventSystem.current.SetSelectedGameObject(chatInput.gameObject);
        }
    }


    // called when the player wants to send a message
    public void OnChatInputSend()
    {
        if(chatInput.text.Length > 0)
        {
            if (chatInput.text.StartsWith("/Drop"))
            {
                Drop(chatInput.text);
            }
            photonView.RPC("Log", RpcTarget.All, PhotonNetwork.LocalPlayer.NickName, chatInput.text);
            chatInput.text = "";
        }

        EventSystem.current.SetSelectedGameObject(null);
    }

    [PunRPC]
    void Log (string playerName, string message)
    {
        chatLogText.text += string.Format("<br>{0}:</b> {1}", playerName, message);

        chatLogText.rectTransform.sizeDelta = new Vector2(chatLogText.rectTransform.sizeDelta.x, chatLogText.mesh.bounds.size.y + 20);
    }

    //splits the command and will check what value placed in first, then drop that amount if player has that much
    void Drop(string command)
    {
        string[] SplitCommand = command.Split(' ');
        int value;
        foreach( string word in SplitCommand)
        {
            bool intTry = int.TryParse(word, out value);

            if (intTry)
            {
                PlayerController player = GameManager.instance.players[PhotonNetwork.LocalPlayer.ActorNumber - 1];
                bool completion = player.DropGold(value);

                if (completion)
                {
                    //text to show
                    chatLogText.text += string.Format("<br>{0}:</b> {1}", player.photonPlayer.NickName, command);
                    chatLogText.rectTransform.sizeDelta = new Vector2(chatLogText.rectTransform.sizeDelta.x, chatLogText.mesh.bounds.size.y + 20);

                    chatLogText.text += string.Format("<br>Command Completed:</b> {0} dropped", value);
                    chatLogText.rectTransform.sizeDelta = new Vector2(chatLogText.rectTransform.sizeDelta.x, chatLogText.mesh.bounds.size.y + 20);

                    return;
                }

                else
                {
                    chatLogText.text += string.Format("<br>Command Failed.</b> Make sure you have the funds to drop");
                    chatLogText.rectTransform.sizeDelta = new Vector2(chatLogText.rectTransform.sizeDelta.x, chatLogText.mesh.bounds.size.y + 20);
                    return;
                }
            }
        }

        chatLogText.text += string.Format("<br>Command Failed.</b> Make sure you have /Drop 'value'.");
        chatLogText.rectTransform.sizeDelta = new Vector2(chatLogText.rectTransform.sizeDelta.x, chatLogText.mesh.bounds.size.y + 20);


    }
}
