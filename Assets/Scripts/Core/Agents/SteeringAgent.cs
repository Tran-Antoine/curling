using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SteeringAgent : MonoBehaviour
{
    public bool blendWeight = false;

    public float maxSpeed = 3f;
    public float maxAccel = 15f;
    public float maxRotation = 360f;
    public float maxAngularAccel = 7.5f;

    public float rotation;
    public Vector3 velocity;
    protected Steering steering;

    protected Dictionary<int, List<Steering>> groups;
    void Start()
    {
        velocity = Vector3.zero;
        steering = new Steering();
        groups = new Dictionary<int, List<Steering>>();
    }

    public virtual void LateUpdate()
    {
        velocity += steering.linear * Time.deltaTime;
        rotation += steering.angular * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        rotation = rotation > maxRotation ? maxRotation : rotation;
        
        rotation = steering.angular == 0.0f ? 0.0f : rotation;
        velocity = steering.linear.sqrMagnitude == 0.0f ? Vector3.zero : velocity;

        steering = new Steering();
    }
    public void SetSteering(Steering steering)
    {
        this.steering = steering;
    }

    public void SetSteering(Steering steering, float weight)
    {
        this.steering.linear += (weight * steering.linear);
        this.steering.angular += (weight * steering.angular);
    }
   
}

public class Steering
{
    public float angular;
    public Vector3 linear;
    public Steering()
    {
        angular = 0.0f;
        linear = Vector3.zero;
    }
}