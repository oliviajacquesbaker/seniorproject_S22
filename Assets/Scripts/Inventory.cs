using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public List<MenuButton> buttons = new List<MenuButton>();
    public float timeSlowRatio;
    private Vector2 mousePos;
    private Vector2 from = new Vector2(0.5f, 1.0f);
    private Vector2 center = new Vector2(0.5f, 0.5f);
    private Vector2 to;
    public KeyCode hotkey;
    public int numItems = 1;
    public int currMenuItem;
    private int prevMenuItem;
    private int selectedWeapon;
    private float initialDeltaTimeScale;
    public GameObject inv;
    private GameObject currWeapon;
    private GameObject bow;
    private GameObject sword;
    private GameObject weapons;
    private GameObject blur;
    private StateHandler state;

    void Start()
    {
        bow = GameObject.Find("Bow");
        sword = GameObject.Find("Sword");
        inv = GameObject.Find("Inventory");
        state = GameObject.Find("Main Camera").GetComponent<StateHandler>();
        blur = GameObject.Find("Background Blur");
        InitInventory();
    }

    void Update()
    {
        GetCurrMenuItem();
        //Debug.Log(IsOpen());

        if (Input.GetKeyDown(hotkey))
        {
            ShowInventory();
        }

        if (Input.GetKeyUp(hotkey))
        {
            CloseInventory();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            GiveItem();
        }
    }

    void InitWeapons()
    {
        inv.SetActive(false);
        bow.SetActive(false);
        sword.SetActive(false);
    }

    void InitInventory()
    {
        InitWeapons();
        
        foreach(MenuButton button in buttons)
        {
            button.sceneImage.color = button.normal;
        }

        currWeapon = bow;
        numItems = buttons.Count;
        currMenuItem = 0;
        prevMenuItem = 0;
        blur.SetActive(false);
    }

    public void ShowInventory()
    {
        inv.SetActive(true);
        blur.SetActive(true);
        Time.timeScale = timeSlowRatio;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseInventory()
    {
        inv.SetActive(false);
        blur.SetActive(false);
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void GetCurrMenuItem()
    {
        mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        to = new Vector2(mousePos.x / Screen.width, mousePos.y / Screen.height);
        float angle = (Mathf.Atan2(from.y - center.y, from.x - center.x) - Mathf.Atan2(to.y - center.y, to.x - center.x)) * Mathf.Rad2Deg;

        if (angle < 0)
        {
            angle += 360;
        }

        //Debug.Log(numItems);
        if (numItems > 0)
            currMenuItem = (int) (angle / (360 / numItems));

        if (currMenuItem != prevMenuItem)
        {
            buttons[prevMenuItem].sceneImage.color = buttons[prevMenuItem].normal;
            prevMenuItem = currMenuItem;
            buttons[currMenuItem].sceneImage.color = buttons[currMenuItem].highlighted;
        }

    }

    void GiveItem()
    {

        if (!IsOpen()) {return;}

        buttons[currMenuItem].sceneImage.color = buttons[currMenuItem].pressed;

        if (currMenuItem == 0) // bow
        {
            Debug.Log("You have been given a bow!");
            // give player weapon
            //sword.SetActive(false);
            currWeapon.SetActive(false);
            bow.SetActive(true);
            currWeapon = bow;
        }
        else if (currMenuItem == 1) // sword
        {
            Debug.Log("You have been given a sword!");
            //bow.SetActive(false);
            currWeapon.SetActive(false);
            sword.SetActive(true);
            currWeapon = sword;
        }
        // repeat for all other weapons

    }

    public bool IsOpen()
    {
        return GameObject.Find("Inventory");
    }

}

[System.Serializable]
public class MenuButton
{
    public string name;
    public Image sceneImage;
    public Color normal = Color.white;
    public Color highlighted = Color.grey;
    public Color pressed = Color.gray;
}
