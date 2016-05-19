using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class trafficLight : MonoBehaviour
{

    public Image redLight;
    public Image orangeLight;
    public Image greenLight;
    public Image fanari;

    private float startTimer;
    private float realTimeTimer;
    // Use this for initialization
    void Start()
    {
        realTimeTimer = Time.realtimeSinceStartup;
        startTimer = 0;
        Time.timeScale = 0F;
    }

    // Update is called once per frame
    void Update()
    {

        startTimer = Time.realtimeSinceStartup;

        if (startTimer - realTimeTimer >= 7f)
        {
            Destroy(gameObject);

        }
        else if (startTimer - realTimeTimer >= 4.6f)
        {
            Color c = fanari.color;
            Color r = redLight.color;
            Color o = orangeLight.color;
            Color g = greenLight.color;
            c.a -= Time.deltaTime * 0.7f;
            r.a -= Time.deltaTime * 0.7f;
            o.a -= Time.deltaTime * 0.7f;
            g.a -= Time.deltaTime * 0.7f;
            fanari.color = c;
            redLight.color = r;
            orangeLight.color = o;
            greenLight.color = g;
        }
        else if (startTimer - realTimeTimer >= 4.5f)
        {
            //PLAY MUSIC
            // ADD DINGS
            greenLight.color = new Color(0, 255, 0);
            //  orangeLight.color = new Color(199, 136, 0);
            Time.timeScale = 1;
        }
        else if (startTimer - realTimeTimer >= 3f)
        {
            orangeLight.color = new Color(255, 136, 0);
            //  redLight.color = new Color(123, 0, 0);
        }
        else if (startTimer - realTimeTimer >= 1.5f) 
        {
            redLight.color = new Color(255, 0, 0);
        }
    }
}
