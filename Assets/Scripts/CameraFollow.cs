using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // public Transform player;
    private GameObject player;

    // Update is called once per frame

    void Awake() {
        player = GameObject.Find("PlayerObject");
    }

    void Update () {
        transform.position = player.transform.position + new Vector3(0, 1, -5);
    }
}