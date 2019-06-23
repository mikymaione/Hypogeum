using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAgainAudio : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (AudioThroughScenes.Instance.gameObject.GetComponent<AudioSource>().isPlaying != true)
		{
			AudioThroughScenes.Instance.gameObject.GetComponent<AudioSource>().Play();
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
