using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParentScript : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip enemydeath2;
    public AudioClip impactsound3;
    
    public void playdamagedsound(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(impactsound3,position,0.2f);
    }
    
    public void playdiesound(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(enemydeath2,position,0.2f);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
