using System;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    public float speed = 100;
    public float rotationAcceleration;
    public float propellerSpeed = 90;
    public Transform propeller;
    public AudioSource propellerSound;
    public TrailRenderer TrailRenderer;

    private Rigidbody _rigidBody;
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.maxAngularVelocity = (float) (Math.PI / 2);
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
    }

        
    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.IsWasted)
        {
            float acceleration = 0.7f;
            if (Input.GetMouseButton(0))
            {
                RotatePlane();
                acceleration = 1.5f;
            }
            else
            {
                StabilizePlane();
            }
            _rigidBody.AddRelativeForce(Vector3.forward * speed * acceleration, ForceMode.VelocityChange);
            propeller.Rotate(0f, Time.deltaTime * propellerSpeed, 0f);
        }

    }

    private void RotatePlane()
    {
        Vector3 t = new Vector3(
            -Input.GetAxis("Mouse Y"), 
            Input.GetAxis("Mouse X"),
            -Input.GetAxis("Mouse X") / 2f);
        _rigidBody.AddRelativeTorque(t * rotationAcceleration, ForceMode.Impulse);
        propellerSound.pitch = 1 - Mathf.Clamp(t.magnitude, 0f, 0.2f);
    }

    private void StabilizePlane()
    {
        var eulerAngles = transform.rotation.eulerAngles;
        var inverseRotation = new Vector3(GetInverseRotation(eulerAngles.x), 0f, GetInverseRotation(eulerAngles.z));
        if (inverseRotation.x == 0f && inverseRotation.z == 0f)
        {
            var av = _rigidBody.angularVelocity;
            _rigidBody.angularVelocity = new Vector3(0f, av.y, 0f);
        }
        else
        {
            _rigidBody.AddRelativeTorque(inverseRotation * rotationAcceleration / 5f, ForceMode.VelocityChange);
        }
    }


    private float GetInverseRotation(float currentAngle)
    {
        float rotationInput = 0f;
        if (currentAngle < 180)
        {
            rotationInput = -currentAngle / 180;
        }
        else
        {
            rotationInput = 1f - (currentAngle - 180f) / 180f;
        }

        if (rotationInput < 0.05 && rotationInput > -0.05f) rotationInput = 0f;

        return rotationInput;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (GameManager.instance.IsWasted) return;

        if (other.gameObject.CompareTag("Obstacle"))
        {
            Wasted();
        }
    }

    private void Wasted()
    {
        propellerSound.Stop();
        _rigidBody.useGravity = true;
        GameManager.instance.OnLose();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.IsWasted) return;
        var collidedObj = other.gameObject;

        if (collidedObj.CompareTag("Coin"))
        {
            Debug.Log("Collect Coin");

            GameManager.instance.CollectCoin(collidedObj);
           
        }
    }

    public void RestoreInitialState()
    {
        _rigidBody.velocity = Vector3.zero;
        _rigidBody.angularVelocity = Vector3.zero;
        _rigidBody.useGravity = false;
        propellerSound.Play();
        transform.position = _initialPosition;
        transform.rotation = _initialRotation;
        TrailRenderer.Clear();
    }

    public void WinningPose()
    {
        _rigidBody.velocity = transform.TransformVector(Vector3.up);
        transform.rotation = Quaternion.LookRotation(Vector3.up);
        _rigidBody.angularVelocity = transform.TransformDirection(Vector3.forward * (float) Math.PI * 2);
    }
}