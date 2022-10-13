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
        if (playerTransform.position.y <=-10){
            playerTransform.position = spawnPosition.position;
        }
    }
}
