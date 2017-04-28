using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Notes{
	public GameObject leftNote, downNote, upNote, rightNote;
}

public class GameController : MonoBehaviour {

	List<int> whichNote = new List<int>() {1, 2, 2, 4, 2, 3, 3, 1, 2, 1, 3, 2, 3, 3, 4, 3, 2, 3, 4, 4, 3, 2, 3, 3};
	List<int> xPosList = new List<int>() {0, -3, -1, 1, 3};

	public int noteMark = 0;
	public Notes notes;
	public string timerReset = "y";
	public Text scoreText;
	public Text comboText;

	private int score = 0;
	private int combo = 0;

	void Start () {
		
	}
	
	void FixedUpdate () {
		updateScoreAndCombo();
		if (timerReset == "y") {
			StartCoroutine  (spawnNote ());
			timerReset = "n";
		}
	}

	IEnumerator spawnNote () {
		yield return new WaitForSeconds (1);

		Vector3 notePos = new Vector3 (xPosList[whichNote[noteMark]], transform.position.y, transform.position.z - 1);
		switch(whichNote[noteMark]){
			case 1:
				Instantiate (notes.leftNote, notePos, notes.leftNote.transform.rotation);
				break;
			case 2:
				Instantiate (notes.downNote, notePos, notes.downNote.transform.rotation);
				break;
			case 3:
				Instantiate (notes.upNote, notePos, notes.upNote.transform.rotation);
				break;
			case 4:
				Instantiate (notes.rightNote, notePos, notes.rightNote.transform.rotation);
				break;
			default:
				break;
		}
		noteMark += 1;
		timerReset = "y";
	}

	public void comboSuccess() {
		combo += 1;
		score += 10 * combo;
		updateScoreAndCombo();
	}

	public void comboFail() {
		combo = 0;
		updateScoreAndCombo();
	}

	void updateScoreAndCombo() {
		scoreText.text = "Score: " + score;
		comboText.text = "Combo: " + combo;
	}
}
