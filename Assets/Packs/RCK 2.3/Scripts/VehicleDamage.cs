using UnityEngine;
using System.Collections;

public class VehicleDamage : MonoBehaviour
{
    public float maxMoveDelta = 1.0f; // maximum distance one vertice moves per explosion (in meters)
    public float maxCollisionStrength = 50.0f;
    public float YforceDamp = 0.1f; // 0.0 - 1.0
    public float demolutionRange = 0.5f;
    public float impactDirManipulator = 0.0f;
    public MeshFilter[] optionalMeshList;
    public AudioSource crashSound;

    public GameObject sparkleEffect;

    private MeshFilter[] meshfilters;
    private float sqrDemRange;

    public void Start()
    {
        if (optionalMeshList.Length > 0)
            meshfilters = optionalMeshList;
        else
            meshfilters = GetComponentsInChildren<MeshFilter>();

        sqrDemRange = demolutionRange * demolutionRange;
    }

    private Vector3 colPointToMe;
    private float colStrength;

    public void OnCollisionEnter(Collision collision)
    {
        Vector3 colRelVel = collision.relativeVelocity;
        colRelVel.y *= YforceDamp;


        if (collision.contacts.Length > 0)
        {

            colPointToMe = transform.position - collision.contacts[0].point;
            colStrength = colRelVel.magnitude * Vector3.Dot(collision.contacts[0].normal, colPointToMe.normalized);


            if (colPointToMe.magnitude > 1.0f && !crashSound.isPlaying)
            {
                crashSound.Play();
                crashSound.volume = colStrength / 200;

                OnMeshForce(collision.contacts[0].point, Mathf.Clamp01(colStrength / maxCollisionStrength));

                Instantiate(sparkleEffect, collision.contacts[0].point, Quaternion.identity);


            }
        }

    }
    
    // if called by SendMessage(), we only have 1 param
    public void OnMeshForce(Vector4 originPosAndForce)
    {
        OnMeshForce((Vector3)originPosAndForce, originPosAndForce.w);

    }

    // force should be between 0.0 and 1.0
    public void OnMeshForce(Vector3 originPos, float force)
    {
        force = Mathf.Clamp01(force);
    }
}
