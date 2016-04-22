using UnityEngine;
using System.Collections;

public class lightFade : MonoBehaviour {

   Light fireLight;

	// Use this for initialization
	void Start () {
        fireLight = this.GetComponent<Light>();
    }
	
	// Update is called once per frame
	void Update () {
        fireLight.range = Mathf.Lerp(fireLight.range, -0, Time.deltaTime*3.3f);
        if (fireLight.range < 0)
        {
            Destroy(fireLight);        
        }
    }
}
