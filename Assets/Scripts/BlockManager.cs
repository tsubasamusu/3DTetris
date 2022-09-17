using System.Collections.Generic;//���X�g���g�p
using System.Collections;//IEnumerator���g�p
using UnityEngine;
using DG.Tweening;//DOTween���g�p

public class BlockManager : MonoBehaviour
{
    public static BlockManager instance;//�C���X�^���X

    [SerializeField]
    private Material ghostMaterial;//�S�[�X�g�p�̃}�e���A��

    [HideInInspector]
    public List<GameObject> cubeList = new();//���݁A�X�e�[�W��ɒ~�ς���Ă��闧���̂̃��X�g

    private BlockController currentBlock;//���݃A�N�e�B�u�ȃu���b�N

    private BlockDataSO.BlockData holdBlockData;//�ۑ����ꂽ�u���b�N�̃f�[�^

    private BlockGenerator blockGenerator;//BlockGenerator

    private GameObject ghostObj;//�S�[�X�g�̃Q�[���I�u�W�F�N�g

    private bool endDigestion=true;//�����̏������I��������ǂ���

    private bool isGameOver;//�Q�[���I�[�o�[���ǂ���

    /// <summary>
    /// �Q�[���I�[�o�[����擾�E�ݒ�p
    /// </summary>
    public bool IsGameOver
    { get { return isGameOver; } set { isGameOver = value; } }

    /// <summary>
    /// �����I������擾�p
    /// </summary>
    public bool EndDigestion
    { get { return endDigestion; } }

    /// <summary>
    /// ���݃A�N�e�B�u�ȃu���b�N�̎擾�E�ݒ�p
    /// </summary>
    public BlockController CurrentBlock
    { get { return currentBlock; } set { currentBlock = value; } }

    /// <summary>
    /// Start���\�b�h���O�ɌĂяo�����
    /// </summary>
    private void Awake()
    {
        //�ȍ~�A�V���O���g���ɕK�{�̋L�q
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
    /// BlockManager�̏����ݒ���s��
    /// </summary>
    /// <param name="gameManager"></param>
    public void SetUpBlockManager(BlockGenerator blockGenerator)
    {
        //BlockGenerator���擾
        this.blockGenerator = blockGenerator;
    }

    /// <summary>
    /// ���݃A�N�e�B�u�ȃu���b�N�̓������~�܂������ɌĂяo�����
    /// </summary>
    public void StoppedCurrentBlock()
    {
        //4��J��Ԃ�
        for (int i = 0; i < 4; i++)
        {
            //���݃A�N�e�B�u�ȃu���b�N�̑�0�������X�g�ɒǉ�
            cubeList.Add(currentBlock.transform.GetChild(0).transform.GetChild(0).gameObject);

            //���݃A�N�e�B�u�ȃu���b�N�̑�0���̐e�����g�ɐݒ�
            currentBlock.transform.GetChild(0).transform.GetChild(0).transform.SetParent(transform);
        }

        //���n��̃u���b�N��BlockController�𖳌���
        currentBlock.enabled = false;

        //�X�e�[�W�ɒ~�ς���Ă��闧���̂̐������J��Ԃ�
        for (int j = 0; j < cubeList.Count; j++)
        {
            //���̗����̂����E���C���𒴂��Ă�����
            if (cubeList[j].transform.position.y > 20.5f)
            {
                //�Q�[���I�[�o�[��Ԃɐ؂�ւ���
                isGameOver = true;

                //�ȍ~�̏������s��Ȃ�
                return;
            }
        }

        //�����̂̏������s�����m�F����
        CheckDigested();

        //�u���b�N��1�x�������A���������u���b�N�̏����擾
        currentBlock = blockGenerator.GenerateBlock();

        //�S�[�X�g�̐����������s��
        PrepareMakeGhost();
    }

    /// <summary>
    /// �����̂̏������s�����ǂ����m�F���A�ꍇ�ɉ����ď������s��
    /// </summary>
    private void CheckDigested()
    {
        //����������
        int digestedCount = 0;

        //20��J��Ԃ�
        for (int i = 1; i < 21; i++)
        {
            //����y���W�̗����̂̃��X�g���쐬
            List<GameObject> samePosYList = cubeList.FindAll(x => (x.transform.position.y > (i - 0.5f)) && (x.transform.position.y < (i + 0.5f)));

            //����y���W�̗����̂̐���10��菬����������i����񂪑����Ă��Ȃ�������j
            if (samePosYList.Count < 10)
            {
                //���̌J��Ԃ������ֈڂ�
                continue;
            }

            //�������I����Ă��Ȃ���Ԃɐ؂�ւ���
            endDigestion = false;

            //10��J��Ԃ�
            for (int j = 0; j < 10; j++)
            {
                //�Ώۂ̗����̂��擾
                GameObject cube = samePosYList[0];

                //�Ώۂ̗����̂�cubeList�����菜��
                cubeList.Remove(cubeList.Find(x => x == cube));

                //�Ώۂ̗����̂�samePosYList�����菜��
                samePosYList.RemoveAt(0);

                //�Ώۂ̗����̂�����
                Destroy(cube);
            }

            //���������񐔂��L�^
            digestedCount++;

            //�X�e�[�W�ɒ~�ς���Ă��闧���̂̐������J��Ԃ�
            for (int k = 0; k < cubeList.Count; k++)
            {
                //�������ꂽ�����ɂ��闧���̂Ȃ�
                if (cubeList[k].transform.position.y > i)
                {
                    //���������񐔂�������������
                    cubeList[k].transform.DOMoveY(cubeList[k].transform.position.y - digestedCount, 0.5f).
                        
                        //�A�j���[�V�������I������A�����I����Ԃɐ؂�ւ���
                        OnComplete(()=>endDigestion=true);
                }
            }
        }

        //1�x�ł��������s��ꂽ�Ȃ�
        if (digestedCount > 0)
        {
            //���ʉ����Đ�
            SoundManager.instance.PlaySound(SoundDataSO.SoundName.DigestionSE);

            //���_�̕\�����X�V
            UIManager.instance.UpdateTxtScore(GameData.instance.ScorePerColumn * digestedCount);
        }
    }

    /// <summary>
    /// �u���b�N�̕ۑ��E�g�p���s��
    /// </summary>
    /// <param name="blockData">�Ăяo�����̃u���b�N�̃f�[�^</param>
    public void HoldBlock(BlockDataSO.BlockData blockData)
    {
        //���݃A�N�e�B�u�ȃu���b�N������
        Destroy(currentBlock.gameObject);

        //�ۑ�����Ă���u���b�N���Ȃ����
        if (holdBlockData == null)
        {
            //�u���b�N�̃f�[�^��ۑ�
            holdBlockData = blockData;

            //�ۑ�����Ă���u���b�N�̕\����ݒ�
            UIManager.instance.SetImgHoldBllock(blockData.sprite);

            //�u���b�N��1�x�������A���������u���b�N�̏����擾
            currentBlock = blockGenerator.GenerateBlock();
        }
        //�ۑ�����Ă���u���b�N�������
        else
        {
            //�ۑ�����Ă���u���b�N�̃C���[�W����ɂ���
            UIManager.instance.ClearImgHoldBlock();

            //�ۑ�����Ă���u���b�N�𐶐�����
            currentBlock = blockGenerator.GenerateBlock(holdBlockData);

            //�ۑ�����Ă���u���b�N�̃f�[�^����ɂ���
            holdBlockData = null;
        }

        //�S�[�X�g�̐����������s��
        PrepareMakeGhost();
    }

    /// <summary>
    /// �S�[�X�g�̐����������s��
    /// </summary>
    public void PrepareMakeGhost()
    {
        //�S�[�X�g�𐶐�����
        StartCoroutine(MakeGhost());
    }

    /// <summary>
    /// �S�[�X�g�𐶐�����
    /// </summary>
    /// <returns>�҂�����</returns>
    private IEnumerator MakeGhost()
    {
        //���ɃS�[�X�g�����݂��Ă���Ȃ�inull�G���[����j
        if (ghostObj != null)
        {
            //�����ɌJ��Ԃ�
            while(true)
            {
                //�S�[�X�g���������I������
                if (!ghostObj.TryGetComponent(out GhostController _))
                {
                    //�J��Ԃ��������甲���o��
                    break;
                }

                //���̃t���[���֔�΂��i�����AUpdate���\�b�h�j
                yield return null;
            }

            //���̃S�[�X�g������
            Destroy(ghostObj.gameObject);
        }

        //MeshRenderer�̃��X�g
        List<MeshRenderer> meshRenderersList = new();

        //�S�[�X�g�𐶐�
        BlockController ghost = Instantiate(CurrentBlock);

        //�S�[�X�g�̃Q�[���I�u�W�F�N�g��ێ�
        ghostObj = ghost.gameObject;

        //�S�[�X�g����BlockController����菜��
        Destroy(ghost);

        //4��J��Ԃ�
        for (int i = 0; i < 4; i++)
        {
            //�S�[�X�g�̑�����̃R���C�_�[�̎擾�ɐ���������
            if (ghostObj.transform.GetChild(0).transform.GetChild(i).gameObject.TryGetComponent(out BoxCollider collider))
            {
                //�R���C�_�[��񊈐�������
                collider.enabled = false;
            }
            //�S�[�X�g�̑�����̃R���C�_�[�̎擾�Ɏ��s������
            else
            {
                //�����
                Debug.Log("�S�[�X�g�̑�����̃R���C�_�[�̎擾�Ɏ��s");
            }

            //�S�[�X�g�����MeshRenderer�̎擾�ɐ���������
            if (ghostObj.transform.GetChild(0).transform.GetChild(i).gameObject.TryGetComponent(out MeshRenderer meshRenderer))
            {
                //�S�[�X�g�̃}�e���A����ݒ�
                meshRenderer.material = ghostMaterial;

                //���X�g�ɒǉ�
                meshRenderersList.Add(meshRenderer);

                //�S�[�X�g���\���ɂ���
                meshRenderer.enabled = false;
            }
            //�S�[�X�g�����MeshRenderer�̎擾�Ɏ��s������
            else
            {
                Debug.Log("�S�[�X�g�����MeshRenderer�̎擾�Ɏ��s");
            }
        }

        //���������S�[�X�g��GhostController�����t����
        StartCoroutine(ghostObj.AddComponent<GhostController>()

            //���������S�[�X�g�̏����ݒ���s��
            .SetUpGhost(meshRenderersList));
    }
}
