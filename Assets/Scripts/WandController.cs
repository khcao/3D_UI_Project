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
    private Vector3 head2Hand1;
    private Vector3 head2Hand2;
    private Quaternion origRotGrip; // initial rotation of controller when pressing grip button
    private Quaternion origSphereRotGrip; // initial rotation of sphere when pressing grip button
    private GameObject rotatingWidget; // the collided head of the widget we want to rotate
    private Quaternion origRotTrigger; // initial rotation of controller when pressing trigger button
    private Quaternion origSphereRotTrigger; // initial rotation of the sphere when pressing trigger button

    public GameObject Sphere;
	//public GameObject cube;
    public GameObject camera;
    
    // Use this for initialization
    void Start()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        rotatingWidget = null;
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






        // Calculate the controller hitting a mystical sphere that doesn't exist (but we pretend it does)
        // Refer to picture of triangle between hmd, controller and sphere intersection
        float r = Sphere.transform.localScale.x * 0.5f; // radius of the sphere
        Vector3 xVec = camera.transform.position - transform.position; // controller position to camera position vector
        float x = xVec.magnitude; // distance between camera and controller
        Vector3 f = transform.forward; // forward vector of the controller
        float alpha = Mathf.Abs(Vector3.Angle(xVec, f)) * Mathf.Deg2Rad; // angle between controller forward and vector x (between controller and camera)

        float beta = Mathf.Asin(x * Mathf.Sin(alpha) / r); // by Law of Sines
        float delta = (180 - (alpha * Mathf.Rad2Deg) - (beta * Mathf.Rad2Deg)) * Mathf.Deg2Rad;
        float j = r * Mathf.Sin(delta) / Mathf.Sin(alpha);

        Vector3 handToSphere = j * f;
        Vector3 intersectionPos = transform.position + handToSphere; // the mystical raycast that hit the mystical sphere




        


        // Render the laser
        LineRenderer laser = GetComponent<LineRenderer>();
        Vector3[] laserPoints = new Vector3[2];
        laserPoints[0] = transform.position;
        laserPoints[1] = transform.position + (transform.forward * 5);
        laser.SetPositions(laserPoints);

        //Rotate the sphere around the user
        if (controller.GetPressDown(gripButton))
        {
            //gripPos = Vector3.Cross(camera.transform.forward, - camera.transform.up) ;
            /* head2Hand1 = controller.transform.pos - camera.transform.position;
             head2Hand1.y = 0;
             originalRot = Sphere.transform.rotation;*/
            //Sphere.transform.parent = this.transform;
            origRotGrip = transform.rotation;
            origSphereRotGrip = Sphere.transform.rotation;
        }
        else if (controller.GetPress(gripButton))
        {

            /*      head2Hand2 = controller.transform.pos - camera.transform.position;
                  head2Hand2.y = 0;

                  float angleBetween = Vector3.Angle(head2Hand1, head2Hand2) * 1.2f;
                  if ((Vector3.Cross(head2Hand1, head2Hand2)).y > 0)
                  {
                      angleBetween = -1 * angleBetween;
                  }
                  Sphere.transform.RotateAround(Sphere.transform.position, Vector3.up, angleBetween);

                  gripPos = controller.transform.pos;
                  head2Hand1 = controller.transform.pos - camera.transform.position;
                  head2Hand1.y = 0;
                  originalRot = Sphere.transform.rotation;

                  Debug.Log("Original Position: " + head2Hand1);
                  Debug.Log("Current Position: " + head2Hand2);
                  Debug.Log("Cross Product: " + Vector3.Cross(head2Hand1, head2Hand2));
                  Debug.Log("Angle Between: " + angleBetween);
                  */
            //Vector3 newGripPos = Vector3.Cross(camera.transform.forward, -camera.transform.up) - gripPos;
            //newGripPos *= 200;
            //float mag = 0;

            //Check if it goes left or right
            /*if (newGripPos.x > 0) {
				mag = newGripPos.magnitude;
			} else {
				mag = -newGripPos.magnitude;
			}*/
            //Sphere.transform.rotation = originalRot;

            Quaternion angleDelta = (transform.rotation * Quaternion.Inverse(origRotGrip));
            angleDelta = Quaternion.Euler(0, angleDelta.eulerAngles.y, 0);
            Sphere.transform.rotation = origSphereRotGrip * angleDelta;

            //Sphere.transform.position = camera.transform.position;
            //Sphere.transform.rotation = Quaternion.Euler(0, Sphere.transform.rotation.eulerAngles.y, 0);



            //Sphere.transform.rotation *= Quaternion.Euler(0, -newGripPos.x, 0);// * Sphere.transform.rotation;
            //Sphere.transform.rotation *= Quaternion.Euler(0,mag, 0);
            ///gripPos = Vector3.Cross(camera.transform.forward, -camera.transform.up);
            //Debug.Log(newGripPos);



        }
        if (controller.GetPressUp(gripButton))
        {
            //Sphere.transform.parent = null;   
        }
        //Move objects within the sphere
        if (controller.GetPressDown(triggerButton))
		{
            //triggerPos = controller.transform.pos - cube.transform.position;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                Debug.Log("Raycast has hit: " + hit.transform.gameObject.name);
                //if we hit a widget
                if (hit.transform.gameObject.layer == 9 && rotatingWidget == null)
                {
                    rotatingWidget = hit.transform.gameObject;//hit.transform.parent.gameObject; // NOTE: THIS IS REFERRED TO AS "Head" IN THE HIERARCHY
                    // origRotTrigger = transform.rotation;
                    // origSphereRotTrigger = rotatingWidget.transform.rotation;
                }
            }
            else
                rotatingWidget = null;
                
        }
		else if (controller.GetPress(triggerButton))
		{
			//Vector3 newPos = controller.transform.pos - triggerPos;
			//cube.transform.position = newPos;

            // if we are rotating a widget
            if(rotatingWidget != null)
            {
                rotatingWidget.transform.position = intersectionPos;
                rotatingWidget.transform.LookAt(Sphere.transform);
                //Quaternion angleDelta = (transform.rotation * Quaternion.Inverse(origRotTrigger));
                //rotatingWidget.transform.rotation = origSphereRotTrigger * angleDelta;

            }
		}
        else
        {
            rotatingWidget = null;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
       
    }

    private void OnTriggerExit(Collider collider)
    {
        
    }
}