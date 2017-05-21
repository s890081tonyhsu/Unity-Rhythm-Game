using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingController : MonoBehaviour {
	public Image black;
	public Animator anim;
	
	private MapController mapController;

	void Start () {
		GameObject mapControllerObject = GameObject.FindWithTag("MapController");
		if(mapControllerObject != null)
			mapController = mapControllerObject.GetComponent<MapController>();
		if(mapController == null)
			Debug.Log("Cannot find 'MapController' script");
	}

	void Update () {
		if(mapController._loadComplete)
			StartCoroutine (loadingEnd ());
	}

	IEnumerator loadingEnd(){
		yield return new WaitForSeconds (1);
		anim.SetBool("Fade", true);
		yield return new WaitUntil(() => black.color.a == 1);
		SceneManager.LoadScene("GameMenu", LoadSceneMode.Single);
	}
}
