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
    public LayerMask PlayerLayerMask;
    private GameObject EchoResults;
    private EchoResult Script;

    void Start()
    {
        Distance = (MinDistance + MaxDistance) / 2;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButton(0)) {
            if (EchoTimer > 0.0f) {
                EchoTimer += Time.deltaTime;

                if (EchoTimer > EchoTimeout) {
                    EchoTimer = 0f;
                }
            } else {
                Vector3 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 Direction = (new Vector3(MousePosition.x, MousePosition.y, 0) - transform.position).normalized;
                EchoResults = GameObject.Find("EchoResults");

                EchoTimer += Time.deltaTime;
                for (float i = -1 * HalfAngle; i < HalfAngle; i += AngleStepSize) {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, Rotate(Direction, i), Distance);
                    if (hit.collider != null) {
                        GameObject SpawnedObject = Instantiate(EchoPrefab, hit.point, Quaternion.identity);
                        // SpawnedObject.GetComponent<EchoResult>().SetInitialColor(hit.distance / Distance);
                        SpawnedObject.transform.SetParent(EchoResults.transform);
                    }
                }  
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0) {
            Distance = Mathf.Min(MaxDistance, Distance + .5f);
            Debug.Log(Distance);
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0) {
            Distance = Mathf.Max(MinDistance, Distance - .5f);
            Debug.Log(Distance);
        }
    }

    Vector2 Rotate(Vector2 Original, float Rotation) {
        float rad = Rotation * Mathf.Deg2Rad;
        return new Vector2(Original.x * Mathf.Cos(rad) - Original.y * Mathf.Sin(rad), 
                            Original.x * Mathf.Sin(rad) + Original.y * Mathf.Cos(rad));
    }
}
