using UnityEngine;
using System.Collections;

public class SphereMove : MonoBehaviour {

    public GameObject camera;
    private SteamVR_TrackedObject trackedObj;

    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }

    private float cntrDist;
    private Vector3 gripPos;
    // Use this for initialization
    void Start () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = camera.transform.position;

        //Move Sphere with controller
        if (controller == null)
        {
            Debug.Log("Controller not initialized");
            return;
        }

        if (controller.GetPressDown(gripButton))
        {
            gripPos = controller.transform.pos;
        }
        if (controller.GetPress(gripButton))
        {
            cntrDist = Vector3.Distance(controller.transform.pos, gripPos);
            transform.RotateAround(transform.position, Vector3.up, cntrDist);
        }

    }
}
