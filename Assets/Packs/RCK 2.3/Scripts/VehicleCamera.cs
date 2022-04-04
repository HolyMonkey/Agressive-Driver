using UnityEngine;
using System.Collections.Generic;

public class VehicleCamera : MonoBehaviour
{
    public Transform target;
    public float smooth = 0.3f;
    public float distance = 5.0f;
    public float height = 1.0f;
    public float Angle = 20;
    public List<Transform> cameraSwitchView;
    public LayerMask lineOfSightMask = 0;
    private float yVelocity = 0.0f;
    private float xVelocity = 0.0f;
    [HideInInspector] public int Switch;
    private int gearst = 0;
    private float thisAngle = -150;
    private float restTime = 0.0f;
    private Rigidbody myRigidbody;
    private PlayerMover carScript;
    private int PLValue = 0;

    public void PoliceLightSwitch()
    {
        if (!target.gameObject.GetComponent<PoliceLights>()) return;

        PLValue++;

        if (PLValue > 1) PLValue = 0;

        if (PLValue == 1)
            target.gameObject.GetComponent<PoliceLights>().activeLight = true;

        if (PLValue == 0)
            target.gameObject.GetComponent<PoliceLights>().activeLight = false;
    }

    public void CameraSwitch()
    {
        Switch++;
        if (Switch > cameraSwitchView.Count)
        {
            Switch = 0;
        }
    }

    public void RestCar()
    {
        if (restTime == 0)
        {
            myRigidbody.AddForce(Vector3.up * 500000);
            myRigidbody.MoveRotation(Quaternion.Euler(0, transform.eulerAngles.y, 0));
            restTime = 2.0f;
        }
    }

    private void Start()
    {
        carScript = target.GetComponent<PlayerMover>();
        myRigidbody = target.GetComponent<Rigidbody>();
        cameraSwitchView = carScript.carSetting.cameraSwitchView;
    }

    private void Update()
    {
        if (!target) return;
        carScript = target.GetComponent<PlayerMover>();
        if (Input.GetKeyDown(KeyCode.G))
        {
            RestCar();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Application.LoadLevel(Application.loadedLevel);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            PoliceLightSwitch();
        }

        if (restTime != 0.0f)
            restTime = Mathf.MoveTowards(restTime, 0.0f, Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.C))
        {
            Switch++;
            if (Switch > cameraSwitchView.Count)
            {
                Switch = 0;
            }
        }

        if (Switch == 0)
        {
            float xAngle = Mathf.SmoothDampAngle(transform.eulerAngles.x,
                target.eulerAngles.x + Angle, ref xVelocity, smooth);

            float yAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y,
                target.eulerAngles.y, ref yVelocity, smooth);

            transform.eulerAngles = new Vector3(xAngle, yAngle, 0.0f);

            var direction = transform.rotation * -Vector3.forward;
            var targetDistance = AdjustLineOfSight(target.position + new Vector3(0, height, 0), direction);


            transform.position = target.position + new Vector3(0, height, 0) + direction * targetDistance;
        }
        else
        {
            transform.position = cameraSwitchView[Switch - 1].position;
            transform.rotation = Quaternion.Lerp(transform.rotation, cameraSwitchView[Switch - 1].rotation,
                Time.deltaTime * 5.0f);
        }
    }

    private float AdjustLineOfSight(Vector3 target, Vector3 direction)
    {
        RaycastHit hit;

        if (Physics.Raycast(target, direction, out hit, distance, lineOfSightMask.value))
            return hit.distance;
        return distance;
    }
}