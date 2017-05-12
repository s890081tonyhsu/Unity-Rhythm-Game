using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {
	public bool loadComplete;

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
}
