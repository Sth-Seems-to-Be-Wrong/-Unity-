using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinControl
{
    private static int nowCoins =0;
    public static int GetnowCoins()
    {
        return nowCoins;
    }

    public static void Clear()
    {
        nowCoins = 0;
    }

    public static void Add(int coins)
    {
        nowCoins += coins;
    }
    public static void SetCoins(int coins)
    {
        nowCoins = coins;
    }

    public static bool CostCoins(int coins)
    {
        if (coins <= nowCoins)
        {
            nowCoins -= coins;
            return true;
        }
        else return false;
    }
}
