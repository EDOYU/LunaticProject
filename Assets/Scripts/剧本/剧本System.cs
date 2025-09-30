using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using OfficeOpenXml;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class 剧本System: MonoBehaviour
{
    public List<GameObject> 已生成文本 = new List<GameObject>();
    public List<GameObject> 选项按钮 = new List<GameObject>();
    public GameObject Content;
    public GameObject 剧本预制体;
    public GameObject 剧本父物体Content;
    public Image BG,SPEAKERBG;
    public Text 说话人TextObject;
    public float 起始生成偏移;
    public float 间隔;
    private int 已阅读=0;
    public static float Yoffset;

    private string 储存的检定结果;
    public string[][] 已储存剧本;
    private string 当前事件 => 已储存剧本[已阅读][0];
    private string 当前说话人 => 已储存剧本[已阅读][1];
    private string 当前说话内容 => 已储存剧本[已阅读][2];
    int 语言偏移
    {
        get
        {
            switch (Center.Language)
            {
                case "CN":
                    return 0;
                case "EN":
                    return 1;
                case "JP":
                    return 2;
            }

            return 1;
        }
    }

    public void Awake()
    {
        已储存剧本 = 读取表格数据("Plot",Center.Languageint );
        Debug.LogError("剧本测试中");
        刷新();
    }

    public void 刷新()
    {
        已阅读 = 0;
      清空文本();
      
    }

    void 清空文本()
    {
        foreach (var VARIABLE in 已生成文本)
        {
            Destroy(VARIABLE.gameObject);
        }

        Yoffset = 起始生成偏移;
        Content.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,起始生成偏移);
    }
[ContextMenu("下一句")]
    public void Next()
    {
        if (已阅读>=已储存剧本.Length)
        {
            return;
        }
        if (当前说话内容.Contains(Center.Tag_notspawn))
        {
            进行指令(当前事件);
            return;
        }
   
        进行指令(当前事件);
        生成剧本预制体();
        已阅读++;
    }

    public GameObject 生成剧本预制体()
    {
     //   Debug.Log("调用生成");
        if (已阅读>0 && 当前说话内容== 已储存剧本[已阅读-1][2])
        {
            return null;
        }
        if (当前说话内容.Contains(Center.Tag_notspawn))
        {
            return null;
        }
        GameObject go = Instantiate(剧本预制体, 剧本父物体Content.transform);
        Yoffset += 间隔;
        go.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -Yoffset);
        string 文本 = 当前说话内容;
        if (文本.Contains(Center.Tag_checkview))
        {
            文本 = 储存的检定结果;
        }

    
        Yoffset+= go.GetComponent<打字机>().初始化(文本);
        说话人TextObject.text = 当前说话人;
        Content.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Yoffset);
        
        已生成文本.Add(go);
        return go;
    }
    
    public string[][] 读取表格数据(string 文件名, int 语言偏移)
    {
        string filepath = Application.streamingAssetsPath + "/" + 文件名;

        // 使用EPPlus打开临时路径的Excel文件
        using (var 包 = new ExcelPackage(new FileInfo(filepath)))
        {
            var 工作表 = 包.Workbook.Worksheets[1];

            // 获取工作表的总行数
            int 总行数 = 工作表.Dimension.End.Row;

            // 初始化结果数组，从第二行开始读取
            string[][] 数据 = new string[总行数 - 1][];

            for (int i = 2; i <= 总行数; i++) // 从第二行开始
            {
                数据[i - 2] = new string[3]; // 初始化每行的数据

                数据[i - 2][0] = 工作表.Cells[i, 1].Text; // 事件

                // 根据语言偏移获取合并后的内容
                string 合并内容 = 工作表.Cells[i, 2 + 语言偏移].Text;

                // 找到冒号并分割
                int 冒号位置 = 合并内容.IndexOf(':');
                if (冒号位置 != -1)
                {
                    数据[i - 2][1] = 合并内容.Substring(0, 冒号位置).Trim(); // 说话人
                    数据[i - 2][2] = 合并内容;
                }
                else
                {
                    数据[i - 2][1] = "";
                    数据[i - 2][2] = 合并内容;
                }

            }

            return 数据;
        }
    }

    public void LoadImage(string imageName, Image targetimg)
    {
        Texture2D texture = Resources.Load<Texture2D>("CG/" + imageName);

        if (texture == null)
        {
            Debug.LogError("无法加载图片: " + imageName);
            return;
        }

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
         targetimg   .sprite = sprite;
    }
    
      public void 进行指令(string tar){
          Debug.Log($"进行指令{tar}");
                var face = tar.Split(Center.Plot指令分隔符);
                foreach (var key in face)
                {
                    if (key.Contains(Center.Command_background))
                    {
                        var prams = 指令切割(key);
                      LoadImage(prams[0],BG);
                    }
                    if (key.Contains(Center.Command_SpeakerSet))
                    {
                        var prams = 指令切割(key);
                        LoadImage(prams[0],SPEAKERBG);
                    }
                    if (key.Contains(Center.Command_Check))//检定
                    {
                        var prams = 指令切割(key);
                        string 被检定属性 = prams[0];
                        int 骰子数量=Convert.ToInt32(prams[1]);
                        int 骰子大小=Convert.ToInt32(prams[2]);
                        int 检定角色=Convert.ToInt32(prams[3]);
                        int 检定目标=Convert.ToInt32(prams[4]);
                        string 成功修改变量= prams[5];
                        int 修改结果=Convert.ToInt32(prams[6]);
                        string 失败修改变量 = prams[7];
                        int 修改结果F=Convert.ToInt32(prams[8]);
                        int 修正值 = PLAYERPROFILE.获取数据<int>(被检定属性,检定角色);
                        int 随机值 = 0;
                        for (int i = 0; i < 骰子数量; i++)
                        {
                            随机值 += Random.Range(0, 骰子大小)+1;
                        }
                        int 最终值 = 随机值 + 修正值;
                        if (最终值>=检定目标)
                        {
                           变量.修改变量(成功修改变量,修改结果);
                           Debug.Log(成功修改变量+$"修改为{修改结果}");
                        }
                        else
                        {
                            变量.修改变量(失败修改变量,修改结果F);
                            Debug.Log(失败修改变量+$"修改为{修改结果F}");
                        }

                        储存的检定结果 = $"{骰子数量}D{骰子大小}={随机值}  {随机值}+ {修正值}={最终值}";
                    }

                    if (key.Contains(Center.Command_Choice))
                    {
                        选项按钮.Clear();
                        var prams = 指令切割(key);
                        int 选项长度= Convert.ToInt32(prams[0]);
                        for (int i = 0; i < 选项长度; i++)
                        {
                            已阅读++;
                            GameObject go = 生成剧本预制体();
                            GameObject text=  go.GetComponent<打字机>().textComponent.gameObject;
                            text.AddComponent<Button>();
                            string 事件=当前事件;
                            text.GetComponent<Button>().onClick.AddListener(() =>
                            {
                                if (事件 != null) 进行指令(事件);
                            
                                foreach (var VARIABLE in 选项按钮)
                                {
                                    VARIABLE.GetComponent<Button>().enabled = false;
                                    VARIABLE.GetComponent<Text>().color = Color.gray;
                                }
                                text.GetComponent<Text>().color = Color.yellow;
                            });
                            选项按钮.Add(text);
                        }
                        已阅读++;
                    }
                    if (key.Contains(Center.Command_Debug))
                    {
                        Debug.Log($"<color=red>剧本Debug</color>>>{key}");
                    }
                    if (key.Contains(Center.Command_Setspace))
                    {
                        var prams = 指令切割(key);
                        间隔=Convert.ToInt32(prams[0]);
                    }

                    if (key.Contains(Center.Command_Next))
                    {
                        var prams = 指令切割(key);
                        已储存剧本 = 读取表格数据(prams[0], Center.Languageint);
                        已阅读 = Convert.ToInt32( prams[1]);
                        Next();
                    }
                    if (key.Contains(Center.Command_If))
                    {
                        var prams = 指令切割(key);
                        if (变量.获取变量(prams[0])==Convert.ToInt32(prams[1]))
                        {
                            已阅读=Convert.ToInt32(prams[2]);
                        }
                    }
                    if (key.Contains(Center.Command_Skip))
                    {
                        已阅读++;
                        Next();
                    }
                    if (key.Contains(Center.Command_Jump))
                    {   
                        var prams = 指令切割(key);
                        已阅读=Convert.ToInt32(prams[0]);
                    }
                    if (key.Contains(Center.Command_Refresh))
                    {   
                      刷新();
                    }
                    if (key.Contains(Center.Command_Clear))
                    {   
                        清空文本();
                    }
    //                 if (key.Contains(PlotManager.Command_sound))
    //                 {
    //                     var prams = 指令切割(key);
    // //                    Debug.LogError(prams[0]);
    //                     SE.clip = Resources.Load<AudioClip>($"Sounds/{prams[0]}");
    //                     SE.loop = prams[1] == "1";
    //                     SE.volume = Progress.Options.Volume_SE;
    //                     SE.Play();
    //                 }
    //                 if (key.Contains(PlotManager.Command_music))
    //                 {
    //                     var prams =指令切割(key);
    //                     Debug.LogError(prams[0]);
    //                     BGM.volume = Progress.Options.Volume_BGM;
    //                     BGM.clip = Resources.Load<AudioClip>($"Music/{prams[0]}");
    //                     BGM.Play();
    //                 }
    //
    //                 if (key.Contains(PlotManager.Command_Main))
    //                 {
    //                     SceneManager.LoadScene("Plot_End");
    //                 }
                }
      }
      public static string[] 指令切割(string command)
      {
          var match = Regex.Match(command, @"\(([^)]*)\)");
          if (match.Success)
          {
              var prams = match.Groups[1].Value.Split(',');
              return prams;
          }

          if(match.Groups[1].Value == "")
          {
              Debug.LogError($"指令格式错误，请检查是否有参数");
          }
          else
          {
              Debug.LogError($"指令格式错误，请检查是否有括号");
          }

          return null;
      }
}