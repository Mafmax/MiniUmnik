using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StatisticsMenu : Menu
{
    private Dictionary<int, StatisticsSlot> Slots = new Dictionary<int, StatisticsSlot>();
    private Button ContinueButton { get; set; }
    private Button BreakButton { get; set; }
    private Text FinallyResult { get; set; }
    // Start is called before the first frame update
    void Awake()
    {
        var slots = GetComponentsInChildren<StatisticsSlot>();
        for (int level = 1; level < 11; level++)
        {
            Slots.Add(level, slots.Where(x => x.name.Equals($"Level ({level})")).FirstOrDefault());
        }
        FinallyResult = GetComponentsInChildren<Text>().Where(x => x.name.Equals("FinallyResult")).FirstOrDefault();
        var buttons = GetComponentsInChildren<Button>();
        ContinueButton = buttons.Where(x => x.name.Equals("Repeat", System.StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        BreakButton = buttons.Where(x => x.name.Equals("Break", System.StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        ContinueButton.onClick.AddListener(OnContinueButton);
        BreakButton.onClick.AddListener(OnBreakButton);
    }

    private void OnBreakButton()
    {
        UIController.GetMenu<StartMenu>().OpenMenu();
    }

    private void OnContinueButton()
    {
        UIController.GetMenu<StartMenu>().RepeatGame();
    }

    private void Show()
    {
        int points = StatisticsMaker.GetStatistics().Count((stat) => stat.IsCompleted);
        FinallyResult.color = points < 8 ? Color.red : Color.green;
        FinallyResult.text = $"Баллов набрано: {points}!";
        foreach (var slot in Slots.Values)
        {
            slot.Show(null);
        }

        foreach (var stat in StatisticsMaker.GetStatistics())
        {
            Slots[stat.Level].Show(stat);
        }
    }
    public override void OpenMenu(bool closeOthers = true)
    {
        base.OpenMenu(closeOthers);
        Show();
    }
    // Update is called once per frame
    void Update()
    {

    }
}

