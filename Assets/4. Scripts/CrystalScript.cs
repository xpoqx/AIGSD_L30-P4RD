using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalScript : MonoBehaviour
{
    public AudioClip crystalsound;
    public AudioSource audiosource;
    [SerializeField] public bool dontplaysound;
    
    // Start is called before the first frame update
    void Start()
    {
        audiosource = gameObject.GetComponent<AudioSource>();
        if (!dontplaysound)
        {
            audiosource.PlayOneShot(crystalsound,0.5f);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            var playerscript = other.GetComponent<_4._Scripts.Player>();
            playerscript.GetCrystal();
            Destroy(gameObject);
        }
    }
}
