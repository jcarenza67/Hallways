using UnityEngine;
using System.Collections;

public class LightOptimize : MonoBehaviour {

	public GameObject DistanceFromObject;
	public float VisibilityDistance; // Distance to MainCamera
	public float FloorHeight; // Floor Height will disable light component if MainCamera is +-2 units (meters) from Y axis compared to the light.
	public bool UseFloorHeight;
	private float Distance;
	private Light Lightcomponent;
	private GameObject MainCamera;
	private float PlayerFloor;
	///------------------------------
	void Start(){

		Lightcomponent = gameObject.GetComponent<Light>();

		if (DistanceFromObject == null) // If no additional game object is assigned, use this game object the script is attached to.
        {
			DistanceFromObject = gameObject;
        }

		if (GameObject.FindGameObjectWithTag ("MainCamera") != null) {
			MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		} else {
			Destroy(GetComponent("LightOptimize"));
		}

	}

	// Update is called once per frame
	void Update () {

		if (UseFloorHeight == true)
		{

			PlayerFloor = MainCamera.transform.position.y;

			if (FloorHeight <= PlayerFloor - 2 || FloorHeight >= PlayerFloor + 2)
			{
				Lightcomponent.enabled = false;
			}
			
			else {
			Distance = Vector3.Distance(MainCamera.transform.position, DistanceFromObject.transform.position);

			if (Distance < VisibilityDistance)
			{
				Lightcomponent.enabled = true;
			}
			if (Distance > VisibilityDistance)
			{
				Lightcomponent.enabled = false;
			}
		}

	}

		else
		{
			Distance = Vector3.Distance(MainCamera.transform.position, DistanceFromObject.transform.position);

			if (Distance < VisibilityDistance)
			{
				Lightcomponent.enabled = true;
			}
			if (Distance > VisibilityDistance)
			{
				Lightcomponent.enabled = false;
			}
		}
		
	}

}