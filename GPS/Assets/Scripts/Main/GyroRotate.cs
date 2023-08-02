using UnityEngine;
using System.Collections;

public class GyroRotate : MonoBehaviour {
	public Gyroscope gyro;
	private bool gyroSupported;
	private Quaternion rotFix;

	[SerializeField]

	void Start(){
		gyroSupported = SystemInfo.supportsGyroscope;
		if (gyroSupported) {
			gyro = Input.gyro;
			gyro.enabled = true;
			rotFix = new Quaternion (0, 0, 1, 0);
		} else {
			
		}
	}
	 
	void Update(){
		if (gyroSupported) {
			transform.localRotation = gyro.attitude * rotFix;
		}
	}
}