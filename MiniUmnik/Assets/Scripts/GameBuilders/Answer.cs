using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Answer : MonoBehaviour
{
    public bool IsCorrect = false;
    private event Action<Answer> OnChoice;
    protected Transform answer;
    protected Transform player;

    public  void Create(Action<Answer> answerCallback)
    {
        OnChoice += answerCallback;
    }

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        player = FindObjectOfType<PlayerController>().transform;
        answer = transform;
    }
    // Update is called once per frame

    
    private void OnCollisionStay(Collision collision)
    {
        if(collision.transform.TryGetComponent<Bullet>(out var plug))
        {
            SendAnswer();
            Destroy(collision.gameObject);
        }
    }
    private void SendAnswer()
    {
        OnChoice.Invoke(this);
        
    }
}
