using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Assets.Resources.Scripts.Entity;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class InterfaceController : MonoBehaviour
{

    private static InterfaceController _instance;

    public GameObject InfoPanel;
    public GameObject ExtendedInfoPanel;

    public static void SetInfo(string stat, float value)
    {
        foreach (Transform obj in _instance.InfoPanel.transform)
        {
           // Debug.Log("SetStat:" + obj.name);
            if (obj.name == stat)
            {
                obj.GetComponent<Text>().text = Convert.ToString(value, CultureInfo.InvariantCulture);
                return;
            }
        }
    }

    public static void SetFullInfo(Character chr)
    {
        SetStats(chr);
        SetAP(chr);
        SetSkills(chr);
    }
    public static void SetStats(Character chr)
    {
        SetInfo("StrText", chr.Strength);
        SetInfo("AgiText", chr.Agility);
        SetInfo("DexText", chr.Dexterity);
        SetInfo("CntText", chr.Constitution);
        SetInfo("IntText", chr.Intellect);
        SetInfo("CncText", chr.Concentration);
    }

    public static void SetAP(Character chr)
    {
        SetInfo("APText", chr.currentActionPoint);
        SetInfo("MaxAPText", chr.MaxActionPoint);
        SetInfo("IncomeAPText", chr.IncomeActionPoint);
    }

    public static void SetSkills(Character chr)
    {
        SetInfo("MaxHealthText", chr.MaxHealth);
        SetInfo("InitText", chr.Initiative);
        SetInfo("MeleeSkillText", chr.MeleeAbility);
        SetInfo("MeleeCritText", chr.MeleeCritChance);
    }


    // Start is called before the first frame update
    void Start()
    {
        _instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowExtendedInfoPanel()
    {
        ExtendedInfoPanel.SetActive(!ExtendedInfoPanel.activeSelf);
    }

}
