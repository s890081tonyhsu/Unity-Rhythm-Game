using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Notes{
	public bool left;
	public bool right;
	public bool up;
	public bool down;
}

public struct NoteData{
	public List<List<Notes>> bars;
}

public class MetaData{
	public bool valid;
	public DirectoryInfo dir;

	public string title;
	public string subtitle;
	public string artist;

	public string bannerPath;
	public string backgroundPath;
	public string musicPath;

	public float offset;
	public Dictionary<float, float> bpms;

	public float sampleStart;
	public float sampleLength;

	public NoteData beginner;
	public bool beginnerExists;
	public NoteData easy;
	public bool easyExists;
	public NoteData medium;
	public bool mediumExists;
	public NoteData hard;
	public bool hardExists;
	public NoteData challenge;
	public bool challengeExists;

	public MetaData(DirectoryInfo dir, string smFilePath){
		this.dir = dir;
		this.valid = true;
		this.beginnerExists = false;
		this.easyExists = false;
		this.mediumExists = false;
		this.hardExists = false;
		this.challengeExists = false;
		using (StreamReader sr = new StreamReader(smFilePath)){
			string[] lines = sr.ReadToEnd().Split("\n"[0]);
			this.iterateDetails(lines);
		}
	}

	private void iterateDetails(string[] lines){
		bool inNotes = false;
		for(int i = 0; i < lines.Length; i += 1){
			string line = lines[i].Trim();
			if(line.StartsWith("//")) continue;
			else if(line.StartsWith("#")){
				string key = line.Substring(0, line.IndexOf(':')).Trim('#').Trim(':');

				switch(key.ToUpper()){
					case "TITLE":
						this.title = line.Substring(line.IndexOf(':')).Trim(':').Trim(';');
						break;
					case "SUBTITLE":
						this.subtitle = line.Substring(line.IndexOf(':')).Trim(':').Trim(';');
						break;
					case "ARTIST":
						this.artist = line.Substring(line.IndexOf(':')).Trim(':').Trim(';');
						break;
					case "BANNER":
						this.bannerPath = this.dir.FullName + "\\" + line.Substring(line.IndexOf(':')).Trim(':').Trim(';');
						break;
					case "BACKGROUND":
						this.backgroundPath = this.dir.FullName + "\\" + line.Substring(line.IndexOf(':')).Trim(':').Trim(';');
						break;
					case "MUSIC":
						this.musicPath = this.dir.FullName + "\\" + line.Substring(line.IndexOf(':')).Trim(':').Trim(';');
						if (!File.Exists(this.musicPath)){
							//No music file found!
							this.musicPath = null;
							this.valid = false;
						}
						break;
					case "OFFSET":
						if (!float.TryParse(line.Substring(line.IndexOf(':')).Trim(':').Trim(';'), out this.offset)){
							//Error Parsing
							this.offset = 0.0f;
						}
						break;
					case "SAMPLESTART":
						if (!float.TryParse(line.Substring(line.IndexOf(':')).Trim(':').Trim(';'), out this.sampleStart)){
							//Error Parsing
							this.sampleStart = 0.0f;
						}
						break;
					case "SAMPLELENGTH":
						if (!float.TryParse(line.Substring(line.IndexOf(':')).Trim(':').Trim(';'), out this.sampleLength)){
							//Error Parsing
							this.sampleLength = 10.0f;
						}
						break;
					case "BPMS":
						string bpmStr = line.Substring(line.IndexOf(':')).Trim(':').Trim(';');
						if(bpmStr.Length == 0){
							this.musicPath = null;
							this.valid = false;
							break;
						}
						string[] bpmStrA = bpmStr.Split(',');
						foreach(string bpm in bpmStrA){
							int eqPos = line.IndexOf('=');
							float mapPos = 0.0F, mapBpm = 0.0F;
							if(float.TryParse(bpm.Substring(0, eqPos-1), out mapPos) && float.TryParse(bpm.Substring(eqPos), out mapBpm)){
								bpms.Add(mapPos, mapBpm);
							}
						}
						break;
					case "NOTES":
						inNotes = true;
						break;
					default:
						break;
				}
			}

			if(inNotes){
				if (line.ToLower().Contains("dance-double")){
					for(int j = i; j < lines.Length; j++){
						if (lines[j].Contains(";")){
							i = j - 1;
							break;
						}
					}
				}

				if (line.ToLower().Contains("beginner") ||
					line.ToLower().Contains("easy") ||
					line.ToLower().Contains("medium") ||
					line.ToLower().Contains("hard") ||
					line.ToLower().Contains("challenge")){
					string difficulty = line.Trim().Trim(':');

					List<string> noteChart = new List<string>();
					for (int j = i; j < lines.Length; j++){
						string noteLine = lines[j].Trim();
						if (noteLine.EndsWith(";")){
							i = j - 1;
							break;
						}else{
							noteChart.Add(noteLine);
						}
					}

					switch (difficulty.ToLower().Trim()){
						case "beginner":
							this.beginnerExists = true;
							this.beginner = ParseNotes(noteChart);
							break;
						case "easy":
							this.easyExists = true;
							this.easy = ParseNotes(noteChart);
							break;
						case "medium":
							this.mediumExists = true;
							this.medium = ParseNotes(noteChart);
							break;
						case "hard":
							this.hardExists = true;
							this.hard = ParseNotes(noteChart);
							break;
						case "challenge":
							this.challengeExists = true;
							this.challenge = ParseNotes(noteChart);
							break;
					}
				}

				if (line.EndsWith(";")) inNotes = false;
			}
		}
	}
	private NoteData ParseNotes(List<string> notes){
		NoteData noteData = new NoteData();
		noteData.bars = new List<List<Notes>>();

		List<Notes> bar = new List<Notes>();
		for(int i = 0; i < notes.Count; i++){
			string line = notes[i].Trim();

			if (line.StartsWith(";")) break;

			if (line.EndsWith(",")){
				noteData.bars.Add(bar);
				bar = new List<Notes>();
			} else if(line.EndsWith(":")){
				continue;
			} else if (line.Length <= 4) {
				Notes note = new Notes();
				note.left = false;
				note.down = false;
				note.up = false;
				note.right = false;

				if (line[0] != '0') note.left = true;
				if (line[1] != '0') note.down = true;
				if (line[2] != '0') note.up = true;
				if (line[3] != '0') note.right = true;

				//We then add this information to our current bar and continue until end
				bar.Add(note);
			}
		}

		return noteData;
	}
}