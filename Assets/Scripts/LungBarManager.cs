using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LungBarManager : MonoBehaviour
{
    private Image fillBar;
    private Image waypoint;
    public float maxCapacity = 100; 
    private float capacity;
    
    public float FillSpeed;
    private float timer = 0.0f;

    private GameObject player;
    // public Vector3 respawnPoint;
    public Transform win;

    void Awake() {
        player = GameObject.Find("PlayerObject");
        fillBar = GameObject.Find("LungFill").GetComponent<Image>();
        waypoint = GameObject.Find("Waypoint").GetComponent<Image>();
        capacity = maxCapacity;
    }

    public bool CheckLoseLung(float value) {
        if (capacity - value < 0) {
            return false;
        } else {
            capacity -= value;
            return true;
        }
    }

    //24.56, 10.5

    // 0, -2.014158

    //128 distance default

    public void waypointSpin(){
        float winX = win.position.x+10;
        float winY = win.position.y;

        float x = player.transform.position.x+10;
        float y = player.transform.position.y;

        float angle = Mathf.Atan((winY-y)/(winX-x)) *100 + 90;

        waypoint.transform.rotation = Quaternion.Euler(0, 0, angle);
    }


    // public void LoseLung(float value, float d) {
    //     if (capacity <= 0) {
    //         return;
    //     }

    //     //update value based on loudness of scream
    //     value *= (d/15.0f);

    //     capacity -= value;
    //     fillBar.fillAmount = capacity/100;
    //     if (capacity <= 0) {
    //         Debug.Log("OUT OF LUNG CAPACITY");
    //     }
    // }

    public void GainLung() {
        if (capacity < maxCapacity) {
            capacity = Mathf.Min(maxCapacity, capacity + FillSpeed * Time.deltaTime);
            fillBar.fillAmount = capacity/maxCapacity;
        }
    }

    public void SetLungWanted(float wanted) {
        // TODO: render a second bar for predicted lung
    }

    public float GetCapacity() {
        return capacity;
    }
    
    //public reset lung 
    //reset echolocation -- there should not be any lit up areas

    // EchoResults --- delete each child

    public void ResetLung() {
        capacity = maxCapacity;
        fillBar.fillAmount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        waypointSpin();
        GainLung();
    }
}
