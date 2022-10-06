using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Echolocation : MonoBehaviour
{

    private Vector3 Direction;
    private float Distance;
    
    public float HalfAngle = 15.0f;
    public float AngleStepSize = .5f;
    public float MaxDistance = 15f;
    public float MinDistance = 5f;


    public GameObject EchoPrefab;
    public LayerMask PlayerLayerMask;
    private EchoResult Script;

    void Start()
    {
        Distance = (MinDistance + MaxDistance) / 2;
    }

    // Update is called once per frame
    void Update() {
        Vector3 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 Direction = (new Vector3(MousePosition.x, MousePosition.y, 0) - transform.position).normalized;

        if (Input.GetMouseButton(0)) {
            for (float i = -1 * HalfAngle; i < HalfAngle; i += AngleStepSize) {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Rotate(Direction, i), Distance);
                GameObject SpawnedObject;
                if (hit.collider != null) {
                    Instantiate(EchoPrefab, hit.point, Quaternion.identity).GetComponent<EchoResult>().SetInitialColor(hit.distance / Distance);
                    // SpawnedObject = Instantiate(EchoPrefab, hit.point, Quaternion.identity);
                    // SpawnedObject.GetComponent<EchoResult>().SetInitialColor(hit.distance / Distance);
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
