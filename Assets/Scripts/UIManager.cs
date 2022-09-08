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
    private Image imgButton;//�{�^���̃C���[�W

    [SerializeField]
    private Text txtScore;//���_

    [SerializeField]
    private Text txtTimeLimit;//��������

    [SerializeField]
    private Text txtButton;//�{�^���̃e�L�X�g

    [SerializeField]
    private Button button;//�{�^��

    [SerializeField]
    private Transform resultTran;//���ʕ\���ʒu

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
        yield return PlayGameClear();

        Debug.Log("END");
    }

    /// <summary>
    /// �Q�[���X�^�[�g���o���s��
    /// </summary>
    /// <returns>�҂�����</returns>
    public IEnumerator PlayGameStart()
    {
        //���o�I������p
        bool end = false;

        //�{�^������p
        bool clicked = false;

        //�{�^����񊈐���
        button.interactable = false;

        //�w�i�𔒐F�ɐݒ�
        imgBackGround.color = Color.white;

        //�{�^����F�ɐݒ�
        imgButton.color = Color.blue;

        //�w�i��\��
        imgBackGround.DOFade(1f, 0f);

        //���S���^�C�g���ɐݒ�
        imgLogo.sprite = GetLogoSprite(LogoType.Title);

        //�{�^����\��
        imgButton.DOFade(1f, 1f);

        //���S��\������
        imgLogo.DOFade(1f, 1f);

        //�{�^���̃e�L�X�g��ݒ肵�\��
        txtButton.DOText("Game Start", 1f).OnComplete(() =>

        //�{�^����������
        button.interactable = true);

        //�{�^���������ꂽ�ۂ̏�����o�^
        button.onClick.AddListener(() => ClickedButton());

        //�{�^�����������܂ő҂�
        yield return new WaitUntil(() => clicked == true);

        //�w�i���\���ɂ���
        imgBackGround.DOFade(0f, 1f);

        //���S���\���ɂ���
        imgLogo.DOFade(0f, 1f);

        //�{�^���̃C���[�W���\���ɂ���
        imgButton.DOFade(0f, 1f);

        //�{�^���̃e�L�X�g���\���ɂ���
        txtButton.DOFade(0f, 1f).OnComplete(() =>

        //���o���I�������Ԃɐ؂�ւ���
        end = true);

        //���o���I���܂ő҂�
        yield return new WaitUntil(() => end == true);

        //�{�^���������ꂽ�ۂ̏���
        void ClickedButton()
        {
            //�{�^���������ꂽ��Ԃɐ؂�ւ���
            clicked = true;

            //�{�^����񊈐���
            button.interactable = false;
        }
    }

    /// <summary>
    /// �Q�[���I�[�o�[���o���s��
    /// </summary>
    /// <returns>�҂�����</returns>
    public IEnumerator PlayGameOver()
    {
        //���o�I������p
        bool end = false;

        //�{�^������p
        bool clicked = false;

        //�w�i�����F�ɐݒ�
        imgBackGround.color = Color.black;

        //���S���uGameOver�v�ɐݒ�
        imgLogo.sprite = GetLogoSprite(LogoType.GameOver);

        ///�{�^���̐F��ԐF�ɐݒ�
        imgButton.color = Color.red;

        ///�{�^���������ꂽ�ۂ̏�����o�^
        button.onClick.AddListener(() => ClickedButton());

        //�w�i��\��
        imgBackGround.DOFade(1f, 1f);

        //���S��\��
        imgLogo.DOFade(1f, 1f);

        //�{�^����\��
        imgButton.DOFade(1f, 1f);

        //�{�^���̃e�L�X�g��ݒ肵�A�\��
        txtButton.DOText("Restart", 1f).OnComplete(() =>

        //�{�^����������
        button.interactable = true);

        //�{�^�����������܂ő҂�
        yield return new WaitUntil(() => clicked == true);

        //�w�i�𔒐F�ɕύX
        imgBackGround.DOColor(Color.white, 1f);

        //���S���\���ɂ���
        imgLogo.DOFade(0f, 1f);

        //�{�^���̃C���[�W���\���ɂ���
        imgButton.DOFade(0f, 1f);

        //�{�^���̃e�L�X�g���\���ɂ���
        txtButton.DOFade(0f, 1f).OnComplete(() =>

        //���o���I�������Ԃɐ؂�ւ���
        end = true);

        //���o���I���܂ő҂�
        yield return new WaitUntil(() => end == true);

        //�{�^���������ꂽ�ۂ̏���
        void ClickedButton()
        {
            //�{�^���������ꂽ��Ԃɐ؂�ւ���
            clicked = true;
        }
    }

    /// <summary>
    /// �Q�[���N���A���o���s��
    /// </summary>
    /// <returns>�҂�����</returns>
    public IEnumerator PlayGameClear()
    {
        //���o�I������p
        bool end = false;

        //�{�^������p
        bool clicked = false;

        //�w�i�𔒐F�ɐݒ�
        imgBackGround.color = Color.white;

        //���S���uGameClear�v�ɐݒ�
        imgLogo.sprite = GetLogoSprite(LogoType.GameClear);

        ///�{�^���̐F�����F�ɐݒ�
        imgButton.color = Color.yellow;

        ///�{�^���������ꂽ�ۂ̏�����o�^
        button.onClick.AddListener(() => ClickedButton());

        //�w�i��\��
        imgBackGround.DOFade(1f, 1f);

        //���S��\��
        imgLogo.DOFade(1f, 1f);

        //�{�^����\��
        imgButton.DOFade(1f, 1f);

        //���_�����ʕ\���ʒu�Ɉړ�������
        txtScore.transform.DOMove(resultTran.position, 1f);

        //�{�^���̃e�L�X�g��ݒ肵�A�\��
        txtButton.DOText("Restart", 1f).OnComplete(() =>

        //�{�^����������
        button.interactable = true);

        //�{�^�����������܂ő҂�
        yield return new WaitUntil(() => clicked == true);

        //���S���\���ɂ���
        imgLogo.DOFade(0f, 1f);

        //�{�^���̃C���[�W���\���ɂ���
        imgButton.DOFade(0f, 1f);

        //���_���\���ɂ���
        txtScore.DOFade(0f, 1f);

        //�{�^���̃e�L�X�g���\���ɂ���
        txtButton.DOFade(0f, 1f).OnComplete(() =>

        //���o���I�������Ԃɐ؂�ւ���
        end = true);

        //���o���I���܂ő҂�
        yield return new WaitUntil(() => end == true);

        //�{�^���������ꂽ�ۂ̏���
        void ClickedButton()
        {
            //�{�^���������ꂽ��Ԃɐ؂�ւ���
            clicked = true;
        }
    }

    /// <summary>
    /// ���_�̕\����ݒ肷��
    /// </summary>
    /// <param name="score">���_</param>
    public void SetTxtScore(int score)
    {
        //���_�̃e�L�X�g��ݒ�
        txtScore.text = score.ToString();
    }

    /// <summary>
    /// �������Ԃ̕\����ݒ肷��
    /// </summary>
    /// <param name="remainingTime">�c�莞��</param>
    public void SetTxtTimeLimit(float remainingTime)
    {
        //�������Ԃ̕\�����c�莞�Ԃɐݒ�
        txtTimeLimit.text = remainingTime.ToString();
    }

    /// <summary>
    /// �ۑ�����Ă���u���b�N�̕\����ݒ肷��
    /// </summary>
    /// <param name="blockSprite">�u���b�N�̃X�v���C�g</param>
    public void SetImgHoldBllock(Sprite blockSprite)
    {
        //�ۑ�����Ă���u���b�N�̃X�v���C�g��ݒ�
        imgHold.sprite = blockSprite;
    }

    /// <summary>
    /// �����\��̃u���b�N�̕\����ݒ肷��
    /// </summary>
    /// <param name="blockDatas">�����\��̃u���b�N�̃f�[�^�̃��X�g</param>
    public void SetImgNextBlocks(BlockDataSO.BlockData[] blockDatas)
    {
        //�p�ӂ��Ă���UI�̐������J��Ԃ�
        for (int i = 0; i < imgNextBlocks.Length; i++)
        {
            //�����\��̃u���b�N�̃C���[�W�̃X�v���C�g��ݒ�
            imgNextBlocks[i].sprite = blockDatas[i].sprite;
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
