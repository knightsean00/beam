using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoResult : MonoBehaviour
{
    public float ExistLength = 2f;
    private float PassedTime = 0f;
    
    public float FinalR = 124;
    public float FinalG = 116;
    public float FinalB = 249;
    public float FinalAlpha = 0;

    public float InitialR = 99;
    public float InitialG = 176;
    public float InitialB = 212;
    public float InitialAlpha = 255;

    private SpriteRenderer sprite;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake() {
        FinalR = Normalize(FinalR);
        FinalG = Normalize(FinalG);
        FinalB = Normalize(FinalB);
        FinalAlpha = Normalize(FinalAlpha);
        InitialR = Normalize(InitialR);
        InitialG = Normalize(InitialG);
        InitialB = Normalize(InitialB);
        InitialAlpha = Normalize(InitialAlpha);
        sprite = GetComponent<SpriteRenderer>();

        // player = GameObject.Find("Player");
        // if (player != null) {

        // }
    }

    public void SetColor(float FractionCompletion) {
        sprite.color = new Color(GetProgression(FractionCompletion, InitialR, FinalR),
                                GetProgression(FractionCompletion, InitialG, FinalG),
                                GetProgression(FractionCompletion, InitialB, FinalB),
                                InitialAlpha);
    }
 
    // Update is called once per frame
    void Update()
    {
        PassedTime += Time.deltaTime;
        sprite.color = new Color(sprite.color.r,
                                sprite.color.g,
                                sprite.color.b,
                                GetProgression(PassedTime / ExistLength, InitialAlpha, FinalAlpha));

        // sprite.color = new Color(GetProgression(PassedTime / ExistLength, InitialR, FinalR),
        //                         GetProgression(PassedTime / ExistLength, InitialG, FinalG),
        //                         GetProgression(PassedTime / ExistLength, InitialB, FinalB),
        //                         GetProgression(PassedTime / ExistLength, InitialAlpha, FinalAlpha));

        if (PassedTime > ExistLength) {
            Object.Destroy(this.gameObject);
        }

        transform.Rotate(0, 0, 1);
    }

    float GetProgression(float FractionCompletion, float Initial, float Final) {
        return Initial + ((Final - Initial) * FractionCompletion);
    }

    float Normalize(float Value) {
        return Value > 1 ? Value / 255 : Value;
    }
}
