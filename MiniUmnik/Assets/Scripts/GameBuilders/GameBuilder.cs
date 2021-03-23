using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBuilder : MonoBehaviour
{

    private GameType gameType;
    private LevelDesigner levelDesigner;
    public  void StartGame(GameType? gameType,int levels)
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
        for (int i = 0; i < levels; i++)
        {


            yield return levelDesigner.CreateLevel(i);
            yield return levelDesigner.WaitAnswer();
            yield return levelDesigner.RemoveLevel();
            Debug.Log("Следующий уровень");
        }
    }
    private void GetDesigner()
    {
        
        if(TryGetComponent(out levelDesigner))
        {
            Destroy(levelDesigner);
        }
        switch (gameType)
        {
            case GameType.Logic:levelDesigner = gameObject.AddComponent<LogicLevelDesigner>(); break;
            case GameType.Mathematic:levelDesigner = gameObject.AddComponent<MathematicLevelDesigner>(); break;
            case GameType.Memory:levelDesigner = gameObject.AddComponent<MemoryLevelDesigner>(); break;
            case GameType.Attention:levelDesigner = gameObject.AddComponent<AttentionLevelDesigner>(); break;

            default: Debug.LogError("Не найден дезайнер уровней");break;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    
}
