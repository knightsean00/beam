using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Echolocation : MonoBehaviour
{

    private Vector3 Direction;
    private float EchoTimer = 0f;
    
    public float EchoTimeout = .15f;
    public float HalfAngle = 15.0f;
    public float AngleStepSize = .1f;
    public float MaxDistance = 10f;
    public float MinDistance = 2.5f;
    public float ChargeTime = 5f;
    public float ChargeCostMul = 5f;
    //public float MinCharge = 0.2f;


    public GameObject EchoPrefab;
    private GameObject EchoResults;
    private GameObject PlayerObject;
    public AudioSource echo;
    public AudioSource quietEcho;
    private LungBarManager lungs;

    private MouseLatch leftMouse = new MouseLatch(0);
    private MouseLatch rightMouse = new MouseLatch(1);

    public Color CrosshairColor;
    public Color CircleColor;
    private LineRenderer CrosshairRenderer;
    private LineRenderer CrosshairCircleRenderer;

    void Start()
    {
    }

    void Awake() {
        PlayerObject = GameObject.Find("PlayerObject");
        EchoResults = GameObject.Find("EchoResults");
        lungs = GetComponent<LungBarManager>();

        leftMouse.Awake();
        rightMouse.Awake();

        CrosshairRenderer = GameObject.Find("Crosshair").GetComponent<LineRenderer>();
        CrosshairRenderer.startColor = CrosshairColor;
        CrosshairRenderer.endColor = CrosshairColor;

        CrosshairCircleRenderer = GameObject.Find("CrosshairCircle").GetComponent<LineRenderer>();
        CrosshairCircleRenderer.startColor = CircleColor;
        CrosshairCircleRenderer.endColor = CircleColor;
    }

    // Update is called once per frame
    void Update() {
        var MousePosition = PlayerObject.transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)));
        var Offset = new Vector3(MousePosition.x, MousePosition.y, 0);
        var CursorDistance = Offset.magnitude;
        var Direction = Offset.normalized;

        CrosshairRenderer.transform.position = PlayerObject.transform.position;
        CrosshairCircleRenderer.transform.position = PlayerObject.transform.position;

        Render(Offset);

        if (EchoTimer > 0.0f) {
            EchoTimer += Time.deltaTime;
            CrosshairRenderer.startColor = new Color(CrosshairColor.r, CrosshairColor.g, CrosshairColor.b, 0);
            CrosshairRenderer.endColor = new Color(CrosshairColor.r, CrosshairColor.g, CrosshairColor.b, 0);
            CrosshairCircleRenderer.startColor = new Color(CircleColor.r, CircleColor.g, CircleColor.b, 0);
            CrosshairCircleRenderer.endColor = new Color(CircleColor.r, CircleColor.g, CircleColor.b, 0);

            if (EchoTimer > EchoTimeout) {
                CrosshairRenderer.startColor = CrosshairColor;
                CrosshairRenderer.endColor = CrosshairColor;
                CrosshairCircleRenderer.startColor = CircleColor;
                CrosshairCircleRenderer.endColor = CircleColor;
                EchoTimer = 0f;
            }
        } else if (EchoTimer == 0.0f && !PlayerObject.GetComponent<PlayerMovement>().getDead()) {
            if (leftMouse.Latch()) {
                var Distance = MaxDistance;
                
                if (lungs.CheckLoseLung(Distance)) {
                    EchoTimer += Time.deltaTime; // Start cooldown.
                    quietEcho.Play();

                    for (float i = -1 * HalfAngle; i < HalfAngle; i += AngleStepSize) {
                        EcholocationRaycast(Rotate(Direction, i), Distance);
                    }  
                }
            }
            if (rightMouse.Latch()) {
                var DistanceQ = ChargeDistance();
                
                if (DistanceQ is float Distance && lungs.CheckLoseLung(Distance * ChargeCostMul)) {
                    EchoTimer += Time.deltaTime; // Start cooldown.
                    echo.Play();

                    for (float i = 0; i < 360; i += AngleStepSize) {
                        EcholocationRaycast(Rotate(Direction, i), Distance);
                    }  
                }
            }
        }


        // if (Input.GetAxis("Mouse ScrollWheel") > 0) {
        //     Distance = Mathf.Min(MaxDistance, Distance + .5f);
        //     // Debug.Log(Distance);
        // }

        // if (Input.GetAxis("Mouse ScrollWheel") < 0) {
        //     Distance = Mathf.Max(MinDistance, Distance - .5f);
        //     // Debug.Log(Distance);
        // }
    }

    // Charge distance as dictated by charge time.
    float MaxChargeDistance() {
        var sigSteepness = 12; // adjust for L&F as necessary
        var sigCenter = 0.4f; // adjust for L&F as necessary
        var sigmoidParam = -(rightMouse.HeldTime - ChargeTime * sigCenter) * sigSteepness / ChargeTime;
        var sigmoid = 1 / (1 + Mathf.Exp(sigmoidParam));
        return Mathf.Lerp(MinDistance, MaxDistance, sigmoid);
    }

    // Actual charge distance as constrained by lung cap.
    float? ChargeDistance() {
        var bestDistance = Mathf.Min(MaxChargeDistance(), lungs.GetCapacity() / ChargeCostMul);
        return bestDistance >= MinDistance ? bestDistance : null;
    }

    void Render(Vector3 MousePosition) {
        var CursorDistance = MousePosition.magnitude;
        var Direction = MousePosition.normalized;

        float minHollaDist;
        if (rightMouse.Held && ChargeDistance() is float dist) {
            minHollaDist = dist;
            CrosshairCircleRenderer.positionCount = 360 - (int) (HalfAngle * 2) + 3;
            var Positions = new Vector3[CrosshairCircleRenderer.positionCount];
            var BaseVec = Direction * minHollaDist;

            Positions[0] = new Vector3(0, 0, 0);
            var count = 1;
            for (float i = HalfAngle; i <= 360 - HalfAngle; i += 1) {
                Positions[count] = Rotate(BaseVec, i);
                count += 1;
            }
            Positions[Positions.Length - 1] = new Vector3(0, 0, 0);
            CrosshairCircleRenderer.SetPositions(Positions);
        } else {
            minHollaDist = 0;
            CrosshairCircleRenderer.positionCount = 0;
        }

        {
            CrosshairRenderer.positionCount = (int) HalfAngle * 2 + 3;
            var Positions = new Vector3[CrosshairRenderer.positionCount];
            var BaseVec = Direction * Mathf.Max(Mathf.Max(CursorDistance, MinDistance), minHollaDist);

            Positions[0] = new Vector3(0, 0, 0);
            int count = 1;
            for (float i = -1 * HalfAngle; i <= HalfAngle; i += 1) {
                Positions[count] = Rotate(BaseVec, i);
                count += 1;
            }
            Positions[Positions.Length - 1] = new Vector3(0, 0, 0);
            CrosshairRenderer.SetPositions(Positions);
        }
    }

    Vector2 Rotate(Vector2 Original, float Rotation) {
        float rad = Rotation * Mathf.Deg2Rad;
        return new Vector2(Original.x * Mathf.Cos(rad) - Original.y * Mathf.Sin(rad), 
                            Original.x * Mathf.Sin(rad) + Original.y * Mathf.Cos(rad));
    }

    public float GetMaxDistance() {
        return MaxDistance;
    }

    private void EcholocationRaycast(Vector2 Direction, float CutoffDistance) {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Direction, CutoffDistance);
        if (hit.collider != null) {
            Vector3 SpawnPoint = hit.point;

            GameObject SpawnedObject = Instantiate(EchoPrefab, SpawnPoint, Quaternion.identity);

            if (hit.transform.tag == "Death") {
                SpawnedObject.tag = "Danger";
            }
            // SpawnedObject.GetComponent<EchoResult>().SetInitialColor(hit.distance / Distance);
            SpawnedObject.transform.SetParent(EchoResults.transform);
        }
    }
}
