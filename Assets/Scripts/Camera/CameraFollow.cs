using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Player _target;
    [SerializeField] private float _smoothSpeed;
    [SerializeField] private Rigidbody rbody;

    private Vector3 _offset;
    private Vector3 _targetPosition;

    private void Start()
    {
        _offset = transform.position - _target.transform.position;
    }

    private void FixedUpdate()
    {
        _targetPosition = _target.transform.position + _offset;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, _targetPosition, _smoothSpeed * Time.fixedDeltaTime);

        transform.position = smoothedPosition;

        transform.LookAt(_target.transform);

        if (Input.GetKeyDown(KeyCode.A))
        {
            StopAllCoroutines();
            StartCoroutine(Skid(-1));
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            StopAllCoroutines();
            StartCoroutine(Skid(1));
        }

    }

    IEnumerator Skid(int direction)
    {
        rbody.velocity += transform.right * 2 * direction;
        for (int i = 0; i < 15; i++)
        {
            yield return new WaitForFixedUpdate();
            rbody.velocity += transform.right * 2 * direction;
        }
        //yield return new WaitForSeconds(0.5f);
        //rbody.velocity = transform.right * 20 * direction;
    }
}
