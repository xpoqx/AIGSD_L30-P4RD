using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPscript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, Time.deltaTime*5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
