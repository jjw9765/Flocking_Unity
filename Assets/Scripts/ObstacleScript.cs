using UnityEngine;
using System.Collections;


//Obstacle is just a GameObject that we need to avoid

public class ObstacleScript : MonoBehaviour {
	private float radius; //hard coded or set in inspector for now
	
	public float Radius {
		get { return radius; }
	}
	
	void Start() {
		//calculate the radius
		float side = transform.localScale.x/2.0f;
		radius = Mathf.Sqrt(side * side * 2); 	//pythagorean theorem
	}
	
	
}

	