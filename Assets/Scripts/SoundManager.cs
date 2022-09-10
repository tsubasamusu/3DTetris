using UnityEngine;
using DG.Tweening;//DOTweenを使用

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;//インスタンス

    [SerializeField]
    private SoundDataSO soundDataSO;//SoundDataSO

    [SerializeField]
    private AudioSource mainAud;//メインのAudioSource

    [SerializeField]
    private AudioSource subAud;//サブのAudioSource

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

    /// <summary>
    /// 指定した音を再生する
    /// </summary>
    /// <param name="name">音の名前</param>
    /// <param name="loop">繰り返すかどうか</param>
    public void PlaySound(SoundDataSO.SoundName name,bool loop=false)
    {
        //指定されている名前の音のクリップを取得
        AudioClip clip = soundDataSO.soundDataList.Find(x => x.name == name).clip;

        //繰り返すなら
        if(loop)
        {
            //クリップを設定
            mainAud.clip = clip;

            //繰り返すように設定
            mainAud.loop = loop;

            //メインのAudioSourceで音を再生
            mainAud.Play();
        }
        //繰り返さないなら
        else
        {
            //サブのAudioSourceで音を再生
            subAud.PlayOneShot(clip);
        }
    }

    /// <summary>
    /// （メインのAudioSourceの）音を止める
    /// </summary>
    /// <param name="fadeOutTime">フェードアウト時間</param>
    public void StopSound(float fadeOutTime = 0f)
    {
        //音をフェードアウトさせる
        mainAud.DOFade(0f, fadeOutTime);
    }
}
