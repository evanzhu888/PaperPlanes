using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.SceneManagement;

public class Plane : Entity
{
private Rigidbody2D rb;

	public Controller controller;

	public Aerodynamic aerodynamics;


    // Use this for initialization
    void Start () {
        controller = this.GetComponent<PlaneController>();
		aerodynamics = this.GetComponent<Aerodynamic>();
		rb = GetComponent<Rigidbody2D>();
		rb.inertia = aerodynamics.inertia;
		rb.velocity = new Vector2(3, -1);
	}

	void FixedUpdate() {

		Camera cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		
		if (transform.position.y < 0) {
			die();
		}
        // Body frame velocity
		aerodynamics.bodyVelocity = transform.InverseTransformVector(rb.velocity);

        // Angle of attack
		aerodynamics.alpha = Mathf.Atan2(-aerodynamics.bodyVelocity.y, aerodynamics.bodyVelocity.x);

        // Aerodynamic force
        Vector2 force = aerodynamics.aeroForce();
		rb.AddForce(force);

		// Control torque
		float pitchCommand = controller.GetAction();

		rb.AddTorque(pitchCommand * aerodynamics.controlStrength * aerodynamics.bodyVelocity.sqrMagnitude);

        // Stability torque
		rb.AddTorque(-aerodynamics.alpha * aerodynamics.stability * aerodynamics.bodyVelocity.sqrMagnitude);

        // Damping torque
		rb.AddTorque(-aerodynamics.damping * rb.angularVelocity * Mathf.Deg2Rad);

	}

	public Rigidbody2D getRigidBody() {
		return rb;
	}

	public void die() {
		//die and respawn
		//print("died");
		SceneManager.LoadScene("SampleScene"); 
	}
}
