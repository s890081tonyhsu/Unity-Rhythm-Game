using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {
	public Text MetaDataText;
	public RawImage backgroundImage;
	public Image black;
	public Animator anim;

	private MapController mapController;
	private Texture defaultBackgroundImage;
	private AudioSource nowPlaying;
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
		defaultBackgroundImage = backgroundImage.mainTexture;
		nowPlaying = GetComponent<AudioSource>();
		MaxVol = nowPlaying.volume;
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
			if (!nowPlaying.isPlaying) nowPlaying.Stop();
			MetaData current = mapController.getCurrentSong();
			MetaDataText.text = current.title + "\n\n" + current.artist;
			if(current.musicPath != "")
				StartCoroutine (playSampleAudio(current.musicPath, current.sampleStart, current.sampleLength));
			if(current.backgroundPath != "")
				StartCoroutine (loadBg(current.backgroundPath));
			else
				backgroundImage.texture = defaultBackgroundImage;
		}catch(Exception e){
			MetaDataText.text = "We're sorry.\n\nYou don't have any song in list.";
			Debug.Log(e);
		}
	}

	IEnumerator loadBg(string path){
		Texture2D tex;
		tex = new Texture2D(4, 4, TextureFormat.DXT5, false);
		WWW www = new WWW("file://" + path.Replace("\\", "/"));
		yield return www;
		www.LoadImageIntoTexture(tex);
		backgroundImage.texture = tex;
	}

	IEnumerator playSampleAudio(string path, float timeStart, float timeDuration){
		nowPlayingPath = path;
		WWW www = new WWW("file://" + path.Replace("\\", "/"));
		yield return www;
		if(path.LastIndexOf(".mp3") != -1)
			nowPlaying.clip = NAudioPlayer.FromMp3Data(www.bytes);
		else
			nowPlaying.clip = www.GetAudioClip(false);
		while(nowPlayingPath == path){
			nowPlaying.volume = MaxVol;
			nowPlaying.time = timeStart;
			nowPlaying.Play();
			while(nowPlaying.time < (timeStart + timeDuration - 1)){
				yield return new WaitForSeconds(0.01f);
				if(nowPlayingPath != path) break;
			}
			float t = nowPlaying.volume;
			while (t > 0.0f) {
				if(nowPlayingPath != path) break;
				t -= MaxVol / 100;
				nowPlaying.volume = t;
				yield return new WaitForSeconds(0.01f);
			}
			if(nowPlayingPath != path) break;
			nowPlaying.Stop();
		}
	}
}
