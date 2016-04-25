using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleController : MonoBehaviour
{

    
    private List<KeyValuePair<GameObject, float>> hitCoolDowns;

    // Use this for initialization
    void Start()
    {
        hitCoolDowns = new List<KeyValuePair<GameObject, float>>();
    }
    
    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < hitCoolDowns.Count; i++)
        {
            if (hitCoolDowns[i].Value + 1.0f < Time.time)
                hitCoolDowns.RemoveAt(i--);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        var colCC = col.gameObject.transform.root.GetComponentInChildren<CarController>();
        if(colCC != null)
        {
            foreach (var existingCD in hitCoolDowns)
                if (existingCD.Key == colCC.gameObject)
                    return;
            Debug.Log("Col btwn " + colCC.gameObject.name + " & " + gameObject.name);
            hitCoolDowns.Add(new KeyValuePair<GameObject, float>(colCC.gameObject, Time.time));

            if(transform.name.Contains("Water"))
            {
                //Here code for effect of water
            }
            else
            {
                colCC.ReduceSpeed();
            }
        }
    }
}
