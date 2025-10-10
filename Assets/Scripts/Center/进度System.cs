using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 进度System : MonoBehaviour
{
  public void 存档()
  {
    ES3.Save<Dictionary<string, int>>("EventFinish",PLAYERPROFILE.instance.已完成任务);
    ES3.Save("Player",PLAYERPROFILE.player);
    ES3.Save("MySkill",剧本技能.当前技能);
    ES3.Save("Bag", 背包.当前背包);
  }

  public void 读档()
  {
    PLAYERPROFILE.instance.已完成任务 = ES3.Load<Dictionary<string, int>>("EventFinish");
    PLAYERPROFILE.player=ES3.Load<PLAYERPROFILE.Player[]>("Player");
    剧本技能.当前技能=ES3.Load<Dictionary<string, int>>("EventFinish");
    背包.当前背包=ES3.Load<Dictionary<string, int>>("Bag");
  }
}
