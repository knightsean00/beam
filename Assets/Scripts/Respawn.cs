using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{   
    public Transform spawnPosition;
    public Transform playerTransform;

    // Update is called once per frame
    void Update()
    {
        if (playerTransform.position.y <=-10) //fall death
        {
            isDead();
        }
        //clear
    }

    void OnTriggerEnter(Collider col)
    {
         isDead();   
         //if(col.gameObject.tag == "Player")
         //{
         //    isDead();
         //}
    }
    

      void OnCollisionEnter2D(Collision2D collision)
    {
         if (collision.gameObject.tag == "Player")
         {
            isDead();
         }
    }

    void isDead()
    {
        playerTransform.position = spawnPosition.position;
    }
}
