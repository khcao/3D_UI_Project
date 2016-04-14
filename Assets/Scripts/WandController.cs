using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WandController : MonoBehaviour
{
    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    private SteamVR_TrackedObject trackedObj;


    private float cntrDist;
    private Vector3 gripPos;
	private Vector3 triggerPos;

    public GameObject Sphere;
	public GameObject cube;
    // Use this for initialization
    void Start()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    // Update is called once per frame
    void Update()
    {

        //Move Sphere with controller
        if (controller == null)
        {
            Debug.Log("Controller not initialized");
            return;
        }


		//Rotate the sphere around the user
        if (controller.GetPressDown(gripButton))
        {
            gripPos = controller.transform.pos;
        }
        if (controller.GetPress(gripButton))
        {
            Vector3 newGripPos = controller.transform.pos - gripPos;
			newGripPos *= 200;
			float mag = 0;

			//Check if it goes left or right
			if (newGripPos.x > 0) {
				mag = newGripPos.magnitude;
			} else {
				mag = -newGripPos.magnitude;

			}
				
			//Sphere.transform.rotation = Quaternion.Euler(0, newGripPos.x, 0) * Sphere.transform.rotation;
			Sphere.transform.rotation *= Quaternion.Euler(0,mag, 0);
			gripPos = controller.transform.pos;
			Debug.Log(newGripPos);
        }


		//Move objects within the sphere
		if (controller.GetPressDown(triggerButton))
		{
			triggerPos = controller.transform.pos - cube.transform.position;
		}
		if (controller.GetPress(triggerButton))
		{
			Vector3 newPos = controller.transform.pos - triggerPos;
			cube.transform.position = newPos;
		}
    }

    private void OnTriggerEnter(Collider collider)
    {
       
    }

    private void OnTriggerExit(Collider collider)
    {
        
    }
}