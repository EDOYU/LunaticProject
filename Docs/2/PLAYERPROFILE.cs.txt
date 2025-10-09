using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PLAYERPROFILE : MonoBehaviour
{
    public static PLAYERPROFILE instance;
    public Dictionary<string, int> 已完成任务=new Dictionary<string, int>();
    public struct Player
    {
        private string name;
        private int hp;
        private int yizhi;
		private int tactics;//作战
        int Physique;//体能

        public string NAME
        {
            get { return name; }
            set { name = value; }
        }

        public int HP
        {
            get { return hp; }
            set { hp = value; }
        }

        public int YIZHI
        {
            get { return yizhi; }
            set { yizhi = value; }
        }
		public int TACTICS
		{
 			get { return tactics; }
            set { tactics = value; }
		}
        public int PHYSIQUE
        {
            get { return Physique; }
            set { Physique = value; }
        }
    }

    public static Player[] player = new Player[20];

    private void Awake()
    {
        instance = this;
        Init();
    }
    
    public void Init()
    {
        player[0] = new Player();
        player[0].NAME = "qiuwu";
        player[0].HP = 6;
        player[0].PHYSIQUE = 4;
        player[0].TACTICS = 5;
        player[0].YIZHI = 3;
    }

    public void 保存任务进度(string 任务, int 进度)
    {
        已完成任务[任务] = 进度;
    }
    public static T 获取数据<T>( string fieldName, int index)
    {
        Player[] players = player;
        if (index < 0 || index >= players.Length)
        {
            throw new IndexOutOfRangeException($"Index {index} is out of range for the players array.");
        }

        Player selectedPlayer = players[index];
        PropertyInfo propertyInfo = typeof(Player).GetProperty(fieldName, BindingFlags.Public | BindingFlags.Instance);

        if (propertyInfo == null)
        {
            throw new ArgumentException($"Property '{fieldName}' not found in Player.");
        }

        if (propertyInfo.PropertyType != typeof(T))
        {
            throw new InvalidOperationException($"Property '{fieldName}' is not of type {typeof(T).Name}.");
        }

        return (T)propertyInfo.GetValue(selectedPlayer);
    }

    public int 获取任务进度(string t)
    {
        int rt = 0;
        try
        {
            rt = 已完成任务[t];
        }
        catch (IndexOutOfRangeException e)
        {
            Debug.LogError("此任务没有做过或者任务名输入错误"+e);
        }
        catch (KeyNotFoundException e)
        {
            Debug.LogError("此任务没有做过或者任务名输入错误"+e);
        }

        return rt;
    }
}