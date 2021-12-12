using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField] public GameObject player,ReloadingUI;
    [SerializeField] private Text ammoText,hpText; 
    [SerializeField] private Image hpBar, weaponImage, reloadimage;
    [SerializeField] private Sprite weapon1, weapon2, weapon3, weapon4;

    private int ammo,ammo_inmagazine;
    private _4._Scripts.Player playerscript;
    private float myhp,reloadtime;
    private bool isreloading;
    void Start()
    {
        ReloadingUI.SetActive(false);
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
            reloadimage.rectTransform.localScale = new Vector3(delaytime / reloadtime,1,1);
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
