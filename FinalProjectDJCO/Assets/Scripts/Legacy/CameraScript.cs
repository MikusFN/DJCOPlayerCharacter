using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class CameraScript : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;


    // Start is called before the first frame update
    void Start()
    {
        
    }



    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3 (player.position.x + offset.x, player.position.y + offset.y,player.position.z + offset.z); // Camera follows the player with specified offset position 
    }
}