using System.Collections.Generic;//リストを使用
using UnityEngine;
using System;//Serializable属性を使用

//アセットメニューで「Create SoundDataSO」を選択すると「SoundDataSO」を作成できる
[CreateAssetMenu(fileName = "SoundDataSO", menuName = "Create SoundDataSO")]
public class SoundDataSO : ScriptableObject
{
    /// <summary> 
    /// 音名前 
    /// </summary> 
    public enum SoundName
    {
       MainBGM,//メインのBGM
       BtnGameStartSE,//ゲームスタートボタンが押された際の効果音
       BtnRestartSE,//リスタートボタンが押された際の効果音
       GameClearSE,//ゲームクリアした際の効果音
       GameOverSE,//ゲームオーバー時の効果音
       DigestionSE,//ブロックを消化した際の効果音
       CannotRotSE,//ブロックが回転できない座標にいるのに、プレイヤーがブロックを回転させようとした際の効果音
       TenTimeLimitSE,//制限時間が10秒を切った際の効果音
    }

    /// <summary> 
    /// 音のデータを管理するクラス 
    /// </summary> 
    [Serializable]
    public class SoundData
    {
        public SoundName name;//名前
        public AudioClip clip;//クリップ
    }

    public List<SoundData> soundDataList = new List<SoundData>();//音のデータのリスト
}