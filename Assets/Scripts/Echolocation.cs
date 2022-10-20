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

    public Color LineColor;
    private LineRenderer CrosshairRenderer;
    private List<Vector3> Positions = new List<Vector3>();

    void Start()
    {
        Distance = (MinDistance + MaxDistance) / 2;
    }

    void Awake() {
        PlayerObject = GameObject.Find("PlayerObject");
        EchoResults = GameObject.Find("EchoResults");
        CrosshairRenderer = GameObject.Find("Crosshair").GetComponent<LineRenderer>();
        CrosshairRenderer.startColor = LineColor;
        CrosshairRenderer.endColor = LineColor;
    }

    // Update is called once per frame
    void Update() {
        CrosshairRenderer.transform.position = PlayerObject.transform.position;
        Vector3 MousePosition = PlayerObject.transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)));
        Vector3 Direction = (new Vector3(MousePosition.x, MousePosition.y, 0)).normalized;

        CrosshairRenderer.positionCount = 360 - (int) (HalfAngle * 2) + 2;
        Vector3[] Positions = new Vector3[CrosshairRenderer.positionCount];

        Positions[0] = new Vector3(0, 0, 0);
        int count = 1;
        for (float i = HalfAngle; i < 360 - HalfAngle; i += 1) {
            Positions[count] = Rotate(new Vector3(MousePosition.x, MousePosition.y, 0), i);
            count += 1;
        }
        Positions[Positions.Length - 1] = new Vector3(0, 0, 0);
        CrosshairRenderer.SetPositions(Positions);
        // CrosshairRenderer.positionCount = 3;
        // CrosshairRenderer.SetPositions(new Vector3[3] {
        //     Rotate(new Vector3(MousePosition.x, MousePosition.y, 0), HalfAngle), 
        //     new Vector3(0, 0, 0), 
        //     Rotate(new Vector3(MousePosition.x, MousePosition.y, 0), -HalfAngle)
        // });


        if (EchoTimer > 0.0f) {
            EchoTimer += Time.deltaTime;
            CrosshairRenderer.startColor = new Color(LineColor.r, LineColor.g, LineColor.b, 0);
            CrosshairRenderer.endColor = new Color(LineColor.r, LineColor.g, LineColor.b, 0);

            if (EchoTimer > EchoTimeout) {
                CrosshairRenderer.startColor = LineColor;
                CrosshairRenderer.endColor = LineColor;
                EchoTimer = 0f;
            }
        } else if (EchoTimer == 0.0f) {
            if (Input.GetMouseButton(0)) {
                Distance = (new Vector3(MousePosition.x, MousePosition.y, 0)).magnitude;
                
                if (this.GetComponent<LungBarManager>().CheckLoseLung(Distance)) {
                    EchoTimer += Time.deltaTime; // Start cooldown.

                    for (float i = -1 * HalfAngle; i < HalfAngle; i += AngleStepSize) {
                        EcholocationRaycast(Rotate(Direction, i), Distance);
                    }  
                }
            } else if (Input.GetMouseButton(1)) {
                Distance = (new Vector3(MousePosition.x, MousePosition.y, 0)).magnitude;
                
                if (this.GetComponent<LungBarManager>().CheckLoseLung(Distance * 5)) {
                    EchoTimer += Time.deltaTime; // Start cooldown.

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
