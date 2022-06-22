using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnText : MonoBehaviour
{
    public Players players;
    public bool LastTurn;
    
    public TextMeshPro tmp0;
    public TextMeshPro tmp1;

    public string title1 = "White";
    public string title2 = "Black";
    // Start is called before the first frame update
    void Start() {
        LastTurn = players.turn;
    }

    // Update is called once per frame
    void Update()
    {
        if(LastTurn != players.turn)
        {
            if (players.turn)
            {
                tmp0.text = title2;
                tmp1.text = title2;
            }
            else
            {
                tmp0.text = title1;
                tmp1.text = title1;
            }
            LastTurn = players.turn;
        }
    }
}
