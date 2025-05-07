using UnityEngine;

public class WallEarthquakeShaker : MonoBehaviour
{
    public float shakeForce = 80f;            // How strong the shaking is
    public float shakeDuration = 5f;          // How long to shake
    public float shakeInterval = 0.1f;        // Time between shake impulses

    public GameObject objectToFall;           // The hanging object to drop

    private float timer = 0f;
    private float nextShakeTime = 0f;
    private Rigidbody rb;
    private bool hasFallen = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = false;

        StartEarthquake();  // Automatically start the earthquake
    }

    public void StartEarthquake()
    {
        timer = shakeDuration;
        nextShakeTime = 0f;
        hasFallen = false;
    }

    void FixedUpdate()
    {
        if (timer > 0)
        {
            timer -= Time.fixedDeltaTime;

            if (Time.time >= nextShakeTime)
            {
                Vector3 shakeDirection = Random.insideUnitSphere;
                rb.AddForce(shakeDirection * shakeForce, ForceMode.Impulse);
                nextShakeTime = Time.time + shakeInterval;
            }

            if (timer <= 0 && !hasFallen)
            {
                MakeObjectFall();
                hasFallen = true;
            }
        }
    }

    void MakeObjectFall()
    {
        if (objectToFall != null)
        {
            // Remove the Hinge Joint so the object falls
            HingeJoint hinge = objectToFall.GetComponent<HingeJoint>();
            if (hinge != null)
            {
                Destroy(hinge);
            }

            // Ensure gravity pulls it down
            Rigidbody rb = objectToFall.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = true;
                rb.isKinematic = false;
            }
        }
    }
}
