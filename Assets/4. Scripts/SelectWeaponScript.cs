using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectWeaponScript : MonoBehaviour
{
    public float W_POWER, W_DAMAGE, W_RPM,W_NOISE;
    public Camera MainCamera;
    GameObject BG,w_stat;
    RaycastHit hitinfo, _lasthit;
    int w_Selected;
    
    public AudioClip gunreloadsound1,gunreloadsound2,gunreloadsound3,gunreloadsound4;

    public AudioSource audiosource;
    
    // Start is called before the first frame update
    void Start()
    {
        MainCamera = Camera.main;
        BG = GameObject.Find("GetReadyBG");
        w_stat = GameObject.Find("Weapon_stat");
        DontDestroyOnLoad(w_stat);
        w_Selected = -1;
        
        audiosource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hitinfo, 1000f))
        {
            for (int i = 0; i < 6; i++)
            {
                if (BG.transform.GetChild(i).transform == hitinfo.collider.transform)
                {
                    BG.transform.GetChild(i).GetComponent<SpriteRenderer>().color = Color.red;
                }
                else
                {
                    if (i != w_Selected)
                    {
                        BG.transform.GetChild(i).GetComponent<SpriteRenderer>().color = Color.white;
                    }
                }

            }
            //Debug.Log(hitinfo.collider.gameObject.name);
            _lasthit = hitinfo;
            if (Input.GetMouseButton(0))
            {
                if (hitinfo.collider.name == "BUT_undo")
                {
                    SceneManager.LoadScene(0);
                }
                else if (hitinfo.collider.name == "BUT_play")
                {
                    
                    SceneManager.LoadScene(2);
                }
                else if (hitinfo.collider.name == "IMG_weapon01")
                {
                    if (w_Selected != 0)
                    {
                        audiosource.PlayOneShot(gunreloadsound1,0.1f);
                    }
                    
                    W_DAMAGE =48f;
                    W_POWER =100f;
                    W_RPM =1f;
                    W_NOISE = 35f;
                    w_Selected = 0;
                    
                }
                else if (hitinfo.collider.name == "IMG_weapon02")
                {
                    if (w_Selected != 1)
                    {
                        audiosource.PlayOneShot(gunreloadsound2,0.1f);
                    }
                    W_DAMAGE = 18f;
                    W_POWER = 180f;
                    W_RPM = 0.25f;
                    W_NOISE = 20f;
                    w_Selected = 1;
                    
                }
                else if (hitinfo.collider.name == "IMG_weapon03")
                {
                    if (w_Selected != 2)
                    {
                        audiosource.PlayOneShot(gunreloadsound3,0.1f);
                    }
                    W_DAMAGE = 32f;
                    W_POWER = 75f;
                    W_RPM = 0.1f;
                    W_NOISE = 40f;
                    w_Selected = 2;
                }
                else if (hitinfo.collider.name == "IMG_weapon04")
                {
                    if (w_Selected != 3)
                    {
                        audiosource.PlayOneShot(gunreloadsound4,0.1f);
                    }
                    W_DAMAGE = 81f;
                    W_POWER = 250f;
                    W_RPM = 2f;
                    W_NOISE = 45f;
                    w_Selected = 3;
                }
                w_stat.GetComponent<W_statscript>().Save_W_stat(W_DAMAGE, W_POWER, W_RPM, W_NOISE,w_Selected);
            }
        }
        else
        {
            if (_lasthit.collider.tag == "Button")
            {
                _lasthit.collider.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }
}
