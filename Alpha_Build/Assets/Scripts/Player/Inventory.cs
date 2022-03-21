using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public List<MenuButton> buttons = new List<MenuButton>();
    public List<GameObject> thumbnails = new List<GameObject>();
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
    public bool debug;
    public GameObject inv;
    private GameObject currWeapon;
    private GameObject bow;
    private GameObject sword;
    private GameObject rope;
    private GameObject weapons;
    private GameObject blur;
    private GameObject shield;
    private StateHandler state;

    private bool bowEnabled, swordEnabled, ropeEnabled, shieldEnabled;

    private _PlayerStats playerStats;
    private _PlayerStatsController playerStatsController;

    void Start()
    {
        currMenuItem = -1;
        bow = GameObject.Find("Bow");
        sword = GameObject.Find("Sword");
        rope = GameObject.Find("RopeItem");
        inv = GameObject.Find("Inventory");
        state = Camera.main.GetComponent<StateHandler>();
        blur = GameObject.Find("Background Blur");
        shield = GameObject.Find("Shield");
        InitInventory();

        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<_PlayerStats>();
        playerStatsController = GameObject.Find("Player").GetComponent<_PlayerStatsController>();
    }

    void Update()
    {
        GetCurrMenuItem();
        //Debug.Log(IsOpen());

        if (shield.activeInHierarchy)
        {
            playerStatsController.SetPlayerImmune(true);
        }
        else
        {
            playerStatsController.SetPlayerImmune(false);
        }

        if (Input.GetKeyDown(hotkey))
        {
            ShowInventory();
        }

        if (Input.GetKeyUp(hotkey))
        {
            if(currMenuItem != -1) GiveItem();
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
        rope.SetActive(false);
        shield.SetActive(false);
    }

    void InitInventory()
    {
        InitWeapons();
        
        foreach(MenuButton button in buttons)
        {
            button.sceneImage.color = button.disabled;
        }

        foreach (GameObject thumbnail in thumbnails)
        {
            thumbnail.SetActive(false);
        }

        currWeapon = bow;
        numItems = buttons.Count;
        currMenuItem = 0;
        prevMenuItem = 0;
        blur.SetActive(false);
        if (!debug)
        {
            bowEnabled = swordEnabled = ropeEnabled = shieldEnabled = false;
        }
        else
        {
            bowEnabled = swordEnabled = ropeEnabled = shieldEnabled = true;
        }
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
            if(GetIntBasedEnabledStatus(prevMenuItem)) buttons[prevMenuItem].sceneImage.color = buttons[prevMenuItem].normal;
            else buttons[prevMenuItem].sceneImage.color = buttons[prevMenuItem].disabled;
            prevMenuItem = currMenuItem;
            if (GetIntBasedEnabledStatus(currMenuItem)) buttons[currMenuItem].sceneImage.color = buttons[currMenuItem].highlighted;
            else buttons[currMenuItem].sceneImage.color = buttons[currMenuItem].disabledHighlight;
        }

    }

    void GiveItem()
    {

        if (!IsOpen()) return;

        buttons[currMenuItem].sceneImage.color = buttons[currMenuItem].pressed;

        if (currMenuItem == 0 && bowEnabled) // bow
        {
            Debug.Log("You have been given a bow!");
            GiveBow();
            playerStats.SetActiveTool("bow");

        }
        else if (currMenuItem == 1 && swordEnabled) // sword
        {
            Debug.Log("You have been given a sword!");
            GiveSword();
            playerStats.SetActiveTool("sword");

        }
        else if (currMenuItem == 2 && ropeEnabled) // rope
        {
            Debug.Log("You have been given a rope!");
            GiveRope();
            playerStats.SetActiveTool("rope");
        }
        else if (currMenuItem == 3 && shieldEnabled) // shield
        {
            if (shield.activeInHierarchy)
            {
                shield.SetActive(false);
                //shieldEnabled = false;
            }
            else
            {
                Debug.Log("You have been given a shield!");
                GiveShield();
                playerStats.SetActiveTool("shield");
            }
        }
        // repeat for all other weapons

    }

    public void EnableItem(ShadowType item)
    {
        Debug.Log(item + "Collected and enabled!");
        int updated = -1;
        if (item == ShadowType.sword) { swordEnabled = true; updated = 1; }
        else if (item == ShadowType.shield) { shieldEnabled = true; updated = 3; }
        else if (item == ShadowType.rope) { ropeEnabled = true; updated = 2; }
        else if (item == ShadowType.bow) { bowEnabled = true; updated = 0; }

        if (updated != -1)
        {
            buttons[updated].sceneImage.color = buttons[updated].normal;
            thumbnails[updated].SetActive(true);
        }
    }

    public bool IsOpen()
    {
        return GameObject.Find("Inventory");
    }

    public void GiveBow()
    {
        currWeapon.SetActive(false);
        bow.SetActive(true);
        currWeapon = bow;
    }

    public void GiveSword()
    {
        currWeapon.SetActive(false);
        sword.SetActive(true);
        currWeapon = sword;
    }

    public void GiveRope()
    {
        currWeapon.SetActive(false);
        rope.SetActive(true);
        currWeapon = rope;
    }
    public void GiveShield()
    {
        //currWeapon.SetActive(false);
        shield.SetActive(true);
        shieldEnabled = true;
        //currWeapon = rope;
    }

    private bool GetIntBasedEnabledStatus(int item)
    {
        if (item == 0) return bowEnabled;
        else if (item == 1) return swordEnabled;
        else if (item == 2) return ropeEnabled;
        else return shieldEnabled;
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
    public Color disabled = new Color(0.2f, 0.2f, 0.2f);
    public Color disabledHighlight = new Color(0.25f, 0.25f, 0.25f);
}
