using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoResults : MonoBehaviour
{
    private GameObject player;
    private float MaxDistance;
    // public float MaxDistance = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake() {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        MaxDistance = player.GetComponent<Echolocation>().GetMaxDistance();
        // float MaxDistance = 0f;
        // foreach(Transform child in transform) {
        //     MaxDistance = Mathf.Max(MaxDistance, (player.transform.position - child.transform.position).magnitude);
        // }

        foreach(Transform child in transform) {
            if (child.tag == "Danger") {
                child.GetComponent<EchoResult>().SetDangerColor(Mathf.Min((player.transform.position - child.transform.position).magnitude, MaxDistance) / MaxDistance);
            } else {
                child.GetComponent<EchoResult>().SetColor(Mathf.Min((player.transform.position - child.transform.position).magnitude, MaxDistance) / MaxDistance);
            }
        }
    }
}
