using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LungBarManager : MonoBehaviour
{
    public Image fillBar;
    private float capacity = 100; 
    public int timer = 0;
    
    private float distance;
    private GameObject player;
    public Vector3 respawnPoint;

    void Awake() {
        player = GameObject.Find("Player");
    }

    //value should be set to 2.0f, d is current loudness (Distance)
    //will calculate the value to be subtracted based on these two values 
    public bool CheckEcholocation(float value, float d) {
        //update value based on loudness of scream
        value *= (d/15.0f);

        if (capacity - value < 0) {
            return false;
        } else {
            capacity -= value;
            return true;
        }
    }

    public void LoseLung(float value, float d) {
        if (capacity <= 0) {
            return;
        }

        //update value based on loudness of scream
        value *= (d/15.0f);

        capacity -= value;
        fillBar.fillAmount = capacity/100;
        if (capacity <= 0) {
            Debug.Log("OUT OF LUNG CAPACITY");
        }
    }

    public void GainLung() {
        if (capacity < 100) {
            capacity += 0.5f;
            fillBar.fillAmount = capacity/100;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += 1;
        if (timer == 25) { //50 calls = 1 second
             GainLung();
             timer = 0;
        }

        if (Input.GetMouseButton(0)) {
            distance = player.GetComponent<Echolocation>().GetDistance();
            Debug.Log(distance);
            LoseLung(2.0f, distance);
        }
        
    }
}
