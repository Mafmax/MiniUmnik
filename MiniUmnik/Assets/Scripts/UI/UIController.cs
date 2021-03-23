using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private static Dictionary<string, Component> menuComponents = new Dictionary<string, Component>();
    private static Dictionary<string, Menu> menus = new Dictionary<string, Menu>();
    private Dictionary<string, Type> names = new Dictionary<string, Type>();
    public static UIController Controller { get; private set; }
    public static T GetMenu<T>() where T:Menu
    {
        var menu = menus.Values.Where(x => x.GetType() == typeof(T)).FirstOrDefault();
        if (menu == null)
        {
            Debug.LogError($"Меню {typeof(T)} отсутствует в словаре");
        }
        return (T)menu;
    }

    // Start is called before the first frame update
    private void InitiateNames()
    {
        Controller = this;
        names.Add("Start", typeof(StartMenu));
        names.Add("Game", typeof(GameMenu));
        names.Add("Choice", typeof(ChoiceMenu));

    }
    void Start()
    {
        var childs = GetComponentsInChildren<Transform>(true);
        InitiateNames();
        foreach (var name in names.Keys)
        {
            var menuTransform = childs.Where(x => x.name == name + "Menu").FirstOrDefault();

            if (menuTransform == null)
            {
                Debug.LogError($"Меню \"{name}\" не найдено");
                continue;
            }

            var menu = menuTransform.gameObject.AddComponent(names[name]);
            menuComponents.Add(name, menu);
            menus.Add(name, menu as Menu);
        }

        GetMenu<StartMenu>().OpenMenu(true);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    public static void CloseMenus()
    {

        foreach (var menu in menus.Values)
        {


            menu.CloseMenu(false);

        }
    }
}
