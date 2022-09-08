using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;//���X�g���g�p
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
    private Text txtGameStart;//�Q�[���X�^�[�g�e�L�X�g

    [SerializeField]
    private Button btnGameStart;//�Q�[���X�^�[�g�{�^��

    [SerializeField]
    private Image[] imgNextBlocks;//���̃u���b�N�̔z��

    [SerializeField]
    private List<LogoData> logoDatasList=new();//���S�̃f�[�^�̃��X�g

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

    //�i���j
    private IEnumerator Start()
    {
        yield return PlayGameStart();

        Debug.Log("�����ꂽ");
    }

    /// <summary>
    /// �Q�[���X�^�[�g���o���s��
    /// </summary>
    /// <returns>�҂�����</returns>
    public IEnumerator PlayGameStart()
    {
        //�Q�[���X�^�[�g�{�^������p
        bool clicked=false;

        //�Q�[���X�^�[�g�{�^����񊈐���
        btnGameStart.interactable = false;

        //�w�i�𔒐F�ɐݒ�
        imgBackGround.color = Color.white;

        //�w�i��\��
        imgBackGround.DOFade(1f, 0f);

        //���S���^�C�g���ɐݒ�
        imgLogo.sprite = GetLogoSprite(LogoType.Title);

        //�Q�[���X�^�[�g�{�^������̃C���[�W�̎擾�ɐ���������
        if(btnGameStart.TryGetComponent(out Image imgGameStart))
        {
            //�Q�[���X�^�[�g�{�^����\��
            imgGameStart.DOFade(1f, 1f);
        }
        //�Q�[���X�^�[�g�{�^������̃C���[�W�̎擾�Ɏ��s������
        else
        {
            //�����
            Debug.Log("�Q�[���X�^�[�g�{�^������̃C���[�W�̎擾�Ɏ��s");
        }

        //���S��\������
        imgLogo.DOFade(1f, 1f);

        //�Q�[���X�^�[�g�{�^���̃e�L�X�g��\��
        txtGameStart.DOText("Game Start",1f).OnComplete(()=>

        //�Q�[���X�^�[�g�{�^����������
        btnGameStart.interactable = true);

        //�Q�[���X�^�[�g�{�^���������ꂽ�ۂ̏�����o�^
        btnGameStart.onClick.AddListener(()=>ClickedBtnGameStart());

        //�Q�[���X�^�[�g�{�^�����������܂ő҂�
        yield return new WaitUntil(()=>clicked==true);

        //�Q�[���X�^�[�g�{�^���������ꂽ�ۂ̏���
        void ClickedBtnGameStart()
        {
            //�Q�[���X�^�[�g�{�^���������ꂽ��Ԃɐ؂�ւ���
            clicked=true;
        }
    }

    /// <summary>
    /// �w�肵�����S�̃X�v���C�g���擾����
    /// </summary>
    /// <param name="logoType">���S�̎��</param>
    /// <returns>�w�肵�����S�̃X�v���C�g</returns>
    private Sprite GetLogoSprite(LogoType logoType)
    {
        //�w�肳��Ă��郍�S�̃X�v���C�g��Ԃ�
        return logoDatasList.Find(x => x.logoType == logoType).sprLogo;
    }
}
