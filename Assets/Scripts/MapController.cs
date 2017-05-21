using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {
	public bool _loadComplete;
	public AudioSource _nowPlaying;
	public bool _isStarted;

	private List<MetaData> metaList;
	private int songIndex;
	private List<int> difficulties;
	private int difficultyIndex;
	private List<startBPM> startBPMs;
	private int nextBPMIndex;

	void Start () {
		DontDestroyOnLoad(this);
		_loadComplete = false;
		metaList = new List<MetaData>();
		DirectoryInfo baseDir = new DirectoryInfo("Songs");
		DirectoryInfo[] dirArray = baseDir.GetDirectories();
		foreach (DirectoryInfo subDir in dirArray) {
			FileInfo[] smFiles = subDir.GetFiles("*.sm");
			if(smFiles.Length == 1) {
				MetaData newSong = new MetaData(subDir, smFiles[0].FullName);
				metaList.Add(newSong);
			}
		}
		_loadComplete = true;
		_isStarted = false;
		songIndex = 0;
		difficulties = new List<int>();
		startBPMs = metaList[songIndex].bpms;
		nextBPMIndex = (startBPMs.Count > 1) ? 1 : 0;
		setDifficulties(metaList[songIndex]);
	}

	void setDifficulties(MetaData map) {
		difficulties.Clear();
		if(map.beginnerExists) difficulties.Add(0);
		if(map.easyExists) difficulties.Add(1);
		if(map.beginnerExists) difficulties.Add(2);
		if(map.hardExists) difficulties.Add(3);
		if(map.challengeExists) difficulties.Add(4);
		difficultyIndex = 0;
	}

	public List<MetaData> getSongList() {
		return metaList;
	}

	public List<int> getDifficultyList() {
		return difficulties;
	}

	public MetaData getCurrentSong() {
		return metaList[songIndex];
	}

	public int getCurrentDifficulty() {
		return difficulties[difficultyIndex];
	}

	public NoteData getCurrentStage() {
		switch(difficulties[difficultyIndex]) {
			case 0:
				return metaList[songIndex].beginner;
			case 1:
				return metaList[songIndex].easy;
			case 2:
				return metaList[songIndex].medium;
			case 3:
				return metaList[songIndex].hard;
			case 4:
				return metaList[songIndex].challenge;
			default:
				return new NoteData();
		}
	}

	public float getCurrentBPM(float gamePos) {
		if(startBPMs.Count == 1) return startBPMs[0].mapBPM;
		if(startBPMs[nextBPMIndex].mapPos <= gamePos)
			nextBPMIndex += 1;
		return startBPMs[nextBPMIndex - 1].mapBPM;
	}

	public float getCurrentOffset() {
		return metaList[songIndex].offset;
	}

	public void shiftSong(bool right) {
		int length = metaList.Count;
		songIndex += right ? 1 : -1;
		if(songIndex == -1) songIndex = length - 1;
		else if(songIndex == length) songIndex = 0;
		startBPMs = metaList[songIndex].bpms;
		setDifficulties(metaList[songIndex]);
	}

	public void shiftDifficulties(bool down) {
		int length = difficulties.Count;
		difficultyIndex += down ? 1 : -1;
		if(difficultyIndex == -1) difficultyIndex = length - 1;
		else if(difficultyIndex == length) difficultyIndex = 0;
	}

	public float getSongTimer() {
		return _nowPlaying.time;
	}

	public IEnumerator playSampleAudio() {
		string path = metaList[songIndex].musicPath;
		float timeStart = metaList[songIndex].sampleStart;
		float timeDuration = metaList[songIndex].sampleLength;
		int playingIndex = songIndex;
		WWW www = new WWW("file://" + path.Replace("\\", "/"));
		yield return www;
		_nowPlaying.clip = (path.LastIndexOf(".mp3") != -1) ? NAudioPlayer.FromMp3Data(www.bytes) : www.GetAudioClip(false);
		while(playingIndex == songIndex) {
			_nowPlaying.volume = 1;
			_nowPlaying.time = timeStart;
			_nowPlaying.Play();
			while(_nowPlaying.time < (timeStart + timeDuration - 1)) {
				yield return new WaitForSeconds(0.01f);
				if(playingIndex != songIndex) break;
			}
			float t = _nowPlaying.volume;
			while (t > 0.0f) {
				if(playingIndex != songIndex) break;
				t -= 0.01f;
				_nowPlaying.volume = t;
				yield return new WaitForSeconds(0.01f);
			}
			if(playingIndex != songIndex) break;
			_nowPlaying.Stop();
		}
	}

	public IEnumerator playAudio(float offset) {
		_isStarted = true;
		yield return new WaitForSeconds(offset);
		_nowPlaying.Play();
		Debug.Log("playing The audio");
	}
}
