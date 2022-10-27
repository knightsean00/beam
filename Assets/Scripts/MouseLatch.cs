using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLatch
{

    private readonly int index;
    public bool Held { get; private set; }
    public float lastTime;
    public float HeldTime { get { return Time.time - lastTime; } }

    public MouseLatch(int index) {
        this.index = index;
    }

    public void Awake() {
        this.lastTime = Time.time;
        this.Held = Input.GetMouseButton(index);
    }

    // Update is called once per frame
    public bool Latch() {
        var oldHeld = Held;
        Held = Input.GetMouseButton(index);
        if (!oldHeld && Held) {
            lastTime = Time.time; // save time at rising edge
        }
        return oldHeld && !Held; // report falling edge
    }
}
