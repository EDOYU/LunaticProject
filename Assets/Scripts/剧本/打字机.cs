using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class 打字机 : MonoBehaviour
{
    public Text textComponent;
    public string 完整文本;
    public float 字符延迟 = 0.03f; 

    private string currentText = "";

    public float 初始化(string 文本)
    {     
        完整文本 = 文本;
        textComponent.text = 完整文本;
        Canvas.ForceUpdateCanvases();
        // 计算高度
        float 框大小 = textComponent.GetComponent<RectTransform>().rect.height/10;

        // 启动打字机效果
        StartCoroutine(ShowText());

        // 返回计算出的高度
        return 框大小;
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
}
