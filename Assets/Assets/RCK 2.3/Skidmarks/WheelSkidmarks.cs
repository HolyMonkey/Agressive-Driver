using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class WheelSkidmarks : MonoBehaviour {

	#pragma strict

	//@script RequireComponent(WheelCollider)//We need a wheel collider

	public GameObject skidCaller;//The parent oject having a rigidbody attached to it.
	public float startSlipValue = 0.4f;
	private Skidmarks skidmarks = null;//To hold the skidmarks object
	private int lastSkidmark = -1;//To hold last skidmarks data
	private WheelCollider wheel_col;//To hold self wheel collider
	private PlayerMover _playerMover;//To hold self wheel collider


	private void Start()
	{
		//Get the Wheel Collider attached to self
		skidCaller = transform.root.gameObject;
		wheel_col = GetComponent<WheelCollider>();
		//find object "Skidmarks" from the game
		if (FindObjectOfType(typeof(Skidmarks)))
		{
			skidmarks = FindObjectOfType(typeof(Skidmarks)) as Skidmarks;
		}
		else
			Debug.Log("No skidmarks object found. Skidmarks will not be drawn");

		if (FindObjectOfType(typeof(PlayerMover)))
		{
			_playerMover = FindObjectOfType(typeof(PlayerMover)) as PlayerMover;
		}
	}

    private void FixedUpdate()
    {
        if (_playerMover.IsSkidmarks)
        {
			StartSkidmarks();
		}
    }

	private void StartSkidmarks()
	{
		WheelHit GroundHit; 
		wheel_col.GetGroundHit(out GroundHit);
		var wheelSlipAmount = Mathf.Abs(GroundHit.sidewaysSlip);


		Vector3 skidPoint  = GroundHit.point + 2*(skidCaller.GetComponent<Rigidbody>().velocity) * Time.deltaTime;
		
		lastSkidmark = skidmarks.AddSkidMark(skidPoint, GroundHit.normal, wheelSlipAmount/2.0f, lastSkidmark);	
	}
}