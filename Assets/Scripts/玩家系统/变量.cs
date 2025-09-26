using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 变量 {
    public static Dictionary<string ,int> 全局变量=new Dictionary<string, int>();
    public static void 修改变量(string key,int target)
    {
        if (全局变量.ContainsKey(key))
        {
            全局变量[key] = target;
        }
        else
        {
            全局变量.Add(key, target);
        }
    }

    public static int 获取变量(string key)
    {
        try
        {
            return 全局变量[key];
        }
        catch (KeyNotFoundException e)
        {
            return -1;
        }
      
    }
}
