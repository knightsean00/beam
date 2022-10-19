using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LungManager : MonoBehaviour
{
    public int capacity = 6; 
    public Image[] lungs; 

    public void loseLung() {
        if (capacity == 0)
            return;
        capacity --;
        lungs[capacity].enabled = false;

        if (capacity==0) {
            Debug.Log("YOU LOST");
        }
    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return)) {
            loseLung();
        }

    }
}
