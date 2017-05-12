using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FontBlinking : MonoBehaviour {
	public float minOpacity;
	public float r;

	private float angle;
	private Text loadingText;

	void Start () {
		loadingText = GetComponent<Text>();
		angle = 0;
		Debug.Log("Color: " + loadingText.color);
	}
	
	void FixedUpdate () {
		float newAlpha = Mathf.Clamp(Mathf.Cos(angle) * r + minOpacity + r, minOpacity, 1.0F);
		Color c = new Vector4(loadingText.color.r, loadingText.color.g, loadingText.color.b, newAlpha);
		loadingText.color = c;
		angle += 0.05f;
	}
}
