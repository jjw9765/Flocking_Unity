using UnityEngine;
using System.Collections;
/*
//This camera smoothes out rotation around the y-axis and height.
//Horizontal Distance to the target is always fixed.
//For every of those smoothed values we calculate the wanted value and the current value.
//Then we smooth it using the Lerp function.
//Then we apply the smoothed values to the transform's position.
*/
public class SmoothFollow : MonoBehaviour 
{
	public Transform target;
	public float distance = 6.0f;
	public float height = 15.5f;
	public float heightDamping = 10.0f;
	public float positionDamping = 4.0f;
	public float rotationDamping = 4.0f;
		
	// Update is called once per frame
	void LateUpdate ()
	{
		// Early out if we don't have a target
		if (!target)
			return;
		
		float dt = Time.deltaTime;
		float wantedHeight = target.position.y + height;
		float currentHeight = transform.position.y;
		
		// Damp the height
		currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * dt);

		// Set the position of the camera 
		Vector3 wantedPosition = target.position - target.forward * distance;
		transform.position = Vector3.Lerp (transform.position, wantedPosition, positionDamping * dt);
	
		// adjust the height of the camera
		transform.position = new Vector3 (transform.position.x, 20, 10);
		
		// look at the target

		transform.forward = Vector3.Lerp (transform.forward, target.position - transform.position, rotationDamping * dt);
		
		}
}