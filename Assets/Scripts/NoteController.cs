using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour {
	public float speed;
	public AudioClip impact;

	public GameObject successBurst;

	private Rigidbody rb;
	private AudioSource audioS;
	private GameController gameController;

	void Start ()
	{
		rb = GetComponent<Rigidbody>();
		rb.velocity = new Vector3(0, 0, speed);
		audioS = GetComponent<AudioSource>();
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if(gameControllerObject != null)
            gameController = gameControllerObject.GetComponent<GameController>();
        if(gameController == null)
            Debug.Log("Cannot find 'GameController' script");
        // Debug.Log(GetInstanceID() + "'s start time: " + gameController.getSongTime());
	}

	void OnTriggerEnter (Collider other) {
		switch(other.gameObject.tag){
			case "FailCollector":
				audioS.PlayOneShot(impact, 0.7F);
				Debug.Log("Failed!");
				gameController.comboFail(GetInstanceID());
				StartCoroutine(destroySelf ());
				break;
			case "SuccessTrigger":
				if(this.gameObject.activeSelf == false)
					break;
				audioS.PlayOneShot(impact, 0.7F);
				Debug.Log("Success!");
				GameObject burst = Instantiate(successBurst, transform.position, successBurst.transform.rotation);
				gameController.comboSuccess();
				Destroy (burst, 1);
				StartCoroutine(destroySelf ());
				break;
			default:
			  break;
		}
	}
	IEnumerator destroySelf() {
		this.gameObject.SetActive(false);
		yield return new WaitForSeconds(2);
		Destroy(rb.gameObject);
	}
}
