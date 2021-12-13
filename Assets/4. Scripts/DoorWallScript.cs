using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorWallScript : MonoBehaviour
{
    private AudioSource _audioSource;

    [SerializeField]
    private AudioClip stonesound;
    // Start is called before the first frame update

    private bool _doorisopen,_doorisclosed;
    
    public void OpenWall()
    {
        if (_doorisclosed)
        {
            _audioSource.PlayOneShot(stonesound,0.1f);
            Invoke("StopWall",1.5f);
            _doorisclosed = false;
            OpenWall();
        }
        if (!_doorisclosed)
        {
            if (!_doorisopen)
            {
                transform.Translate(Vector3.right*0.006f);
                Invoke("OpenWall",0.01f);
            }
        }
        
        
    }
    
    void StopWall()
    {
        _doorisopen = true;
    }
    
    void Start()
    {
        _doorisopen = false;
        _doorisclosed = true;
        _audioSource = gameObject.GetComponent<AudioSource>();
        //Invoke("OpenWall", 1f);
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
            playerscript.NearTemple();
            Destroy(gameObject.GetComponent<BoxCollider>());
        }
    }
}
