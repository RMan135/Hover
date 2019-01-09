using UnityEngine;

public class VehicleMovement : MonoBehaviour {

    public float speed;

    [Header("Drive Settings")]
    public float driveForce = 17f;
    public float slowingVelFactor = .99f;
    public float brakingVelFactor = .99f;
    public float angleOfRoll = 30f;

    [Header("Hover Settings")]
    public float hoverHeight = 1.5f;
    public float maxGroundDist = 5f;
    public float hoverForce = 300f;
    public LayerMask whatIsGround;
    public PIDController hoverPID;

    [Header("Physics Settings")]
    public Transform shipBody;
    public float terminalVelocity = 100f;
    public float hoverGravity = 20f;
    public float fallGravity = 80f;

    Rigidbody rigidBody;
    PlayerInput input;
    float drag;
    public bool isOnGround;

    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        input = GetComponent<PlayerInput> ();

        drag = driveForce / terminalVelocity;
    }

    void FixedUpdate () {
        speed = Vector3.Dot(rigidBody.velocity, transform.forward);

        CalculateHover();
        CalculatePropulsion();
    }

    void CalculateHover () {
        Vector3 groundNormal;

        Ray ray = new Ray(transform.position, -transform.up);

        Debug.DrawRay(transform.position, -transform.up * maxGroundDist);

        RaycastHit hitInfo;
               
        isOnGround = Physics.Raycast(ray, out hitInfo, maxGroundDist, whatIsGround);

        Debug.DrawRay(hitInfo.point, hitInfo.normal, Color.red);
        Debug.Log("" + hitInfo.distance);

        if (isOnGround)
        {
            float height = hitInfo.distance;
            groundNormal = hitInfo.normal.normalized;
            float forcePercent = hoverPID.Seek(hoverHeight, height);

            Vector3 force = groundNormal * hoverForce * forcePercent;
            Vector3 gravity = -groundNormal * hoverGravity * height;

            rigidBody.AddForce(force, ForceMode.Acceleration);
            rigidBody.AddForce(gravity, ForceMode.Acceleration);
        }
        else
        {
            groundNormal = Vector3.up;
            Vector3 gravity = -groundNormal * fallGravity;
            rigidBody.AddForce(gravity, ForceMode.Acceleration);
        }

        Vector3 projection = Vector3.ProjectOnPlane(transform.forward, groundNormal);
        Quaternion rotation = Quaternion.LookRotation(projection, groundNormal);

        rigidBody.MoveRotation(Quaternion.Lerp(rigidBody.rotation, rotation, Time.deltaTime * 10f));

        float angle = angleOfRoll * -input.rudder;
        Quaternion bodyRotation = transform.rotation * Quaternion.Euler(0f, 0f, angle);
        shipBody.rotation = Quaternion.Lerp(shipBody.rotation, bodyRotation, Time.deltaTime * 10f);
    }

    void CalculatePropulsion () {
        float rotationTorque = input.rudder - rigidBody.angularVelocity.y;

        rigidBody.AddRelativeTorque(0f, rotationTorque, 0f, ForceMode.VelocityChange);

        float sidewaysSpeed = Vector3.Dot(rigidBody.velocity, transform.right);

        Vector3 sideFriction = -transform.right * (sidewaysSpeed / Time.fixedDeltaTime);

        rigidBody.AddForce(sideFriction, ForceMode.Acceleration);

        if (input.thruster <= 0f)
            rigidBody.velocity *= slowingVelFactor;

        if (!isOnGround)
            return;

        if (input.isBraking)
            rigidBody.velocity *= brakingVelFactor;

        float propulsion = driveForce * input.thruster - drag * Mathf.Clamp(speed, 0f, terminalVelocity);
        rigidBody.AddForce(transform.forward * propulsion, ForceMode.Acceleration);
    }

    // Update is called once per frame
    void Update () {
	
	}
}
