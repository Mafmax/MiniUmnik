using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : Menu
{
    private Button startButton, exitButton, choiceButton;
    public GameType? gameType = null;
    private ChoiceMenu choiceMenu;
    private GameMenu gameMenu;

    private GameBuilder gameBuilder;
    // Start is called before the first frame update
    void Awake()
    {
        var buttons = GetComponentsInChildren<Button>();
        startButton = buttons.Where(x => x.name == "StartButton").FirstOrDefault();
        exitButton = buttons.Where(x => x.name == "ExitButton").FirstOrDefault();
        choiceButton = buttons.Where(x => x.name == "ChoiceButton").FirstOrDefault();
        choiceButton.onClick.AddListener(OnChoiceButtonClick);
        startButton.onClick.AddListener(OnStartButtonClick);
        exitButton.onClick.AddListener(OnExitButtonClick);
        gameBuilder = FindObjectOfType<GameBuilder>();
    }
    private void Start()
    {
        choiceMenu = UIController.GetMenu<ChoiceMenu>();
        gameMenu= UIController.GetMenu<GameMenu>();

    }

    private void OnExitButtonClick()
    {
        Application.Quit();
    }

    private void OnStartButtonClick()
    {
        gameMenu.OpenMenu(true);
        Debug.LogWarning("Поменять");
        gameBuilder.StartGame(GameType.Attention, 10);
    }

    private void OnChoiceButtonClick()
    {
        choiceMenu.OpenMenu(false);
    }
    // Update is called once per frame
    void Update()
    {

    }
    public override void OpenMenu(bool closeOthers = true)
    {
        base.OpenMenu(closeOthers);

        string type = "Случайно";
        switch (gameType)
        {
            case GameType.Attention: type = "Внимание"; break;
            case GameType.Mathematic: type = "Математика"; break;
            case GameType.Logic: type = "Логика"; break;
            case GameType.Memory: type = "Память"; break;
        }
        choiceButton.transform.GetChild(0).GetComponent<Text>().text = type;
    }
}
