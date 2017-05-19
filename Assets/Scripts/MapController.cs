using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {
	public bool loadComplete;
	public AudioSource nowPlaying;

	private List<MetaData> metaList;
	private int songIndex;

	void Start () {
		DontDestroyOnLoad(this);
		loadComplete = false;
		metaList = new List<MetaData>();
		DirectoryInfo baseDir = new DirectoryInfo("Songs");
		DirectoryInfo[] dirArray = baseDir.GetDirectories();
		foreach (DirectoryInfo subDir in dirArray) {
			FileInfo[] smFiles = subDir.GetFiles("*.sm");
			if(smFiles.Length == 1){
				MetaData newSong = new MetaData(subDir, smFiles[0].FullName);
				Debug.Log("Now parsing: " + smFiles[0].FullName);
				metaList.Add(newSong);
			}
		}
		loadComplete = true;
		songIndex = 0;
	}

	public List<MetaData> getSongList(){
		return metaList;
	}

	public MetaData getCurrentSong(){
		return metaList[songIndex];
	}

	public void shiftSong(bool right){
		int length = metaList.Count;
		songIndex += right ? 1 : -1;
		if(songIndex == -1) songIndex = length - 1;
		else if(songIndex == length) songIndex = 0;
	}

	public IEnumerator playSampleAudio(float timeStart, float timeDuration){
		string path = metaList[songIndex].musicPath;
		int playingIndex = songIndex;
		WWW www = new WWW("file://" + path.Replace("\\", "/"));
		yield return www;
		if(path.LastIndexOf(".mp3") != -1)
			nowPlaying.clip = NAudioPlayer.FromMp3Data(www.bytes);
		else
			nowPlaying.clip = www.GetAudioClip(false);
		while(playingIndex == songIndex){
			nowPlaying.volume = 1;
			nowPlaying.time = timeStart;
			nowPlaying.Play();
			while(nowPlaying.time < (timeStart + timeDuration - 1)){
				yield return new WaitForSeconds(0.01f);
				if(playingIndex != songIndex) break;
			}
			float t = nowPlaying.volume;
			while (t > 0.0f) {
				if(playingIndex != songIndex) break;
				t -= 0.01f;
				nowPlaying.volume = t;
				yield return new WaitForSeconds(0.01f);
			}
			if(playingIndex != songIndex) break;
			nowPlaying.Stop();
		}
	}
}
