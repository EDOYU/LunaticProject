using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Center : MonoBehaviour
{
    
    public static Center instance;
    public Font[] Fonts;
    
    public static string Language = "CN";

    public static int Languageint
    {
        get {
            switch (Language)
            {case "CN":
                return 0;
            case "EN":
                return 1;
            case "JP":
                return 2;
            }

            return 0;
        }
    }

    #region Plots

    public static string Command_background="CG";
    public static string Command_SpeakerSet="SPEAK";
    public static string Command_Check="CHECK";
    public static string Command_Set="SET";
    public static string Command_Choice="ISCHOICE";
    public static string Command_Debug="DEBUG";
    public static string Command_Setspace="SETSPACE";
    public static string Command_Next="NEXT";
    public static string Command_If="IF";
    public static string Command_End = "END";
    public static string Command_Skip="SKIP";
    public static string Command_Jump="JUMPTO";
    public static string Command_Refresh="REFRESH";
    public static string Command_Clear="CLEAR";
    public static string Command_Item="ITEM";
    public static string Command_Modify="MODIFY";
    public static string Tag_checkview = "!CHECKVIEW";
    public static string Tag_notspawn = "!DONTSPAWN";
    public static readonly char Plot指令分隔符 = '+';

    #endregion

    private void Awake()
    {
        instance = this;
    }

    public Font GetFont()
    {
        return Fonts[Languageint];
    }
}