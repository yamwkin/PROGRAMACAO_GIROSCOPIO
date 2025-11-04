using NUnit.Framework.Constraints;
using UnityEngine;

public class WallJailController : MonoBehaviour
{
    public float speed = 12f;
    public float deadZone = 0.012f;
    public float sleepVel = 0.2f;
    public Vector3 rotFixEuler = new Vector3(90, 0, 0); //Para landscape ^o^
    public bool autoCalibrationOnStart = true;

    Rigidbody rb;
    Quaternion calib = Quaternion.identity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = 0.3f;
        rb.angularDamping = 0.2f;
        Input.gyro.enabled = true;

        if (autoCalibrationOnStart)
            calib = GetWorldAttitude();

        ZeroMotion();
    }

    Quaternion GetWorldAttitude()
    {
        var g = Input.gyro.attitude;
        var q = new Quaternion(g.x, g.y, -g.z, -g.w);
        return Quaternion.Euler(rotFixEuler) * q;
    }

    void ZeroMotion()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        Quaternion worldRot = GetWorldAttitude();
        Quaternion rel = Quaternion.Inverse(calib) * worldRot;

        Vector3 fwd = rel * Vector3.forward;
        Vector3 dir = new Vector3(fwd.x, 0f, fwd.z);

        if (dir.magnitude < deadZone)
        {
            if (rb.linearVelocity.magnitude < sleepVel && rb.angularVelocity.magnitude < sleepVel)
            {
                rb.Sleep();
            }
            return;

        }
        
            rb.WakeUp();
            rb.AddForce(dir.normalized * dir.magnitude * speed, ForceMode.Acceleration);
        }
}