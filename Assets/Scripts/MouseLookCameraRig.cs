using UnityEngine;
using System.Collections;

public class MouseLookCameraRig : MonoBehaviour 
{
    public GameObject target;
	public bool disableVerticalLimits = false;

	void Start () 
	{
	    if (target == null)
        {
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
        //HACK:
        return q;
        //TODO: q * (1.0 / q.length())? 

    }

	private Quaternion qMouseX = Quaternion.identity;
	private Quaternion qMouseY = Quaternion.identity;

	void updateCamera()
	{
		#if !UNITY_EDITOR
			return;
		#endif

        float mdx = Input.GetAxis("MouseLookX");//get mouse deltas! //NOTE: bad on touchpad!
        float mdy = Input.GetAxis("MouseLookY");
		
        /* from galactose:
        const Vector3 &mouseXAxis = Vector3::YAxis;
	    const Vector3 &mouseYAxis = Vector3::XAxis;
	    const Vector3 &zAxis = Vector3::ZAxis;//roll
	    if (bFreeLook)
	    {
		    float scaleFreelookAmount = 2.0f;
		    xAmount *= -1.0f;
		    yAmount *= -1.0f;

		    xAmount *= scaleFreelookAmount;
		    yAmount *= scaleFreelookAmount;
	    }
	    Quaternion mouseLookRotX(AxisAngle(+xAmount * (options->isXInverted() ? -1.0f : 1.0f), mouseXAxis));
	    Quaternion mouseLookRotY(AxisAngle(+yAmount * (options->isYInverted() ? -1.0f : 1.0f), mouseYAxis));
	    Quaternion mouseLookRotZ(AxisAngle(+zAmount, zAxis));
	
	    Quaternion qtmp = mouseLookRotX.normalize() * mouseLookRotY.normalize() * mouseLookRotZ.normalize(); 
	    qtmp = qtmp.normalize(); //probably redundant tho

        */
        Vector3 MouseXAxis = Vector3.up;//y
        Vector3 MouseYAxis = Vector3.left;//x
        Vector3 ZAxis = Vector3.forward;//z

        float scaleMouseLookInput = 4.0f;// 2.0f;// 1.0f;
        float xAmount = mdx * scaleMouseLookInput;
        float yAmount = mdy * scaleMouseLookInput;
        float zAmount = 0.0f;

        float invertYFlag = 1.0f; //don't invert for now!

        //Quaternion qLookRotX = new Quaternion(MouseXAxis.x, MouseXAxis.y, MouseXAxis.z, xAmount * 1.0f);
        Quaternion qLookRotX = Quaternion.AngleAxis(xAmount * 1.0f, MouseXAxis);
        Quaternion qLookRotY = Quaternion.AngleAxis(yAmount * invertYFlag, MouseYAxis);
        //Quaternion qLookRotY = new Quaternion(MouseYAxis.x, MouseYAxis.y, MouseYAxis.z, yAmount * 1.0f);
        Quaternion qLookRotZ = Quaternion.AngleAxis(zAmount, ZAxis);
		qMouseX = normalize(qMouseX * qLookRotX);
		qMouseY = normalize(qMouseY * qLookRotY);
		if (true)
		{
			if (!this.disableVerticalLimits)
			{
				//qMouseY = new Quaternion(Mathf.Clamp(qMouseY.x, -0.49f, +0.29f), 0, 0, Mathf.Clamp(qMouseY.w, 0.49f, +0.71f));//down constraint
				//originally, was +/- 0.49f...but that allows designing uncomfortable levels!
				//float upConstraint = -0.29f;//look up amount (towards heaven)
				float upConstraint = -0.17f;// -0.15f;// -0.2f;//look up amount (towards heaven)
				float downConstraint = +0.29f; //look up amount (towards earth)
				qMouseY = new Quaternion(Mathf.Clamp(qMouseY.x, upConstraint, downConstraint), 0, 0, Mathf.Clamp(qMouseY.w, 0.49f, +0.71f));
			}
			else
			{
				//normal branch
				qMouseY = new Quaternion(Mathf.Clamp(qMouseY.x, -0.49f, +0.49f), 0, 0, Mathf.Clamp(qMouseY.w, 0.49f, +0.71f));
			}
		}
		else
		{
			//test: no clamping!
		}

		this.transform.localRotation = normalize(qMouseX * qMouseY);
    }
}
