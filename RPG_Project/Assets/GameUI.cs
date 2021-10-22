using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{

    public TextMeshProUGUI goldText;

    //instance 
    public static GameUI instance;

    private void Awake()
    {
        instance = this;
        goldText.text = "<b>Gold:</b> " + 0;
    }

    public void UpdateGoldText(int gold)
    {
        goldText.text = "<b>Gold:</b> " + gold;
    }


}
