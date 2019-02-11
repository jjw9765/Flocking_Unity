using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	private GameObject myGuy;
	private GameObject target;
	private GameObject flag;
	private Color america = new Color(0,0,255,1);
	private Color russia = new Color(255,0,0,1);
	private Vector3 targetPos = new Vector3 (0, 0, 0);
	
	//these variable are visible in the Inspector
	public GameObject TargetPrefab;
	public GameObject GuyPrefab;
	public GameObject ObstaclePrefab;
	public GameObject FlagPrefab;
	
	//public GameObject Centroid;
	
	public ArrayList allGuys = new ArrayList();
	public ArrayList allFlags = new ArrayList();
	
	// Use this for initialization
	void Start () {
		// the plane is 50x50 (with 0,0,0 in center), so make stuff within 40x40

		// create the pillar of fire that we chase
		Vector3 pos = new Vector3(Random.Range(-40, 40), 4f, Random.Range( -40, 40));
		targetPos = pos;
		target = (GameObject)GameObject.Instantiate(TargetPrefab, pos, Quaternion.identity);
		
		//make our guy
		for(int i = 0; i < 5; i++){
		pos = new Vector3(Random.Range(-40, 40), 1.0f, Random.Range(-40, 40)); // in the middle, above the ground 
		myGuy = (GameObject)GameObject.Instantiate(GuyPrefab, pos, Quaternion.identity);
		myGuy.GetComponent<SteeringVehicle>().Target = target.gameObject;
		GetComponent<Renderer>().material.shader = Shader.Find ("BodyColor");
		GetComponent<Renderer>().material.SetColor ("_BodyColor", Color.red);
		allGuys.Add (myGuy);
		}
		
		//make some obstacles
		for (int i=0; i< 200; i++) 
		{
			float rand1 = Random.Range(-250, 250);
			float rand2 = Random.Range(-250, 250);
			float scal;
			pos =  new Vector3(rand1, 0f, rand2);
			Quaternion rot = Quaternion.Euler(0, Random.Range(0, 90), 0);
			GameObject o = (GameObject)GameObject.Instantiate(ObstaclePrefab, pos, rot);
			
			//resize the cubes 
			if((rand1 > 50 || rand1 < -50) && (rand2 > 50 || rand2 < -50)){
			scal = Random.Range(15f, 30f);
			}
			else {
				scal = Random.Range(2f, 10f);
			}
			o.transform.localScale = new Vector3(scal, scal, scal);
		}
		
		//tell camera to follow myGuy
		foreach(GameObject myGuy in allGuys){
		Camera.main.GetComponent<SmoothFollow>().target = myGuy.transform;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		foreach(GameObject myGuy in allGuys){
		if(Vector3.Distance( myGuy.transform.position, target.transform.position) < 5) 
		{
			GameObject newFlag = (GameObject)GameObject.Instantiate(FlagPrefab, targetPos, Quaternion.identity);
				targetPos = new Vector3(Random.Range(-30, 30), 4f, Random.Range(-30, 30));
				target.transform.position = targetPos;
		}
		}
	}
}
