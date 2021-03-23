using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{

    public bool IsOpen => gameObject.activeSelf;
    public virtual void CloseMenu(bool closeOther = true)
    {
        if (closeOther)
        {
            UIController.CloseMenus();
        }
        else
        {

            gameObject.SetActive(false);
        }
    }
    public virtual void OpenMenu(bool closeOthers = true)
    {
        if (closeOthers)
        {
            UIController.CloseMenus();
        }
        gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
