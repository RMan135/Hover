using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Rigidbody target;
    public Camera camera;

    public float defaultDistance = 5f;
    public float defaultHeight = 1f;
    public float defaultFOV = 60f;
    public float maximumFOVGain = 60f;

    public float positionalStiffness = 10f;
    public float rotationalStiffness = 5f;

    void Start ()
    {
        Reset();
    }

	void FixedUpdate ()
    {
        float targetSpeed = Vector3.Dot(target.velocity, target.transform.forward);

        camera.fov = Mathf.Lerp(camera.fov, defaultFOV + (targetSpeed / 100f) * maximumFOVGain, Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, target.transform.position - target.transform.forward * defaultDistance + target.transform.up * defaultHeight, Time.deltaTime * positionalStiffness * Mathf.Max(1f, targetSpeed));
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Time.deltaTime * rotationalStiffness);
    }

    void Reset ()
    {
        camera.fov = defaultFOV;
        transform.rotation = target.transform.rotation;
        transform.position = target.transform.position - target.transform.forward * defaultDistance + target.transform.up * defaultHeight;
    }
}
