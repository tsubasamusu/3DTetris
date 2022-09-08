using System.Collections.Generic;//���X�g���g�p
using UnityEngine;
using System;//Serializable�������g�p

//�A�Z�b�g���j���[�ŁuCreate SoundDataSO�v��I������ƁuSoundDataSO�v���쐬�ł���
[CreateAssetMenu(fileName = "SoundDataSO", menuName = "Create SoundDataSO")]
public class SoundDataSO : ScriptableObject
{
    /// <summary> 
    /// �����O 
    /// </summary> 
    public enum SoundName
    {
       MainBGM,//���C����BGM
       BtnGameStartSE,//�Q�[���X�^�[�g�{�^���������ꂽ�ۂ̌��ʉ�
       BtnRestartSE,//���X�^�[�g�{�^���������ꂽ�ۂ̌��ʉ�
       GameClearSE,//�Q�[���N���A�����ۂ̌��ʉ�
       GameOverSE,//�Q�[���I�[�o�[���̌��ʉ�
       DigestionSE,//�u���b�N�����������ۂ̌��ʉ�
       CannotRotSE,//�u���b�N����]�ł��Ȃ����W�ɂ���̂ɁA�v���C���[���u���b�N����]�����悤�Ƃ����ۂ̌��ʉ�
       TenTimeLimitSE,//�������Ԃ�10�b��؂����ۂ̌��ʉ�
    }

    /// <summary> 
    /// ���̃f�[�^���Ǘ�����N���X 
    /// </summary> 
    [Serializable]
    public class SoundData
    {
        public SoundName name;//���O
        public AudioClip clip;//�N���b�v
    }

    public List<SoundData> soundDataList = new List<SoundData>();//���̃f�[�^�̃��X�g
}