using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChoiceMenu : Menu
{
    public class ChoicePosition
    {
        public Sprite image;
        public string name;
        public string CreateTooltipText()
        {
            switch (type)
            {
                case null: return "Тематика игры выбирается случайным образом";
                case GameType.Attention: return "Задания на развитие внимания";
                case GameType.Mathematic: return "Задания на развитие математических способностей";
                case GameType.Logic: return "Задания на развитие логического мышления";
                case GameType.Memory: return "Задания на тренеровку памяти";
                default: return "Описание типа заданий не добавлено";
            }

        }
        public GameType? type;
        
    }
    public ChoicePosition choice;
    private List<ChoicePosition> positions = new List<ChoicePosition>();
    private List<GameObject> fields = new List<GameObject>();
    private GameObject exampleField;
    private Transform fieldsParent;
    // Start is called before the first frame update
    void Awake()
    {
        exampleField = Resources.Load<GameObject>("Prefabs/UI/ChoiceField");
        fieldsParent = GetComponentsInChildren<Transform>().Where(x => x.name == "Fields").FirstOrDefault();
    }
    private static Sprite LoadLabelSprite(string mainTextureName)
    {
        var allSprites = Resources.LoadAll<Sprite>("Images/UI/Game type labels/" + mainTextureName);
        if (allSprites != null && allSprites.Length > 0)
        {
            return allSprites[0];
        }
        else
        {
            return null;
        }
    }
    public override void OpenMenu(bool closeOthers = false)
    {

        base.OpenMenu(closeOthers);
        if (positions == null || positions.Count == 0)
        {
            InitPositions();
        }
        if (fields == null || fields.Count == 0)
        {
            ConfigureMenu();
        }

    }
    private void InitPositions()
    {
        positions.Add(new ChoicePosition
        {
            image = LoadLabelSprite("random"),
            name = "Случайно",
            type = null
        });
        positions.Add(new ChoicePosition
        {
            image = LoadLabelSprite("attention"),
            name = "Внимание",
            type = GameType.Attention
        });
        positions.Add(new ChoicePosition
        {
            image = LoadLabelSprite("mathematic"),
            name = "Математика",
            type = GameType.Mathematic
        });
        positions.Add(new ChoicePosition
        {
            image = LoadLabelSprite("memory"),
            name = "Память",
            type = GameType.Memory
        });
        positions.Add(new ChoicePosition
        {
            image = LoadLabelSprite("logic"),
            name = "Логика",
            type = GameType.Logic
        });

    }
    private void ConfigureMenu()
    {
        var count = positions.Count();
        var parentRect = fieldsParent.GetComponent<RectTransform>();
        var fieldHeight = parentRect.rect.height / count;

        for (int i = 0; i < count; i++)
        {
            var field = Instantiate(exampleField, fieldsParent);

            var fieldRect = field.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -parentRect.rect.height / 2 + fieldHeight * (i + 0.5f));
            ConstructField(field, positions[i]);
            fields.Add(field);

        }


    }
    private void ConstructField(GameObject field, ChoicePosition choicePositionData)
    {
        var childs = field.GetComponentsInChildren<Transform>(true);

        childs.Where(x => x.name == "Type").FirstOrDefault().gameObject.GetComponent<Text>().text = choicePositionData.name;
        childs.Where(x => x.name == "Label").FirstOrDefault().gameObject.GetComponent<Image>().sprite = choicePositionData.image;
        var btn = childs.Where(x => x.name == "Choice").FirstOrDefault().gameObject.GetComponent<Button>();
        btn.name = choicePositionData.type.ToString();
        btn.onClick.AddListener(OnChoice);
    }

    private void OnChoice()
    {
        choice = positions.Where(x => x.type.ToString() == EventSystem.current.currentSelectedGameObject.name).FirstOrDefault();

        var menu = UIController.GetMenu<StartMenu>();
        menu.gameType = choice.type;
        menu.OpenMenu(true);
    }
}
