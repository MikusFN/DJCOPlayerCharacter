using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TwoPlayersCustom : MonoBehaviour
{

    float speed = 30.0f;

    public GameObject model1;
    public GameObject model2;
    public Button redButton1;
    public Button redButton2;
    public Button blueButton1;
    public Button blueButton2;
    public string color1;
    public string color2;

    void Awake() {
        setBlue2();
        redButton1.Select();
        redButton2.interactable = false;
        blueButton1.interactable = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        setBlue2();
        redButton1.Select();
        redButton2.interactable = false;
        blueButton1.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        model1.transform.Rotate(Vector3.up * speed * Time.deltaTime);
        model2.transform.Rotate(Vector3.up * speed * Time.deltaTime);
        
    }

    public void setRed1() {
        model1.GetComponent<Renderer>().material.color = new Color32(255, 0, 0, 200);
        color1 = "red";
    }

    public void setBlue1() {
        model1.GetComponent<Renderer>().material.color = new Color32(0, 0, 255, 200);
        color1 = "blue";
    }

    public void setPurple1() {
        model1.GetComponent<Renderer>().material.color = new Color32(255, 0, 200, 200);
        color1="purple";
    }

    public void setGreen1() {
        model1.GetComponent<Renderer>().material.color = new Color32(0, 255, 0, 200);
        color1 = "green";
    }

    public void setRed2() {
        model2.GetComponent<Renderer>().material.color = new Color32(255, 0, 0, 200);
        color2 = "red";
    }

    public void setBlue2() {
        model2.GetComponent<Renderer>().material.color = new Color32(0, 0, 255, 200);
        color2 = "blue";
    }

    public void setPurple2() {
        model2.GetComponent<Renderer>().material.color = new Color32(255, 0, 200, 200);
        color2="purple";
    }

    public void setGreen2() {
        model2.GetComponent<Renderer>().material.color = new Color32(0, 255, 0, 200);
        color2 = "green";
    }

    public void nextScreen() {

        if(color1 == "red") {

        }

        else if(color1 == "blue") {

        }

        else if(color1 == "purple") {

        }

        else if(color1== "green") {

        }
    }
}
