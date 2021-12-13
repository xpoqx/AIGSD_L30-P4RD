using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField] public GameObject player,ReloadingUI,MissionUI,DoorWall,PopupUI,GOUI;
    [SerializeField] private Text ammoText,hpText; 
    [SerializeField] private Image hpBar, weaponImage, reloadimage;
    [SerializeField] private Sprite weapon1, weapon2, weapon3, weapon4;

    private int ammo,ammo_inmagazine,alpha;
    private _4._Scripts.Player playerscript;
    private float myhp,reloadtime,popupwidth,popupheight;
    private bool isreloading;
    public int mycrystal,missionNumber;
    
    

    public void RefreshMission()
    {
        if (missionNumber == 1)
        {
            PopupStart();
            MissionUI.transform.GetChild(2).GetComponent<Text>().text = mycrystal + " / 5";
            if (mycrystal == 5)
            {
                DoorWall.GetComponent<DoorWallScript>().OpenWall();
                missionNumber = 2;
                Invoke("RefreshMission", 2f);
            }
        }
        else if(missionNumber == 2)
        {
            MissionUI.transform.GetChild(1).GetComponent<Text>().text = "열린 돌 문을 찾아라";
            MissionUI.transform.GetChild(2).GetComponent<Text>().text = "?? / ??";
        }
        else if (missionNumber == 3)
        {
            PopupStart();
            MissionUI.transform.GetChild(1).GetComponent<Text>().text = "신전에 들어가보자";
            MissionUI.transform.GetChild(2).GetComponent<Text>().text = "?? / ??";
        }
        else if (missionNumber == 4)
        {
            PopupStart();
            MissionUI.transform.GetChild(1).GetComponent<Text>().text = "살아남아라";
            MissionUI.transform.GetChild(2).GetComponent<Text>().text = "?? / ??";
        }
    }

    void PopupRefresh()
    {
        if (missionNumber == 1)
        {
            if (mycrystal < 5)
            {
                PopupUI.transform.GetChild(2).GetComponent<Text>().text = "반짝이는 보석을 획득했다";
            }
            else if (mycrystal==5)
            {
                PopupUI.transform.GetChild(2).GetComponent<Text>().text = "북쪽에서 마찰음이 들린다";
            }
        }
        else if (missionNumber == 3)
        {
            PopupUI.transform.GetChild(2).GetComponent<Text>().text = "앞쪽에 커다란 건물이 보인다";
        }
        else if (missionNumber == 4)
        {
            PopupUI.transform.GetChild(2).GetComponent<Text>().text = "사방에서 적이 쏟아진다!!";
        }
        
    }
    
    void PopupStart()
    {
        PopupRefresh();
        popupwidth = 0;
        popupheight = 0;
        PopupUI.transform.GetChild(1).GetComponent<Image>().rectTransform.localScale = (new Vector3((-1/(popupwidth+1))+1, 1, 1));
        PopupUI.transform.GetChild(0).GetComponent<Image>().rectTransform.localScale = (new Vector3(1, (-1/(popupheight+1))+1, 1));
        InvokeRepeating("PopupAnimation",0,0.0025f);
        PopupUI.SetActive(true);
        // Invoke("PopupEnd",2f);
    }

    void PopupEnd()
    {
        PopupUI.transform.GetChild(2).gameObject.SetActive(false);
        PopupUI.SetActive(false);
        popupwidth = 0;
        popupheight = 0;
    }

    void PopupAnimation()
    {
        
        if (popupwidth < 20)
        {
            PopupUI.transform.GetChild(1).GetComponent<Image>().rectTransform.localScale = (new Vector3((-1/(popupwidth+1))+1, 1, 1));
            popupwidth += 0.1f;
        }
        else
        {
            if (popupheight < 20)
            {
                PopupUI.transform.GetChild(0).GetComponent<Image>().rectTransform.localScale = (new Vector3(1, (-1/(popupheight+1))+1, 1));
                popupheight += 0.1f;
            }
            else
            {
                CancelInvoke("PopupAnimation");
                PopupUI.transform.GetChild(2).gameObject.SetActive(true);
                Invoke("PopupEnd",2f);
            }
        }
    }

    public void GameOver()
    {
        alpha = 0;
        GOUI.SetActive(true);
        GOUI.transform.GetChild(3).gameObject.SetActive(false);
        InvokeRepeating("RefreshGameoverUI",0,0.005f);
        //RefreshGameoverUI();
    }

    void RefreshGameoverUI()
    {
        GOUI.transform.GetChild(0).GetComponent<Image>().color=new Color(0,0,0,alpha/255f*150/255f);
        GOUI.transform.GetChild(1).GetComponent<Image>().color=new Color(48/255f,0,0,alpha/255f*200/255f);
        GOUI.transform.GetChild(2).GetComponent<Text>().color=new Color(1,88/255f,88/255f,alpha/255f);
        alpha++;
        if (alpha == 255)
        {
            CancelInvoke("RefreshGameoverUI");
            InvokeRepeating("GameoverTexton",0.5f,1f);
            InvokeRepeating("GameoverTextoff",1f,1f);
        }
    }

    void GameoverTextoff()
    {
        GOUI.transform.GetChild(3).gameObject.SetActive(false);
    }
    void GameoverTexton()
    {
        GOUI.transform.GetChild(3).gameObject.SetActive(true);
    }
    void Start()
    {
        
        mycrystal = 0;
        ReloadingUI.SetActive(false);
        GOUI.SetActive(false);
        isreloading = false;
        playerscript=player.GetComponent<_4._Scripts.Player>();
        int weaponNumber = playerscript.selectedweapon;
        if (weaponNumber == 0)
        {
            weaponImage.sprite = weapon1;
        }
        else if (weaponNumber == 1)
        {
            weaponImage.sprite = weapon2;
        }
        else if (weaponNumber == 2)
        {
            weaponImage.sprite = weapon3;
        }
        else if (weaponNumber == 3)
        {
            weaponImage.sprite = weapon4;
        }

        MissionUI.transform.GetChild(1).GetComponent<Text>().text = "빛나는 보석을 찾아라";
        MissionUI.transform.GetChild(2).GetComponent<Text>().text = mycrystal + " / 5";
        missionNumber = 1;
        PopupEnd();
    }
    
    // Update is called once per frame
    void Update()
    {
        ammo=playerscript.ammo;
        ammo_inmagazine = playerscript.ammo_magazine;
        myhp = playerscript.hp;
        ammoText.text = ammo_inmagazine + "/" + ammo;
        hpText.text = myhp + "/100"; 
        hpBar.rectTransform.localScale = new Vector3 (myhp / 100,1,1);
        if (isreloading)
        {
            float delaytime = playerscript.delaytime;
            reloadimage.rectTransform.localScale = new Vector3((reloadtime - delaytime) / reloadtime,1,1);
            if (delaytime < 0)
            {
                ReloadingUI.SetActive(false);
                isreloading = false;
            }
        }
    }

    public void StartReloading()
    {
        reloadtime=playerscript.RELOADTIME;
        ReloadingUI.SetActive(true);
        isreloading = true;
    }
}
