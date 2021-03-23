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
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        startMenu = UIController.GetMenu<StartMenu>();
    }
    private void Awake()
    {
        question = GetComponentsInChildren<Text>().Where(x => x.name == "Question").FirstOrDefault();

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
    }
    public override void CloseMenu(bool closeOther = true)
    {
        base.CloseMenu(closeOther);
        Cursor.lockState = CursorLockMode.None;
    }
}
