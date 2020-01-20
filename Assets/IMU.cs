using UnityEngine;
using System.Collections;

public class IMU : MonoBehaviour
{
	public float updateFreq = 2.0f;
	public Vector3 angVel;
	public Vector3 angAccel;
	public Vector3 linVel;
	public Vector3 linAccel;
	public float heading;
	private Vector3 lastPos;
	private Vector3 lastAng;
	private Vector3 lastLinVel;
	private Vector3 lastAngVel;
	private float timer = 0.0f;
 
	void Start()
	{
		Init();
	}

	void Update()
	{
		timer += Time.deltaTime;
		if (timer > (1 / updateFreq))
		{

			lastLinVel = linVel;
			lastAngVel = angVel;

			Vector3 lastPosInv = transform.InverseTransformPoint(lastPos);

			linVel.x = (0f - lastPosInv.x) / timer;
			linVel.y = (0f - lastPosInv.y) / timer;
			linVel.z = (0f - lastPosInv.z) / timer;

			float deltaX = Mathf.Abs((transform.rotation.eulerAngles).x) - lastAng.x;
			if (Mathf.Abs(deltaX) < 180f && deltaX > -180f) angVel.x = deltaX / timer;
			else
			{
				if (deltaX > 180f) angVel.x = (360f - deltaX) / timer;
				else angVel.x = (360f + deltaX) / timer;
			}

			float deltaY = Mathf.Abs((transform.rotation.eulerAngles).y) - lastAng.y;
			if (Mathf.Abs(deltaY) < 180f && deltaY > -180f) angVel.y = deltaY / timer;
			else
			{
				if (deltaY > 180f) angVel.y = (360f - deltaY) / timer;
				else angVel.y = (360f - deltaY) / timer;
			}

			float deltaZ = Mathf.Abs((transform.rotation.eulerAngles).z) - lastAng.z;
			if (Mathf.Abs(deltaZ) < 180f && deltaZ > -180f) angVel.z = deltaZ / timer;
			else
			{
				if (deltaZ > 180f) angVel.z = (360f - deltaZ) / timer;
				else angVel.z = (360f + deltaZ) / timer;
			}


			linAccel.x = (linVel.x - lastLinVel.x) / timer;
			linAccel.y = (linVel.y - lastLinVel.y) / timer;
			linAccel.z = (linVel.z - lastLinVel.z) / timer;
			angAccel.x = ((angVel.x - lastAngVel.x) / timer) / 9.81f;
			angAccel.y = ((angVel.y - lastAngVel.y) / timer) / 9.81f;
			angAccel.z = ((angVel.z - lastAngVel.z) / timer) / 9.81f;

			lastPos = transform.position;

			lastAng.x = Mathf.Abs((transform.rotation.eulerAngles).x);
			lastAng.y = Mathf.Abs((transform.rotation.eulerAngles).y);
			lastAng.z = Mathf.Abs((transform.rotation.eulerAngles).z);

			heading = transform.rotation.eulerAngles.y;

			timer = 0f;
		}

	}

	void OnEnable()
	{
		Init();
	}

	void Init()
	{
	}
}