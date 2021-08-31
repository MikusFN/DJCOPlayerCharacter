using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberOfPlayers : MonoBehaviour
{

    public GameObject GameManager;
    public Button pressButton1;
    public Button pressButton2;
    public Button pressButton3;
    public Button pressButton4;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void returnScreen() {

        if(GameManager.GetComponent<GameManagerScript>().button1) {
            pressButton1.Select();
        }
        if(GameManager.GetComponent<GameManagerScript>().button2) {
            pressButton2.Select();

        }
        if(GameManager.GetComponent<GameManagerScript>().button3) {
            pressButton3.Select();
        }
        if(GameManager.GetComponent<GameManagerScript>().button4) {
            pressButton4.Select();
        }

    }
}
