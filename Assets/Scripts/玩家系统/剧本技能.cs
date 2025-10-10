using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 剧本技能 : MonoBehaviour
{
    public static Dictionary<string, int> 当前技能=new Dictionary<string, int>();
    
    public  static void 增加计数(string 技能名, int 持续回合)
    {
        if (当前技能.ContainsKey(技能名))
            当前技能[技能名] += 持续回合;
        else
            当前技能.Add(技能名, 持续回合);
    }
    public static void 减少数值(string 技能名, int 持续回合=1)
    {
        if (当前技能.TryGetValue(技能名, out int value))
        {
            value -= 持续回合;
            if (value > 0)
                当前技能[技能名] = value;
            else
                当前技能.Remove(技能名);
        }
    }
    public static void 技能计数全部减少()
    {
        List<string> keysToRemove = new List<string>();
        foreach (var kvp in 当前技能)
        {
            int newValue = kvp.Value - 1;
            if (newValue > 0)
                当前技能[kvp.Key] = newValue;
            else
                keysToRemove.Add(kvp.Key);
        }
        foreach (string key in keysToRemove)
            当前技能.Remove(key);
    }

    [ContextMenu("测试继承者技能")]
    public void 测试技能()
    {
        增加计数("继承者", 1);
    }
}
