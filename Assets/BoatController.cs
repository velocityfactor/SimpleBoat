using UnityEngine;
using System.Collections;

public class BoatController : MonoBehaviour
{
    //Speed calculations

    // public for debugging
    public float currentSpeed;
    private Vector3 lastPosition;
    private IMU imu;
    public Vector3 rel;

    private void Start()
    {
        imu = GetComponent<IMU>();
    }
    void FixedUpdate()
    {
		CalculateSpeed();
	}

    // This is called by BoatEngine's Update function
    public EngineState UpdateEngine(EngineState curEngineState)
    {
        EngineState newEngineState = new EngineState();
        newEngineState.power = curEngineState.power;
        newEngineState.rudder = curEngineState.rudder;

        //Forward / reverse
        if (Input.GetKey(KeyCode.I))
        {
            newEngineState.power = curEngineState.power+1.0f;
            if(newEngineState.power>100.0f)
            {
                newEngineState.power = 100.0f;
            }
        }
        else if (Input.GetKey(KeyCode.K))
        {
            newEngineState.power = curEngineState.power - 1.0f;
            if (newEngineState.power < -100.0f)
            {
                newEngineState.power = -100.0f;
            }
        }
        else
        {
            newEngineState.power = 0;
        }

        //Steer left
        if (Input.GetKey(KeyCode.J))
        {
            newEngineState.rudder += 1.0f;
            if (newEngineState.rudder > 100.0f) newEngineState.rudder = 100.0f;
        }

        //Steer right
        else if (Input.GetKey(KeyCode.L))
        {
            newEngineState.rudder -= 1.0f;
            if (newEngineState.rudder < -100.0f) newEngineState.rudder = -100.0f;
        }
        return newEngineState;
    }


    //Calculate the current speed in m/s
    private void CalculateSpeed()
    {
		//Calculate the distance of the Transform Object between the fixedupdate calls with 
		//'(transform.position - lastPosition).magnitude' Now you know the 'meter per fixedupdate'
		//Divide this value by Time.deltaTime to get meter per second
		currentSpeed = (transform.position - lastPosition).magnitude / Time.deltaTime;

        //Save the position for the next update
        lastPosition = transform.position;
	}
	
	 public float CurrentSpeed
    {
        get
        {
            return this.currentSpeed;
        }
    }
}