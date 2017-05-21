using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
	public Text MetaDataName;
	public Text MetaDataArtist;
	public RawImage BannerBG;
	public Image difficultySelector;
	public Image black;
	public Animator anim;

	private MapController mapController;
	private ShowDifficulty showDifficulty;
	private string nowPlayingPath;
	private bool holdKey;
	private float MaxVol;

	void Start () {
		GameObject mapControllerObject = GameObject.FindWithTag("MapController");
		if(mapControllerObject != null)
			mapController = mapControllerObject.GetComponent<MapController>();
		if(mapController == null)
			Debug.Log("Cannot find 'MapController' script");
		showDifficulty = difficultySelector.GetComponent<ShowDifficulty>();
		holdKey = false;
		loadSong();
	}
	
	void FixedUpdate () {
		float changeSong = Input.GetAxis("Horizontal");
		float changeDifficulty = Input.GetAxis("Vertical");
		if(changeSong != 0 && !holdKey) StartCoroutine(shiftSong(changeSong > 0));
		if(changeDifficulty != 0 && !holdKey) StartCoroutine(shiftDifficulty(changeDifficulty < 0));
		if(Input.GetKeyDown(KeyCode.Return)) StartCoroutine(enterGame());
	}

	IEnumerator shiftSong(bool right){
		holdKey = true;
		mapController.shiftSong(right);
		loadSong();
		yield return new WaitForSeconds(.3f);
		holdKey = false;
	}

	IEnumerator shiftDifficulty(bool down){
		holdKey = true;
		mapController.shiftDifficulties(down);
		setDifficultyShow();
		yield return new WaitForSeconds(.3f);
		holdKey = false;
	}

	void loadSong(){
		try{
			if (!mapController._nowPlaying.isPlaying) mapController._nowPlaying.Stop();
			MetaData current = mapController.getCurrentSong();
			MetaDataName.text = current.title;
			MetaDataArtist.text = current.artist;
			showDifficulty.SetDifficulty(current);
			setDifficultyShow();
			if(current.musicPath != "")
				StartCoroutine (mapController.playSampleAudio());
			StartCoroutine (loadBg(current.bannerPath));
		}catch(Exception e){
			MetaDataName.text = "No songs are loaded.";
			Debug.Log(e);
		}
	}

	void setDifficultyShow(){
		showDifficulty.SetSelectorPosition(mapController.getCurrentDifficulty());
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
	IEnumerator enterGame(){
		mapController._nowPlaying.Stop();
		mapController._nowPlaying.time = 0;
		yield return new WaitForSeconds (1);
		anim.SetBool("Fade", true);
		yield return new WaitUntil(() => black.color.a == 1);
		SceneManager.LoadScene("GameBase", LoadSceneMode.Single);
	}
}
