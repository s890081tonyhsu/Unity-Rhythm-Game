using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccessTrigger : MonoBehaviour {

	public KeyCode activateString;
	public Renderer blinkSubBase;
	public string holdedKey = "n";
	public Material noteBase;
	public Material whiteBlink;

	private Rigidbody rb;

	void Start () {
		rb = GetComponent<Rigidbody>();
		blinkSubBase.sharedMaterial = noteBase;
	}
	
	void FixedUpdate () {
		if (Input.GetKeyDown(activateString) && (holdedKey == "n")) {
			rb.transform.position = new Vector3(rb.transform.position.x, rb.transform.position.y + 1, rb.transform.position.z);
			blinkSubBase.sharedMaterial = whiteBlink;

			StartCoroutine(retractCollider ());
			// holdedKey = "y";
			// This is extends for hold notes...
		}

		if(Input.GetKeyUp(activateString)) {
			holdedKey = "n";
		}
	}

	IEnumerator retractCollider () {
		if(holdedKey == "y") {
			yield return new WaitForSeconds(.1f);
			StartCoroutine(retractCollider ());
		}
		if(holdedKey == "n") {
			yield return new WaitForSeconds(.1f);
			rb.transform.position = new Vector3(rb.transform.position.x, rb.transform.position.y - 1, rb.transform.position.z);
			blinkSubBase.sharedMaterial = noteBase;
		}
	}
}
