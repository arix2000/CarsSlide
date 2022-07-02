using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Rigidbody rigidbody;
    public float forwardAccel = 8f, reverseAccel = 4f, maxSpeed = 50f, turnStrength = 180, gravityForce = 10f, dragOnGround = 3f, smooth = 80f, speed = 800f;

    private float speedInput, turnInput;

    private bool grounded;

    public LayerMask whatIsRoad, whatIsTerrain;
    public float graundRayLenght = .5f;
    public Transform groundRayPoint;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
            Application.LoadLevel(0);

        speedInput = 0f;
        if(Input.GetAxis("Vertical") > 0) 
        {
            speedInput = Input.GetAxis("Vertical") * forwardAccel * speed;
        } else if(Input.GetAxis("Vertical") < 0) 
        {
            speedInput = Input.GetAxis("Vertical") * reverseAccel * speed/2;
        }

        turnInput = Input.GetAxis("Horizontal");
        
        if(grounded) {
            transform.rotation = Quaternion.Euler(
                transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnStrength * Time.deltaTime * Input.GetAxis("Vertical"), 0f));
        }
        transform.position = rigidbody.transform.position;
    }

    private void FixedUpdate() 
    {
        grounded = false;
        RaycastHit hit;

        if(Physics.Raycast(groundRayPoint.position, -transform.up, out hit, graundRayLenght, whatIsTerrain)) 
        {
            speedInput = Input.GetAxis("Vertical") * forwardAccel * speed / 2.2f;
            grounded = true;
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation, smooth * Time.deltaTime);
        }

        if(Physics.Raycast(groundRayPoint.position, -transform.up, out hit, graundRayLenght, whatIsRoad)) 
        {
            speedInput = Input.GetAxis("Vertical") * forwardAccel * speed;
            grounded = true;
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation, smooth * Time.deltaTime);
        }

        if(grounded) 
        {
            rigidbody.drag = dragOnGround;
            if(Mathf.Abs(speedInput) > 0)
            {
                Vector3 force = transform.forward * speedInput;
                rigidbody.AddForce(force);
            }   
        } else
        {
            rigidbody.drag = 0.1f;
            rigidbody.AddForce(Vector3.up * -gravityForce * 100);
        }
    }
}
