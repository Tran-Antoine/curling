using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// This class manages a cellulo object in the game
/// </summary>
public class CelluloAgent : SteeringAgent
{
    public Cellulo _celluloRobot;
    public bool isConnected = false; //Whether isConnected to Real Cellulo Robot or not.
    public bool isMoved = true; //Whether is Moved by the use or not 
    public Vector3 velocityToRobot = Vector3.zero;
    public int agentID { get; protected set;} //Agent Unique ID
    protected static int agentCount_static = 0;
    protected float clock_t = 0.0f;// Internal Clock in seconds.

    private GameObject _leds;
    [SerializeField]
    private Color initialColor;
    private float controlPeriod = 0.1f;

	/// <summary>
	/// Assign the correct agent ID at start
	/// </summary>
    protected virtual void Awake()
    {
        AssignID();
        _leds = this.transform.Find("Leds").gameObject;
        SetVisualEffect(0, initialColor, 0);

    }

    protected virtual void FixedUpdate() {
        clock_t += Time.deltaTime;

        if (_celluloRobot!=null) {
            _celluloRobot.UpdateProperties();
            if(isConnected) RealToUnity();
        }

    }

    public void Spawn(GameObject parent, Vector3 position)
    {
        transform.SetParent(parent.transform);
        this.transform.localPosition = position;
    }

	/// <summary>
	/// Assign the correct agent ID
	/// </summary>
    private void AssignID()
    {
        agentID = ++agentCount_static;
        this.name = "CelluloAgent_"+agentID;
    }

    public void SetControlPeriod(float controlPeriod)
    {
        this.controlPeriod = controlPeriod;
    }

    /// <summary>
    /// Reset the robot
    /// </summary>
    public void ResetRobot()
    {
        if (isConnected) _celluloRobot.Reset();
        
    }
    public void SetRobotToNull()
    {
        isConnected = false;
        _celluloRobot = null;
    }
	/// <summary>
	/// Shutdown the robot
	/// </summary>
    public void Shutdown()
    {
        if (isConnected) _celluloRobot.Shutdown();
    }

    public void OnKidnappedChanged(object sender, EventArgs e){
        //Debug.Log("kidnapped: "+ _celluloRobot.Kidnapped);
    }
    public void OnShutDown(object sender, EventArgs e){
        Invoke("SetRobotToNull", 1);
    }


    public void OnConnectionStatusChanged(object sender, EventArgs e)
    {
        if (_celluloRobot.ConnectionStatus == ConnectionStatus.ConnectionStatusConnected)
        {
            isConnected = true; 
            _celluloRobot.OnKidnappedChanged += OnKidnappedChanged;
            _celluloRobot.OnShutDown += OnShutDown;
            _celluloRobot.OnTouchBegan += OnTouchBegan;
            _celluloRobot.OnTouchReleased += OnTouchReleased;
            _celluloRobot.OnLongTouch += OnLongTouch;
            SetVisualEffect(0, initialColor, 0);
            StartCoroutine(SendVelocityCommandsToRobot());
        }
        else if(_celluloRobot.ConnectionStatus == ConnectionStatus.ConnectionStatusDisconnected)
        {
            _celluloRobot.OnKidnappedChanged -= OnKidnappedChanged;
            _celluloRobot.OnShutDown -= OnShutDown;
            StopCoroutine(SendVelocityCommandsToRobot());
            if (isConnected)
            {
                _celluloRobot = null;
                isConnected = false;
            }
        }
    }

    protected virtual void OnLongTouch(int key)
    {
        //Debug.Log("Touch Key " + key + " is long touched");
    }
    protected virtual void OnTouchBegan(int key)
    {
        //Debug.Log("Touch Key " + key + "is touched");
    }
    protected virtual void OnTouchReleased(int key)
    {
        //.Log("Touch Key " + key + " is released");
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {

    }

    protected virtual void OnCollisionExit(Collision collision)
    {

    }

    IEnumerator SendVelocityCommandsToRobot() {
    while (true) { 
        if(isConnected && _celluloRobot!=null && !isMoved)
        {
            _celluloRobot.SetGoalVelocity(Config.UnityToRealScaleInX(velocity.x),Config.UnityToRealScaleInY(velocity.z),0);
            velocityToRobot = new Vector3(Config.UnityToRealScaleInX(velocity.x),Config.UnityToRealScaleInY(velocity.z),0);
        }
        yield return new WaitForSecondsRealtime(controlPeriod);
        }
    }

    public void RealToUnity(){
        if (isConnected && _celluloRobot.pose.sqrMagnitude>0)
        {
            transform.localPosition = new Vector3(Config.RealToUnityScaleInX(_celluloRobot.pose.x), 0, Config.RealToUnityScaleInY(_celluloRobot.pose.y));
            transform.localRotation = Quaternion.Euler(0, _celluloRobot.pose.z, 0);
        }
    }

    public void SetVisualEffect(VisualEffect effect, Color color, int value){
        switch(effect){
            case VisualEffect.VisualEffectConstSingle:
                _leds.transform.GetChild(value%6).gameObject.GetComponent<Renderer>().materials[0].color = color;
            break;

            case VisualEffect.VisualEffectConstAll:
            default:
                for(int i =0 ; i< _leds.transform.childCount; i++)
                    _leds.transform.GetChild(i).gameObject.GetComponent<Renderer>().materials[0].color = color;
            break; 
        }

        if(isConnected){
            _celluloRobot.SetVisualEffect((long)effect,(int)(color.r*255),(int)(color.g*255),(int)(color.b*255),value);
        }
    }
}
