using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class 打字机 : MonoBehaviour
{
    public bool IsTesting;
    public Text textComponent;
    public string 完整文本;
    public float 字符延迟 = 0.03f;
    private bool inited;

    private string currentText = "";

    public float 初始化(string 文本)
    {
        float 框大小 = 0;
        剧本System.instance.当文本更新时 += 下一句;
 
        if (!inited)
        {
            inited = true;
            完整文本 = 文本;
            textComponent.text = 完整文本;
            Canvas.ForceUpdateCanvases();
            // 计算高度
            框大小 = textComponent.GetComponent<RectTransform>().rect.height;

            // 启动打字机效果
            StartCoroutine(ShowText());
        }
        // 返回计算出的高度
        return 框大小;
    }

    private void Update()
    {
        if (IsTesting)
        {
            Debug.Log($"位置{transform.position}");
        }
    }

    void 下一句()
    {
        StopAllCoroutines();
        textComponent.text = 完整文本 + " ";
    }

    private void OnDisable()
    {
        剧本System.instance.当文本更新时 -= 下一句;
    }

    IEnumerator ShowText()
    {
        textComponent.color = new Color(1, 1, 1, 1); // 确保文本在显示时是可见的
        for (int i = 0; i < 完整文本.Length; i++)
        {
            currentText = 完整文本.Substring(0, i + 1);
            textComponent.text = currentText;
            yield return new WaitForSeconds(字符延迟);
        }
    }
    
    
    /// <summary>
    /// 判断某个Text是否在屏幕渲染范围内
    /// </summary>
    /// <returns>true 表示在范围内，false 表示不在范围内</returns>
    public bool IsTextVisible()
    {
        if (textComponent == null)
        {
            Debug.LogWarning("targetText 未设置！");
            return false;
        }
        
        RectTransform rectTransform = textComponent.rectTransform;
        
        // 获取四个角在世界空间下的坐标
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        // 依次将四个角转换到屏幕空间，检查是否在屏幕范围内
        for (int i = 0; i < corners.Length; i++)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(corners[i]);
            
            // 如果z小于0，说明在摄像机后方，也忽略
            if (screenPos.z < 0)
                continue;

            // 判断 x, y 是否在屏幕范围内
            if (screenPos.x >= 0 && screenPos.x <= Screen.width &&
                screenPos.y >= 0 && screenPos.y <= Screen.height)
            {
                return true;
            }
        }

        return false;
    }
}
