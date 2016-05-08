using UnityEngine;
using System.Collections;

[AddComponentMenu("Effects/SetRenderQueue")]
[RequireComponent(typeof(Renderer))]

public class shieldScript : MonoBehaviour {

    Renderer shieldRenderer;
    float pulse;
    [SerializeField]
    float pulseDuration = 0.75f;
    Color color1 = new Color32(125, 125, 125, 255);
    Color color2 = new Color32(255, 255, 255, 255);

    // Use this for initialization
    void Start () {
        shieldRenderer = this.GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
        pulse = Mathf.PingPong(Time.time, pulseDuration) / pulseDuration;
        shieldRenderer.material.renderQueue = 3001 ;
        shieldRenderer.material.SetColor("_Color", Color.Lerp(color1, color2, pulse));
        shieldRenderer.material.SetColor("_EmissionColor", Color.Lerp(color1, color2, pulse));
    }
}
