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

    public GameObject Sphere;

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

        if (controller.GetPressDown(gripButton))
        {
            gripPos = controller.transform.pos;
        }
        if (controller.GetPress(gripButton))
        {
            cntrDist = Vector3.Distance(controller.transform.pos, gripPos);
            Sphere.transform.RotateAround(Sphere.transform.position, Vector3.up, cntrDist);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
       
    }

    private void OnTriggerExit(Collider collider)
    {
        
    }
}