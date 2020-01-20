using UnityEngine;
using System.Collections;

public class BoatEngine : MonoBehaviour 
{
    //Drags
    public Transform waterJetTransform;

    //What's the boat's maximum engine power?
    public float maxPower;

    //What's the rudder angle range?
    public float maxRudder;

    //The boat's current engine power is public for debugging
    public float currentJetPower;
 
    //The boat's current rudder is public for debugging
    public float currentRudder;
    
    private Rigidbody boatRB;

    BoatController boatController;

    void Start() 
	{
        boatRB = GetComponent<Rigidbody>();

        boatController = GetComponent<BoatController>();
    }


    void Update() 
	{
        EngineState curEngineState = new EngineState();
        curEngineState.power = currentJetPower / maxPower*100.0f;
        curEngineState.rudder = waterJetTransform.localEulerAngles.y;
        if (curEngineState.rudder < 180f) curEngineState.rudder/= maxRudder;
        else curEngineState.rudder = (curEngineState.rudder - 360f) / maxRudder;
        curEngineState.rudder *= 100.0f;

        EngineState newEngineState=boatController.UpdateEngine(curEngineState);

        currentJetPower = newEngineState.power / 100.0f * maxPower;
        float rotation = 0f;
        if (newEngineState.rudder >= 0f) rotation = newEngineState.rudder/100.0f * maxRudder;
        else rotation = 360f + newEngineState.rudder/100.0f * maxRudder;

        currentRudder = rotation;

        Vector3 newRotation = new Vector3(0f, rotation, 0f);

        waterJetTransform.localEulerAngles = newRotation;

    }

    void FixedUpdate()
    {
        UpdateWaterJet();
    }


    void UpdateWaterJet()
    {
        //Debug.Log(boatController.CurrentSpeed);

        Vector3 forceToAdd = -waterJetTransform.forward * currentJetPower;

        //Only add the force if the engine is below sea level
        float waveYPos = WaterController.current.GetWaveYPos(waterJetTransform.position, Time.time);

        if (waterJetTransform.position.y < waveYPos)
        {
            boatRB.AddForceAtPosition(forceToAdd, waterJetTransform.position);
        }
        else
        {
            boatRB.AddForceAtPosition(Vector3.zero, waterJetTransform.position);
        }
    }
}	

public class EngineState
{
    
    public float power; // 0 to 100
    public float rudder; // -100 to 100
}