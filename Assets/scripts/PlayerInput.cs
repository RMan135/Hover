using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string verticalAxisName = "Vertical";
    public string horizontalAxisName = "Horizontal";
    public string brakingKey = "Brake";

    public float thruster;
    public float rudder;
    public bool isBraking;

    void Update ()
    {
        if (Input.GetButtonDown("Cancel") && !Application.isEditor)
            Application.Quit();

        thruster = Input.GetAxis(verticalAxisName);
        rudder = Input.GetAxis(horizontalAxisName);
        isBraking = Input.GetButton(brakingKey);
    }
}
