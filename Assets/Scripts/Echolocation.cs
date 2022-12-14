using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Echolocation : MonoBehaviour
{

    private Vector3 Direction;
    private float Distance;
    private float EchoTimer = 0f;
    
    public float EchoTimeout = .15f;
    public float HalfAngle = 15.0f;
    public float AngleStepSize = .1f;
    public float MaxDistance = 15f;
    public float MinDistance = 5f;


    public GameObject EchoPrefab;
    private GameObject EchoResults;
    private GameObject PlayerObject;
    public AudioSource echo;
    public AudioSource quietEcho;

    public Color CrosshairColor;
    public Color CircleColor;
    private LineRenderer CrosshairRenderer;
    private LineRenderer CrosshairCircleRenderer;
    private List<Vector3> Positions = new List<Vector3>();

    void Start()
    {
        Distance = (MinDistance + MaxDistance) / 2;
    }

    void Awake() {
        PlayerObject = GameObject.Find("PlayerObject");
        EchoResults = GameObject.Find("EchoResults");

        CrosshairRenderer = GameObject.Find("Crosshair").GetComponent<LineRenderer>();
        CrosshairRenderer.startColor = CrosshairColor;
        CrosshairRenderer.endColor = CrosshairColor;

        CrosshairCircleRenderer = GameObject.Find("CrosshairCircle").GetComponent<LineRenderer>();
        CrosshairCircleRenderer.startColor = CircleColor;
        CrosshairCircleRenderer.endColor = CircleColor;
    }

    // Update is called once per frame
    void Update() {
        CrosshairRenderer.transform.position = PlayerObject.transform.position;
        CrosshairCircleRenderer.transform.position = PlayerObject.transform.position;

        Vector3 MousePosition = PlayerObject.transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)));
        Vector3 Direction = (new Vector3(MousePosition.x, MousePosition.y, 0)).normalized;

        CrosshairRenderer.positionCount = (int) HalfAngle * 2 + 3;
        Vector3[] Positions = new Vector3[CrosshairRenderer.positionCount];

        Positions[0] = new Vector3(0, 0, 0);
        int count = 1;
        for (float i = -1 * HalfAngle; i <= HalfAngle; i += 1) {
            Positions[count] = Rotate(new Vector3(MousePosition.x, MousePosition.y, 0), i);
            count += 1;
        }
        Positions[Positions.Length - 1] = new Vector3(0, 0, 0);
        CrosshairRenderer.SetPositions(Positions);


        CrosshairCircleRenderer.positionCount = 360 - (int) (HalfAngle * 2) + 3;
        Positions = new Vector3[CrosshairCircleRenderer.positionCount];

        Positions[0] = new Vector3(0, 0, 0);
        count = 1;
        for (float i = HalfAngle; i <= 360 - HalfAngle; i += 1) {
            Positions[count] = Rotate(new Vector3(MousePosition.x, MousePosition.y, 0), i);
            count += 1;
        }
        Positions[Positions.Length - 1] = new Vector3(0, 0, 0);
        CrosshairCircleRenderer.SetPositions(Positions);

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
            if (Input.GetMouseButton(0)) {
                Distance = (new Vector3(MousePosition.x, MousePosition.y, 0)).magnitude;
                
                if (this.GetComponent<LungBarManager>().CheckLoseLung(Distance)) {
                    EchoTimer += Time.deltaTime; // Start cooldown.
                    quietEcho.Play();

                    for (float i = -1 * HalfAngle; i < HalfAngle; i += AngleStepSize) {
                        EcholocationRaycast(Rotate(Direction, i), Distance);
                    }  
                }
            } else if (Input.GetMouseButton(1)) {
                Distance = (new Vector3(MousePosition.x, MousePosition.y, 0)).magnitude;
                
                if (this.GetComponent<LungBarManager>().CheckLoseLung(Distance * 5)) {
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

    Vector2 Rotate(Vector2 Original, float Rotation) {
        float rad = Rotation * Mathf.Deg2Rad;
        return new Vector2(Original.x * Mathf.Cos(rad) - Original.y * Mathf.Sin(rad), 
                            Original.x * Mathf.Sin(rad) + Original.y * Mathf.Cos(rad));
    }

    public float GetMaxDistance() {
        return MaxDistance;
    }

    public float GetDistance() {
        return Distance;
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
