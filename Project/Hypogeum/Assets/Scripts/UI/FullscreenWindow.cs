using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullscreenWindow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Screen.fullScreen)
		{
			GameObject.Find("FullscreenWindowText").GetComponent<Text>().text = "full screen";
		}
		else
		{
			GameObject.Find("FullscreenWindowText").GetComponent<Text>().text = "window";
		}
    }
}
