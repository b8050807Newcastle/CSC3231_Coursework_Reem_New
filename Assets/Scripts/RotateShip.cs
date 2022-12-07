using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RotateShip : MonoBehaviour
{
    public int rotationSpeed = 75;
    public int movementspeed = 10;
    public int thrust = 10;
    public float RotationSpeed = 5;

    private bool isPKeyDown = false;
    private float acceleration = .0f;
    private Vector3 previousPosition = Vector3.zero;
    private Rigidbody _rigidbody;
    private bool landing = false;
    private Vector3 originPosition;
    private Vector3 lastPosition;
    private const float minDistance = 0.2f;
    private Transform baseTarget;
    private float distanceTraveled;
    private Vector3 targetAngles;

    // Use this for initialization
    void Start()
    {
        baseTarget = GameObject.Find("Space ship").transform;
        originPosition = transform.position;
        _rigidbody = GetComponent<Rigidbody>();
        Debug.Log("Acc Speed: " + thrust);
    }

    // Update is called once per frame
    void Update()
    {
        if (landing == false)
        {
            var v3 = new Vector3(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"), 0.0f);
            transform.Rotate(v3 * rotationSpeed * Time.deltaTime);
            transform.position += transform.forward * Time.deltaTime * movementspeed;

            if (Input.GetKey(KeyCode.Z))
                transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

            if (Input.GetKey(KeyCode.R))
                transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);

            if (Input.GetKey(KeyCode.P))
            {
                isPKeyDown = Input.GetKey(KeyCode.P);
                float distance = Vector3.Distance(previousPosition, transform.position);
                acceleration = distance / Mathf.Pow(Time.deltaTime, 2);

                previousPosition = transform.position;
                _rigidbody.AddRelativeForce(0f, 0f, thrust, ForceMode.Acceleration);
            }
        }
        else
        {
            transform.position += transform.forward * Time.deltaTime * movementspeed;
            /*distanceTraveled += Vector3.Distance(transform.position, lastPosition);
            if (distanceTraveled >= 50)
            {
                targetAngles = transform.eulerAngles + Random.Range(90.0f, 180.0f) * transform.up;
                StartCoroutine(TurnShip(transform, transform.eulerAngles, targetAngles, 1f));
                distanceTraveled = 0;
                landing = false;
            }*/
            var targetRotation = Quaternion.LookRotation(baseTarget.position - transform.position);
            var str = Mathf.Min(.5f * Time.deltaTime, 1);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, str);
        }

        if (landed == true)
            TakeOff();

        if (Input.GetKey(KeyCode.L))
        {
            // Automatic landing
            landing = true;
            lastPosition = transform.position;
        }
    }

    IEnumerator TurnShip(Transform ship, Vector3 startAngle, Vector3 endAngle, float smooth)
    {
        float lerpSpeed = 0;

        while (lerpSpeed < 1)
        {
            ship.eulerAngles = Vector3.Lerp(startAngle, endAngle, lerpSpeed);
            lerpSpeed += Time.deltaTime * smooth;
            yield return null;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (landing == true && other.gameObject.name == "Base")
        {
            StartCoroutine(Landed());
        }
    }

    bool landed = false;
    IEnumerator Landed()
    {
        yield return new WaitForSeconds(5);
        Debug.Log("Landed");
        landed = true;
    }

    private void TakeOff()
    {
        if (transform.position != originPosition)
        {
            _rigidbody.AddForce(transform.up * 10);
        }

        if ((transform.position - originPosition).sqrMagnitude <= (1f * 1f))
        {
            landed = false;
            _rigidbody.useGravity = false;
        }
    }

    void OnGUI()
    {
        if (isPKeyDown)
        {
            GUI.Label(new Rect(100, 100, 200, 200), "Acc Speed: " + acceleration);
        }
    }
}
