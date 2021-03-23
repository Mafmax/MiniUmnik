using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelDesigner : MonoBehaviour
{
    protected int currentLevel;
    protected PlatformController platform;
    protected List<Answer> answers = new List<Answer>();
    protected bool answerRecieved = false;
    protected Transform answerAnchor;
    protected Answer answerExample;
    protected Text question;
    protected virtual void Awake()
    {
        answerExample = Resources.Load<Answer>("Prefabs/Answer");
        platform = FindObjectOfType<PlatformController>();
        answerAnchor = GameObject.Find("Answers").transform;
        question = UIController.GetMenu<GameMenu>().question;
        
    }
    public virtual IEnumerator CreateLevel(int level)
    {
        Debug.Log($"Старт уровня (заглушка)");
        yield return new WaitForSeconds(4);

        yield break;
    }
    public virtual IEnumerator RemoveLevel()
    {

        Debug.Log($"Удаление уровня (заглушка)");
        yield break;
    }
    public virtual IEnumerator WaitAnswer()
    {
        while (!answerRecieved)
        {

            yield return new WaitForSeconds(0.5f);
        }

        yield break;
    }
}
