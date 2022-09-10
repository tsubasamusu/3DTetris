using UnityEngine;
using DG.Tweening;//DOTween���g�p

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;//�C���X�^���X

    [SerializeField]
    private SoundDataSO soundDataSO;//SoundDataSO

    [SerializeField]
    private AudioSource mainAud;//���C����AudioSource

    [SerializeField]
    private AudioSource subAud;//�T�u��AudioSource

    /// <summary>
    /// Start���\�b�h���O�ɌĂяo�����
    /// </summary>
    private void Awake()
    {
        //�ȉ��A�V���O���g���ɕK�{�̋L�q
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
    /// �w�肵�������Đ�����
    /// </summary>
    /// <param name="name">���̖��O</param>
    /// <param name="loop">�J��Ԃ����ǂ���</param>
    public void PlaySound(SoundDataSO.SoundName name,bool loop=false)
    {
        //�w�肳��Ă��閼�O�̉��̃N���b�v���擾
        AudioClip clip = soundDataSO.soundDataList.Find(x => x.name == name).clip;

        //�J��Ԃ��Ȃ�
        if(loop)
        {
            //�N���b�v��ݒ�
            mainAud.clip = clip;

            //�J��Ԃ��悤�ɐݒ�
            mainAud.loop = loop;

            //���C����AudioSource�ŉ����Đ�
            mainAud.Play();
        }
        //�J��Ԃ��Ȃ��Ȃ�
        else
        {
            //�T�u��AudioSource�ŉ����Đ�
            subAud.PlayOneShot(clip);
        }
    }

    /// <summary>
    /// �i���C����AudioSource�́j�����~�߂�
    /// </summary>
    /// <param name="fadeOutTime">�t�F�[�h�A�E�g����</param>
    public void StopSound(float fadeOutTime = 0f)
    {
        //�����t�F�[�h�A�E�g������
        mainAud.DOFade(0f, fadeOutTime);
    }
}
