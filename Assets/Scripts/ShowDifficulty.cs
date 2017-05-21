using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowDifficulty : MonoBehaviour {
	public Image _selectorImage;
	public RawImage _beginnerImage;
	public RawImage _easyImage;
	public RawImage _mediumImage;
	public RawImage _hardImage;
	public RawImage _challengeImage;

	void Start() {
	}
	
	public void SetDifficulty(MetaData map) {
		SetOpacity(_beginnerImage, map.beginnerExists);
		SetOpacity(_easyImage, map.easyExists);
		SetOpacity(_mediumImage, map.beginnerExists);
		SetOpacity(_hardImage, map.hardExists);
		SetOpacity(_challengeImage, map.challengeExists);
	}

	void SetOpacity(RawImage image, bool exist) {
		float newAlpha = exist ? 1.0f : 0.2f;
		Color c = new Vector4(image.color.r, image.color.g, image.color.b, newAlpha);
		image.color = c;
	}

	public void SetSelectorPosition(int currentDifficulty) {
		Vector3 newPos = _selectorImage.GetComponent<RectTransform>().localPosition;
		newPos.y = -40 * currentDifficulty;
		_selectorImage.GetComponent<RectTransform>().localPosition = newPos;
	}
}
