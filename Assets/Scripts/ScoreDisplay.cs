using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour {
    public Text titleText;
    public Text scoreText;
    public Text maxComboText;
    public Text rankText;
    public Text percentageText;

    private string[] rankTextList = {"E", "D", "C", "B", "A-", "A+", "AA", "AAA"};
    private float[] rankCompare = {0.5f, 0.55f, 0.6f, 0.7f, 0.85f, 0.95f, 1.0f};
    private List<Color> rankTextColor = new List<Color>() {Color.red, Color.magenta, Color.blue, Color.blue, Color.green, Color.green, Color.cyan, Color.gray};

    void Start () {
        if(PlayerPrefs.HasKey("score"))
            scoreText.text = "Score: " + PlayerPrefs.GetInt("score").ToString().PadLeft(8, '0');
        if(PlayerPrefs.HasKey("maxCombo"))
            maxComboText.text = "Max Combo: " + PlayerPrefs.GetInt("maxCombo");
        if(PlayerPrefs.HasKey("playerHitted") && PlayerPrefs.HasKey("totalBeats")) {
            int hitted = PlayerPrefs.GetInt("playerHitted");
            int total = PlayerPrefs.GetInt("totalBeats");
            float percent = (float)hitted / total;
            percentageText.text = (Mathf.Round(percent * 100.0f)).ToString() + "%";
            int rankNum = getRank(percent);
            rankText.text = rankTextList[rankNum];
            rankText.color = rankTextColor[rankNum];
        }
    }

    int getRank(float percent) {
        int rankNum = 0;
        for(int i = 0; i < rankCompare.Length; i++){
            if(percent >= rankCompare[i]) rankNum = i;
        }
        return rankNum;
    }

    void Update () {
        
    }
}
