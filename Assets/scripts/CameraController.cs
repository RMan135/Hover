using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Rigidbody target;

    public float defaultDistance = 10f;
    public float defaultHeight = 3f;
	
    void Start ()
    {
        Reset();
    }

	void FixedUpdate ()
    {
        transform.position = Vector3.Lerp(transform.position, target.transform.position - target.transform.forward * defaultDistance + target.transform.up * defaultHeight, Time.deltaTime * 5f);
    }

    void Reset ()
    {
        transform.position = target.transform.position - target.transform.forward * defaultDistance + target.transform.up * defaultHeight;
    }
}
