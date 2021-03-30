using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBuilder : MonoBehaviour
{
    private GameMenu GameMenu { get; set; }
    private GameType gameType;
    private LevelDesigner levelDesigner;

    private void Start()
    {
        GameMenu = UIController.GetMenu<GameMenu>();
    }
    public void StartGame(GameType? gameType, int levels)
    {

        if (gameType == null)
        {
            this.gameType = (GameType)UnityEngine.Random.Range(0, 4);
        }
        else
        {

            this.gameType = gameType.Value;
        }

        GetDesigner();
        StartCoroutine(StartGameCoroutine(levels));


    }
    private IEnumerator StartGameCoroutine(int levels)
    {
        StatisticsMaker.Reset();
        UIController.GetMenu<GameMenu>().question.text = "";
        for (int i = 0; i < levels; i++)
        {
           
            yield return levelDesigner.CreateLevel(i);
            yield return levelDesigner.WaitAnswer();
            yield return levelDesigner.RemoveLevel();
        }
        GameMenu.ShowStat();
    }
    private void Update()
    {
        if (UIController.GetMenu<StatisticsMenu>().IsOpen)
        {
            if (levelDesigner != null)
            {
                StopAllCoroutines();
                levelDesigner.StopAllCoroutines();
                StartCoroutine(levelDesigner.RemoveLevel());
                Destroy(levelDesigner);
            }
        }
    }

    private void GetDesigner()
    {

        if (TryGetComponent(out levelDesigner))
        {
            levelDesigner.OnTimerTick -= UIController.GetMenu<GameMenu>().ShowTimer;
            Destroy(levelDesigner);
        }
        switch (gameType)
        {
            case GameType.Logic: levelDesigner = gameObject.AddComponent<LogicLevelDesigner>(); break;
            case GameType.Mathematic: levelDesigner = gameObject.AddComponent<MathematicLevelDesigner>(); break;
            case GameType.Memory: levelDesigner = gameObject.AddComponent<MemoryLevelDesigner>(); break;
            case GameType.Attention: levelDesigner = gameObject.AddComponent<AttentionLevelDesigner>(); break;

            default: Debug.LogError("Не найден дезайнер уровней"); break;
        }
        levelDesigner.OnTimerTick += UIController.GetMenu<GameMenu>().ShowTimer;
    }





}
