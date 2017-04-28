using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour {
	public float speed;

	public GameObject successBurst;

	private Rigidbody rb;
	private GameController gameController;

	void Start ()
	{
		rb = GetComponent<Rigidbody>();
		rb.velocity = new Vector3(0, 0, speed);
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if(gameControllerObject != null)
            gameController = gameControllerObject.GetComponent<GameController>();
        if(gameController == null)
            Debug.Log("Cannot find 'GameController' script");
	}

	void OnTriggerEnter (Collider other) {
		switch(other.gameObject.tag){
			case "FailCollector":
				Debug.Log("Failed!");
				gameController.comboFail();
				Destroy(rb.gameObject);
				break;
			case "SuccessTrigger":
				Debug.Log("Success!");
				GameObject burst = Instantiate(successBurst, transform.position, successBurst.transform.rotation);
				gameController.comboSuccess();
				Destroy(rb.gameObject);
				Destroy (burst, 1);
				break;
			default:
			  break;
		}
	}
}
