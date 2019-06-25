using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RadarManager : NetworkBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		var rawImage = gameObject.GetComponentInChildren<RawImage>().texture = GB.getAnimalRenderTexture(GB.Animal.Value, GB.GameType.Value);
	}
}
