using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainShip : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate((Vector3.up+Vector3.right)*Time.deltaTime/100f);
        transform.Translate((Vector3.right)*Time.deltaTime/40f);
    }
}
