using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class 任务节点 : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(() =>
        {
            大地图系统.instance.开始剧情 (name.Replace("(Clone)",""));
        });
    }
}
