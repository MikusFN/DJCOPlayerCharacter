using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{

    public int players = 1;
    public GameObject selectPlayer1;
    public GameObject selectPlayer2;
    public GameObject selectPlayer3;
    public GameObject selectPlayer4;

    public bool button1 = false;
    public bool button2 = false;
    public bool button3 = false;
    public bool button4 = false;

    // Start is called before the first frame update
    void Start()
    {
        button1 = false;
        button2 = false;
        button3 = false;
        button4 = false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setNumberPlayers(int num) {
        players = num;
    }

    public void nextScreen() {
        switch(players) {
            case 1:
            selectPlayer1.SetActive(true);
            button1 = true;
            button2 = false;
            button3 = false;
            button4 = false;

            break;

            case 2:
            selectPlayer2.SetActive(true);
            button2 = true;
            button1 = false;
            button3 = false;
            button4 = false;


            break;

            case 3:
            selectPlayer3.SetActive(true);
            button3 = true;
            button1 = false;
            button2 = false;
            button4 = false;

            break;

            case 4:
            selectPlayer4.SetActive(true);
            button4 = true;
            button1 = false;
            button2 = false;
            button3 = false;

            break;

        }
    }
}
