using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagment : MonoBehaviour
{

    public int lifes = 5;
    public int health = 100;

    public CheckPointFinder checkPointFinder;


    void die()
    {
        //play dying animation
        if (lifes != 0)
        {
            lifes--;
            health = 100;
            Respawn();

            //block in a position with sleep

        }
        else
        {
            //enable godmode, player looses
        }


    }

    private void Respawn()
    {
        if (checkPointFinder)
        {
            transform.position = checkPointFinder.LastCheckPoint+Vector3.up*0.5f;
        }
    }

    public void loseHealth(int ammount)
    {
        if (this.health - ammount < 0)
        {
            this.health = 0;
            die();
        }
        else
            this.health -= ammount;
    }

    void earnHealth(int ammount)
    {
        if (this.health + ammount > 100)
            this.health = 100;
        else
            this.health += ammount;
    }

    void loseLife()
    {
        this.lifes--;
    }

    void earnLife()
    {
        this.lifes++;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Resp"))
        {
            die();
        }
    }




}
