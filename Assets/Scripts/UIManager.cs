using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//UI���g�p
using DG.Tweening;//DOTween���g�p
using System;//Serializable�������g�p

public class UIManager : MonoBehaviour
{
    /// <summary>
    /// ���S�̎��
    /// </summary>
    public enum LogoType
    {
        Title,GameClear,GameOver//�񋓎q
    }

    /// <summary>
    /// ���S�̃f�[�^�̊Ǘ��p
    /// </summary>
    [Serializable]
    public class LogoData
    {
        public LogoType logoType;//���S�̎��
        public Sprite sprLogo;//�X�v���C�g
    }

    public static UIManager instance;//�C���X�^���X

    [SerializeField]
    private Image imgBackGround;//�w�i

    [SerializeField]
    private Image imgLogo;//���S

    [SerializeField]
    private Image imgHold;//�ۑ����ꂽ�u���b�N

    [SerializeField]
    private Text txtScore;//���_

    [SerializeField]
    private Text txtTimeLimit;//��������

    [SerializeField]
    private Image[] imgNextBlocks;//���̃u���b�N�̔z��

    [SerializeField]
    private LogoData[] logoDatas;//���S�̃f�[�^�̔z��

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
    /// �Q�[���X�^�[�g���o���s��
    /// </summary>
    /// <returns>�҂�����</returns>
    public IEnumerator PlayGameStart()
    {
        //�i���j
        yield return null;
    }
}
