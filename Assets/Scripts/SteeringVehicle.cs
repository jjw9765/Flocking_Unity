﻿using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Steer))]
[RequireComponent(typeof(CharacterController))]


public class SteeringVehicle : MonoBehaviour {

//movement variables - exposed in inspector panel
	private GameObject target  = null;
	
	//reference to an array of obstacles
	private  GameObject[] obstacles; 
	
	//reference to an array of guys
	private GameObject[] myGuys;
	
	//reference to an array of targets
	private GameObject[] targets;
	
	// These weights will be exposed in the Inspector window

	
	public float gravity = 20.0f; // keep us grounded
	
	// Each vehicle contains a CharacterController which helps to deal with
	// the relationship between movement initiated by the character and the forces
	// generated by contact with the terrain & other game objects.
	private CharacterController characterController;
	
	// the SteeringAttributes holds several variables needed for steering
	private SteeringAttributes attr;

	// the Steer component implements the basic steering functions
	private Steer steer;

	private Vector3 acceleration;	//change in velocity per second
	private Vector3 velocity;		//change in position per second
	public Vector3 Velocity {
		get { return velocity; }
		set { velocity = value;}
	}

	public GameObject Target {
		get { return target; }
		set { target = value;}
	}
	
	void Start ()
	{
		acceleration = Vector3.zero;
		velocity = transform.forward;
		obstacles = GameObject.FindGameObjectsWithTag ("Obstacle");	
		myGuys = GameObject.FindGameObjectsWithTag ("Player");
		targets = GameObject.FindGameObjectsWithTag ("Finish");
		
		//get component references
		characterController = gameObject.GetComponent<CharacterController> ();
		steer = gameObject.GetComponent<Steer> ();
		attr = GameObject.Find("MainGO").GetComponent<SteeringAttributes> ();
	}
	
	void Update ()
	{
		CalcSteeringForce ();
		
		//update velocity
		velocity += acceleration * Time.deltaTime;
		velocity.y = 0;	// we are staying in the x/z plane
		velocity = Vector3.ClampMagnitude (velocity, attr.maxSpeed);
		
		//orient the transform to face where we going
		if (velocity != Vector3.zero)
			transform.forward = velocity.normalized;

		// keep us grounded
		velocity.y -= gravity * Time.deltaTime;

		// the CharacterController moves us subject to physical constraints
		characterController.Move (velocity * Time.deltaTime);
		
		//reset acceleration for next cycle
		acceleration = Vector3.zero;
	}
	
	
	//calculate and apply steering forces
	private void CalcSteeringForce ()
	{ 
		Vector3 force = Vector3.zero;
		
		//obstacles
		for (int i=0; i<obstacles.Length; i++)
		{	
			force += attr.avoidWt * steer.AvoidObstacle (obstacles[i], attr.avoidDist);
		}
		
		//guys
		for(int i = 0; i < myGuys.Length; i++){
			force += attr.avoidWt * steer.Separate(myGuys[i], attr.avoidDist);
			
			//target
			for(int j = 0; j < targets.Length; j++){
				force += steer.Align (targets[j], myGuys[i], attr.avoidDist);
				force += steer.Cohere (targets[j], myGuys[i], attr.avoidDist);
			}
		}
		
		
		Debug.DrawRay (transform.position, force, Color.cyan);
	 
		//in bounds
		force += attr.inBoundsWt * steer.StayInBounds (48, Vector3.zero);
		
		//seek target
		force += attr.seekWt * steer.Seek (target.transform.position);
		
		force = Vector3.ClampMagnitude (force, attr.maxForce);
		ApplyForce(force);
	}

	
	private void ApplyForce (Vector3 steeringForce)
	{
		acceleration += steeringForce/attr.mass;
	}
}
