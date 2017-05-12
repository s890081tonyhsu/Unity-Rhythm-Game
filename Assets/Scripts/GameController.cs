using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class NoteObj{
	public GameObject leftNote, downNote, upNote, rightNote;
}

public class GameController : MonoBehaviour {

	List<int> whichNote = new List<int>() {1, 2, 2, 8, 2, 4, 4, 1, 2, 1, 4, 2, 4, 4, 8, 4, 2, 4, 8, 8, 4, 2, 4, 4};
	List<int> xPosList = new List<int>() {-3, -1, 1, 3};

	public int noteMark;
	public float offset;
	public float BPM;
	public NoteObj notes;
	public string timerReset = "y";
	public Text scoreText;
	public Text comboText;
	public Image black;
	public Animator anim;

	private int score;
	private int hitted;
	private int[] combo;
	private double timer;

	void Start () {
		noteMark = 0;
		score = 0;
		hitted = 0;
		combo = new int[2] {0, 0};
		updateScoreAndCombo();
	}
	
	void FixedUpdate () {
		if (timerReset == "y") {
			if (noteMark < whichNote.Count) {
				StartCoroutine  (spawnNote ());
			} else {
				StartCoroutine (gameEnd ());
			}
			timerReset = "n";
		}
	}

	IEnumerator spawnNote () {
		yield return new WaitForSeconds (BPM / 60);
		Vector3 notePos = new Vector3 (0, transform.position.y, transform.position.z - 1);
		if((whichNote[noteMark] & 0x8) != 0) {
			notePos.x = xPosList[0];
			Instantiate (notes.leftNote, notePos, notes.leftNote.transform.rotation);
		}
		if((whichNote[noteMark] & 0x4) != 0) {
			notePos.x = xPosList[1];
			Instantiate (notes.downNote, notePos, notes.downNote.transform.rotation);
		}
		if((whichNote[noteMark] & 0x2) != 0) {
			notePos.x = xPosList[2];
			Instantiate (notes.upNote, notePos, notes.upNote.transform.rotation);
		}
		if((whichNote[noteMark] & 0x1) != 0) {
			notePos.x = xPosList[3];
			Instantiate (notes.rightNote, notePos, notes.rightNote.transform.rotation);
		}
		noteMark += 1;
		timerReset = "y";
	}

	public void comboSuccess() {
		hitted += 1;
		combo[0] += 1;
		if(combo[0] > combo[1]) combo[1] = combo[0];
		score += 10 * combo[0];
		updateScoreAndCombo();
	}

	public void comboFail() {
		combo[0] = 0;
		updateScoreAndCombo();
	}

	void updateScoreAndCombo() {
		string scoreStr = score.ToString();
		scoreText.text = "Score: \n" + scoreStr.PadLeft(8, '0');
		if(combo[0] > 1)
			comboText.text = "x" + combo[0];
		else
			comboText.text = "";
	}

	IEnumerator gameEnd(){
		yield return new WaitForSeconds (offset);
		PlayerPrefs.SetInt("score", score);
		PlayerPrefs.SetInt("maxCombo", combo[1]);
		PlayerPrefs.SetInt("playerHitted", hitted);
		PlayerPrefs.SetInt("totalBeats", whichNote.Count);
		anim.SetBool("Fade", true);
		yield return new WaitUntil(() => black.color.a == 1);
		SceneManager.LoadScene("ScoreBoard", LoadSceneMode.Single);
	}
}
