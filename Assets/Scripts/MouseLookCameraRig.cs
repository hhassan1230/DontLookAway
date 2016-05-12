using UnityEngine;
using System.Collections;

/**
 * MouseLookCameraRig modified for the Playcrafting VR Class
 * Usage: add to the GameObject with the Camera component. This does not run when not in the editor and when a VR device is present.
 * @author John O'Meara
**/
public class MouseLookCameraRig : MonoBehaviour 
{
	//Optional target, this will be what is rotated if non-null.
    public GameObject target;

	//Initial Rotation States, our camera manager is stateful.
	private Quaternion qMouseX = Quaternion.identity;
	private Quaternion qMouseY = Quaternion.identity;

	//These are the input axes names for Mouse Delta X/Delta Y
	private static readonly string Axis_MouseLookX = "Mouse X";
	private static readonly string Axis_MouseLookY = "Mouse Y";

	void Start() 
	{
	    if (target == null)
        {
			//this is fine if you didn't want to set a different target.
            Debug.Log("mouselookcameraRig: null target, using self as target!");
            target = this.gameObject;
        }
	}
	
	void Update () 
	{
        updateCamera();
	}

    private static Quaternion normalize(Quaternion q)
    {
        //Not needed in Unity.
        return q;
        //Normally want: q = q * (1.0 / q.length())? 
    }

	
	void updateCamera()
	{
		#if !UNITY_EDITOR
			//Don't function outside of the Unity Editor
			return;
		#else
			//check that we don't have an HMD connnected:
			if ((UnityEngine.VR.VRSettings.loadedDevice != UnityEngine.VR.VRDeviceType.None)&&(UnityEngine.VR.VRDevice.isPresent))
			{
				return;
			}
		#endif

        float mdx = Input.GetAxis(Axis_MouseLookX);//get mouse deltas! //NOTE: bad on touchpad!
        float mdy = Input.GetAxis(Axis_MouseLookY);
		
		//Rotation Axes:	
        Vector3 MouseXAxis = Vector3.up;//y
        Vector3 MouseYAxis = Vector3.left;//x
        Vector3 ZAxis = Vector3.forward;//z

		//Calculate input amounts
        float scaleMouseLookInput = 4.0f;
        float xAmount = mdx * scaleMouseLookInput;
        float yAmount = mdy * scaleMouseLookInput;
        float zAmount = 0.0f;

        float invertYFlag = 1.0f; //don't invert for now!

		//Generate Quaternions for this frame
		Quaternion qLookRotX = Quaternion.AngleAxis(xAmount, MouseXAxis);
		Quaternion qLookRotY = Quaternion.AngleAxis(yAmount * invertYFlag, MouseYAxis);
		Quaternion qLookRotZ = Quaternion.AngleAxis(zAmount, ZAxis);

		//Update the Quaternions with the input from this frame
		qMouseX = normalize(qMouseX * qLookRotX);
		qMouseY = normalize(qMouseY * qLookRotY);

		{
			//clamp view range a bit:
			qMouseY = new Quaternion(Mathf.Clamp(qMouseY.x, -0.49f, +0.49f), 0, 0, Mathf.Clamp(qMouseY.w, 0.49f, +0.71f));
		}

		//Set Rotation
		this.transform.localRotation = normalize(qMouseX * qMouseY);
    }
}
