using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class btnFX : MonoBehaviour
{
	public Color cursorInsideColor = new Color(200.0f / 255.0f, 200.0f / 255.0f, 200.0f / 255.0f, 128.0f/ 255.0f);
	public Color cursorOutsideColor = new Color(255.0f / 255.0f, 255.0f / 0.0f, 255.0f / 255.0f, 255.0f/ 255.0f);
	//public Button button;
	public Text text;

    public void OnCursorEnter()
	{
		//button = GetComponent<Button>();
		text = GetComponentInChildren<Text>();
		text.color = cursorInsideColor;
	}

	public void onCursorExit()
	{
		//button = GetComponent<Button>();
		text = GetComponentInChildren<Text>();
		text.color = cursorOutsideColor;
	}
}
