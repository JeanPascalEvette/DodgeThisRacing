using UnityEngine;
using System.Collections;

public class SpeedBoostAnimation : MonoBehaviour {

    [SerializeField]
    Texture[] speedBoostTextures;

    [SerializeField]
    float changeInterval = 0.5f;

    Renderer materialRenderer;

    // Use this for initialization
    void Start ()
    {
        materialRenderer = this.GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        int textureIndex = Mathf.FloorToInt(Time.time / changeInterval);
        textureIndex = textureIndex % speedBoostTextures.Length;
        materialRenderer.material.mainTexture = speedBoostTextures[textureIndex];

	}


}
