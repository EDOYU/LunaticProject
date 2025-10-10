using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class 背包 : MonoBehaviour
{
    public static Dictionary<string, int> 当前背包  = new Dictionary<string, int>();
    public static Dictionary<string, int> 准备使用道具 = new Dictionary<string, int>();
    public static void 增加计数(string 道具名, int 数量 = 1)
    {
        if (数量 <= 0) return;

        if (当前背包.ContainsKey(道具名))
            当前背包[道具名] += 数量;
        else
            当前背包[道具名] = 数量;
    }

    public static void 减少数值(string 道具名, int 数量 = 1)
    {
        if (数量 <= 0) return;

        if (当前背包.TryGetValue(道具名, out int value))
        {
            value -= 数量;
            if (value > 0)
                当前背包[道具名] = value;
            else
                当前背包.Remove(道具名);
        }
    }
    public static void 背包计数全部减少()
    {
        List<string> keysToRemove = new List<string>();
        foreach (var kvp in 当前背包)
        {
            int newValue = kvp.Value - 1;
            if (newValue > 0)
                当前背包[kvp.Key] = newValue;
            else
                keysToRemove.Add(kvp.Key);
        }
        foreach (string key in keysToRemove)
            当前背包.Remove(key);
    }
    
    public static int 获取数量(string 道具名)
    {
        return 当前背包.TryGetValue(道具名, out int num) ? num : 0;
    }
    public static bool 使用道具(string 道具名, int 数量 = 1)
    {
        if (数量 <= 0) return false;
        int cur = 获取数量(道具名);
        if (cur < 数量)
        {
            Debug.LogWarning($"背包中 [{道具名}] 数量不足，无法消耗！");
            return false;
        }
        减少数值(道具名, 数量);
        // 时间戳
        string timeSuffix = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        string keyWithTime = $"{道具名}_{timeSuffix}";
        
        准备使用道具[keyWithTime] = 数量;

        return true;
    }
    
    public static void 道具倒数(string t)
    {
        if (string.IsNullOrEmpty(t)) return;

        var keys = 准备使用道具.Keys.Where(k => k.Contains(t)).ToList();
        foreach (var key in keys)
        {
            int newValue = 准备使用道具[key] - 1;
            if (newValue > 0)
                准备使用道具[key] = newValue;
            else
                准备使用道具.Remove(key);
        }
    }
    public static int 检查已使用道具(string t)
    {
        if (string.IsNullOrEmpty(t)) return 0;

        int cnt = 0;
        foreach (var key in 准备使用道具.Keys)
        {
            if (key.Contains(t))
                cnt++;
        }
        return cnt;
    }
    [ContextMenu("TEST_添加10药水")]
    void TEST_ADD()
    {
        增加计数("药水", 10);
        Debug.Log($"药水数量 = {获取数量("药水")}");
    }

    [ContextMenu("TEST_使用3药水")]
    void TEST_USE()
    {
        使用道具("药水", 3);
        Debug.Log("准备使用道具表：" + DictionaryToString(准备使用道具));
    }

    [ContextMenu("TEST_药水倒数一次")]
    void TEST_COUNTDOWN()
    {
        道具倒数("药水");
        Debug.Log("倒数后表：" + DictionaryToString(准备使用道具));
    }
    
    private string DictionaryToString(Dictionary<string,int> dict)
    {
        string s = "";
        foreach(var kv in dict)
            s += $"{kv.Key}:{kv.Value} ";
        return s;
    }
}
