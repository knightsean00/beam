using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoResult : MonoBehaviour
{
    public float ExistLength = 2f;
    private float PassedTime = 0f;

    public Color Final = new Color32(124, 116, 249, 255);
    public Color Initial = new Color32(99, 176, 212, 255);
    public float FinalAlpha = 0;
    public float InitialAlpha = 255;

    private SpriteRenderer sprite;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake() {
        FinalAlpha = Normalize(FinalAlpha);
        InitialAlpha = Normalize(InitialAlpha);
        sprite = GetComponent<SpriteRenderer>();
    }

    public void SetColor(float FractionCompletion) {
        sprite.color = new Color(GetProgression(FractionCompletion, Initial.r, Final.r),
                                GetProgression(FractionCompletion, Initial.g, Final.g),
                                GetProgression(FractionCompletion, Initial.b, Final.b),
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
