using System;
using System.Reflection;
using UnityEngine;

public class PLAYERPROFILE : MonoBehaviour
{
    public struct Player
    {
        private string name;
        private int hp;
        private int yizhi;

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
    }

    public static Player[] player = new Player[20];

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
}