using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {
	public Text MetaDataText;
	public RawImage BannerBG;
	public Image black;
	public Animator anim;

	private MapController mapController;
	private string nowPlayingPath;
	private bool holdKey;
	private float MaxVol;

	void Start () {
		GameObject mapControllerObject = GameObject.FindWithTag("MapController");
		if(mapControllerObject != null)
			mapController = mapControllerObject.GetComponent<MapController>();
		if(mapController == null)
			Debug.Log("Cannot find 'MapController' script");
		holdKey = false;
		loadSong();
	}
	
	void FixedUpdate () {
		float change = Input.GetAxis("Horizontal");
		if(change != 0 && !holdKey) StartCoroutine(shift(change > 0));
	}

	IEnumerator shift(bool right){
		holdKey = true;
		mapController.shiftSong(right);
		loadSong();
		yield return new WaitForSeconds(.5f);
		holdKey = false;
	}

	void loadSong(){
		try{
			if (!mapController.nowPlaying.isPlaying) mapController.nowPlaying.Stop();
			MetaData current = mapController.getCurrentSong();
			MetaDataText.text = current.artist + " - " + current.title;
			// if(current.musicPath != "")
			// 	StartCoroutine (playSampleAudio(current.musicPath, current.sampleStart, current.sampleLength));
			StartCoroutine (loadBg(current.bannerPath));
		}catch(Exception e){
			MetaDataText.text = "No songs are loaded.";
			Debug.Log(e);
		}
	}

	IEnumerator loadBg(string path){
		Color imageShow = BannerBG.color;
		if(path == ""){
			imageShow.a = 0;
			BannerBG.color = imageShow;
		}else{
			BannerBG.color = imageShow;
			Texture2D tex;
			tex = new Texture2D(4, 4, TextureFormat.DXT5, false);
			WWW www = new WWW("file://" + path.Replace("\\", "/"));
			yield return www;
			www.LoadImageIntoTexture(tex);
			BannerBG.texture = tex;
			imageShow.a = 1;
			BannerBG.color = imageShow;
		}
	}
}
