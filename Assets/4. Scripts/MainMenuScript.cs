using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public Camera MainCamera;
    GameObject BG;
    RaycastHit hitinfo,_lasthit;

    [SerializeField] private Texture2D cursorimg;
    
    private Vector2 _hotspot;
    
    // Start is called before the first frame update
    void Start()
    {
        MainCamera = Camera.main;
        BG = GameObject.Find("MainMenuBG");
        _hotspot.x = cursorimg.width / 2;
        _hotspot.y = cursorimg.height / 2;
        Cursor.SetCursor(cursorimg,_hotspot, CursorMode.ForceSoftware);
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);



        if (Physics.Raycast(ray, out hitinfo, 1000f))
        {
            for (int i = 0; i < 4; i++)
            {
                if (BG.transform.GetChild(i).transform == hitinfo.collider.transform)
                {
                    BG.transform.GetChild(i).GetComponent<SpriteRenderer>().color = Color.red;
                }
                else
                {
                    BG.transform.GetChild(i).GetComponent<SpriteRenderer>().color = Color.white;
                }

            }
            //Debug.Log(hitinfo.collider.gameObject.name);
            _lasthit = hitinfo;
            if (Input.GetMouseButton(0))
            {
                if (hitinfo.collider.name == "BUT_singleplay")
                {
                    SceneManager.LoadScene(1);
                }
                else if (hitinfo.collider.name == "BUT_quit")
                {
                    UnityEditor.EditorApplication.isPlaying = false;
                    Application.Quit();
                }
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
