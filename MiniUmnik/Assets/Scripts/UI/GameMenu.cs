using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : Menu
{
    private StartMenu startMenu;
    private PlayerController player;

    [HideInInspector]
    public Text question;
    private Text timer;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        startMenu = UIController.GetMenu<StartMenu>();
    }
    private void Awake()
    {
        question = GetComponentsInChildren<Text>().Where(x => x.name == "Question").FirstOrDefault();
        timer = GetComponentsInChildren<Text>().Where(x => x.name == "Timer").FirstOrDefault();

    }
    public void ShowStat()
    {

        UIController.GetMenu<StatisticsMenu>().OpenMenu(true);
    }
    public void ShowTimer(float time)
    {
        timer.text = ((int)time).ToString() + "сек";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowStat();
        }
        if (Input.GetMouseButtonDown(0))
        {
            player.ShootLogic();
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        player.MoveLogic();
        player.CameraLogic();

      

    }
    public override void OpenMenu(bool closeOthers = true)
    {
        base.OpenMenu(closeOthers);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public override void CloseMenu(bool closeOther = true)
    {
        base.CloseMenu(closeOther);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
