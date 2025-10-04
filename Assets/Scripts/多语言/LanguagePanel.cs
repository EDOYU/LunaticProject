using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using XLua;

public class LanguagePanel : MonoBehaviour{

    public string Info;
    private void OnEnable(){
        if (Info==null) {
            Info = name;
        }
        Component component;
        TryGetComponent(typeof(Image), out component);
        if (component!=null) {
            string path = LanguageSYSTEM.languageENV.Global.Get<string>(Info);
            Debug.Log(path);
            GetComponent<Image>().sprite = GetSpriteFromAssets(path);
            return;
        }

        TryGetComponent(typeof(Text), out component);
        if (component!=null) {
            string text = LanguageSYSTEM.languageENV.Global.Get<string>(Info);
            Text textcomponent= GetComponent<Text>();
            textcomponent.text= text;
            textcomponent.font = Center.instance.GetFont();
            
            return;
        }

    }
    
    public static Sprite GetSpriteFromAssets(string ImgPath){
        Sprite spriteToUse;
        string spritePath = Application.streamingAssetsPath +"/"+ LanguageSYSTEM.ImagePath+"/" +ImgPath+LanguageSYSTEM.Language+".png";

        if (File.Exists(spritePath))
        {
            byte[] spriteData = File.ReadAllBytes(spritePath);

            // 创建Sprite
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(spriteData); // 加载字节数据到Texture2D

            spriteToUse = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
        else
        {
            Debug.LogWarning($"无法找到Sprite：{spritePath}");
            spriteToUse = Resources.Load<Sprite>("DefaultSprite"); // 替换为默认的Sprite
        }

        return spriteToUse;
    }
    
}
