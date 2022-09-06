using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;//インスタンス

    private int score;//得点

    private float normalFallSpeed;//ブロックの降下速度（普通）

    private float specialFallSpeed;//ブロックの降下速度（特別）

    private float timeLimit;//制限時間

    /// <summary>
    /// 得点取得用
    /// </summary>
    public int Score
    { get { return score; } }

    /// <summary>
    /// ブロックの降下速度（普通）取得用
    /// </summary>
    public float NormalFallSpeed
    { get { return normalFallSpeed; } }

    /// <summary>
    /// ブロックの降下速度（特別）取得用
    /// </summary>
    public float SpecialFallSpeed
    { get { return specialFallSpeed; } }

    /// <summary>
    /// 制限時間取得用
    /// </summary>
    public float TimeLimit
    { get { return timeLimit; } }

    /// <summary>
    /// Startメソッドより前に呼び出される
    /// </summary>
    private void Awake()
    {
        //以下、シングルトンに必須の記述
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
