using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_statscript : MonoBehaviour
{
    public float w_damage, w_power, w_rpm, w_noise;

    
    
    public int selected;
    // Start is called before the first frame update
    void Start()
    {
        w_damage = 0;
        w_power = 0;
        w_rpm = 0;
        w_noise = 0;
    }

    public void Save_W_stat(float dam, float pow, float rpm,float noi,int w_select)
    {
        w_damage = dam;
        w_power = pow;
        w_rpm = rpm;
        w_noise = noi;
        selected = w_select;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
