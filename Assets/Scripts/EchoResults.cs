using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoResults : MonoBehaviour
{
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.Find("Player");
        float MaxDistance = 0f;
        foreach(Transform child in transform) {
            MaxDistance = Mathf.Max(MaxDistance, (player.transform.position - child.transform.position).magnitude);
        }

        foreach(Transform child in transform) {
            child.GetComponent<EchoResult>().SetColor(((player.transform.position - child.transform.position).magnitude) / MaxDistance);
        }
    }
}