using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;
public class LanguageSYSTEM : MonoBehaviour{

    public static Font Font;
    public static string Language="CN";
    public static string ImagePath="图片";
    public static LuaEnv languageENV;

    public void Awake(){
        languageENV = new LuaEnv();
     
        string info = File.ReadAllText(Application.streamingAssetsPath+"/Language/"+ $"{Center.Language}.lua.txt");
        languageENV.DoString(info);
    }

    public void GetFont(){
        switch (Language) {
            case "CN":
                break;
        }
    }
    
}
