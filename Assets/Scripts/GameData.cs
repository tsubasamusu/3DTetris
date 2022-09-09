using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;//インスタンス

    [SerializeField]
    private float normalFallSpeed;//ブロックの降下速度（普通）

    [SerializeField]
    private float specialFallSpeed;//ブロックの降下速度（特別）

    [SerializeField]
    private float ghostFallSpeed;//ゴーストの降下速度

    [SerializeField]
    private float timeLimit;//制限時間

    [SerializeField, Header("ブロックの生成予定の数")]
    private int appointmentsNumber;//ブロックの生成予定の数

    [SerializeField,Header("1列あたりの得点")]
    private int scorePerColumn;//1列あたりの得点

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
    /// 「ブロックの生成予定の数」取得用
    /// </summary>
    public int AppointmentsNumber
    { get { return appointmentsNumber; } }

    /// <summary>
    /// 「1列あたりの得点」取得用
    /// </summary>
    public int ScorePerColumn
    { get { return scorePerColumn; } }

    /// <summary>
    /// ゴーストの降下速度取得用
    /// </summary>
    public float GhostFallSpeed
    { get { return ghostFallSpeed; } }

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
