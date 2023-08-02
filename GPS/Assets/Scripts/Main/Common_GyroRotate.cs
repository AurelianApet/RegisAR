﻿using UnityEngine;
using System.Collections;

public class Common_GyroRotate : MonoBehaviour {
	#region [Private fields]

	private bool gyroEnabled = true;
	private const float lowPassFilterFactor = 0.2f;

	private readonly Quaternion baseIdentity =  Quaternion.Euler(90, 0, 0);
	private readonly Quaternion landscapeRight =  Quaternion.Euler(0, 0, 90);
	private readonly Quaternion landscapeLeft =  Quaternion.Euler(0, 0, -90);
	private readonly Quaternion upsideDown =  Quaternion.Euler(0, 0, 180);

	private Quaternion cameraBase =  Quaternion.identity;
	private Quaternion calibration =  Quaternion.identity;
	private Quaternion baseOrientation =  Quaternion.Euler(90, 0, 0);
	private Quaternion baseOrientationRotationFix =  Quaternion.identity;

	private Quaternion referanceRotation = Quaternion.identity;
	private bool debug = true;

	#if UNITY_ANDROID
	private AndroidJavaObject curActivity;
	#endif

	#endregion

	#region [Unity events]

	protected void Start () 
	{
		AttachGyro();
	}

	Quaternion getGyroValue()
	{
		return Input.gyro.attitude;
	}

	protected void Update() 
	{
		if (!gyroEnabled)
			return;
                #if UNITY_ANDROID
		transform.rotation = Quaternion.Slerp(transform.rotation,
			cameraBase * ( ConvertRotation(referanceRotation * getGyroValue()) * GetRotFix()), lowPassFilterFactor);
                #elif UNITY_IPHONE
		Quaternion q = cameraBase * (ConvertRotation(referanceRotation * getGyroValue())*GetRotFix());
		Vector3 ab = q.eulerAngles;

		if(Input.compass.enabled == true)
			ab.y = Input.compass.trueHeading;
		
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler (ab), lowPassFilterFactor);
		#endif
	}

	#endregion

	#region [Public methods]

	private void AttachGyro()
	{
		gyroEnabled = true;
		ResetBaseOrientation();
		UpdateCalibration(true);
		UpdateCameraBaseRotation(true);
		RecalculateReferenceRotation();
	}

	private void DetachGyro()
	{
		gyroEnabled = false;
	}

	#endregion

	#region [Private methods]
    	
	private void UpdateCalibration(bool onlyHorizontal)
	{
		if (onlyHorizontal)
		{
			var fw = (getGyroValue()) * (-Vector3.forward);
			fw.z = 0;
			if (fw == Vector3.zero)
			{
				calibration = Quaternion.identity;
			}
			else
			{
				calibration = (Quaternion.FromToRotation(baseOrientationRotationFix * Vector3.up, fw));
			}
		}
		else
		{
			calibration = getGyroValue();
		}
	}
    	
	private void UpdateCameraBaseRotation(bool onlyHorizontal)
	{
		if (onlyHorizontal)
		{
			var fw = transform.forward;
			fw.y = 0;
			if (fw == Vector3.zero)
			{
				cameraBase = Quaternion.identity;
			}
			else
			{
				cameraBase = Quaternion.FromToRotation(Vector3.forward, fw);
			}
		}
		else
		{
			cameraBase = transform.rotation;
		}
	}
    	
	private static Quaternion ConvertRotation(Quaternion q)
	{
		return new Quaternion(q.x, q.y, -q.z, -q.w);	
	}

	private Quaternion GetRotFix()
	{
		#if UNITY_3_5
		if (Screen.orientation == ScreenOrientation.Portrait)
		return Quaternion.identity;

		if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.Landscape)
		return landscapeLeft;

		if (Screen.orientation == ScreenOrientation.LandscapeRight)
		return landscapeRight;

		if (Screen.orientation == ScreenOrientation.PortraitUpsideDown)
		return upsideDown;
		return Quaternion.identity;
		#else
		return Quaternion.identity;
		#endif
	}

	private void ResetBaseOrientation()
	{
		baseOrientationRotationFix = GetRotFix();
		baseOrientation = baseOrientationRotationFix * baseIdentity;
	}

	private void RecalculateReferenceRotation()
	{
		referanceRotation = Quaternion.Inverse(baseOrientation)*Quaternion.Inverse(calibration);
	}

	#endregion
}