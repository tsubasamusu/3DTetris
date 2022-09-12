using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;//DOTween���g�p

public class BlockManager : MonoBehaviour
{
    public static BlockManager instance;//�C���X�^���X

    [SerializeField]
    private BlockGenerator blockGenerator;//BlockGenerator

    private GameManager gameManager;//GameManager

    [SerializeField]
    private Material ghostMaterial;//�S�[�X�g�p�̃}�e���A��

    [HideInInspector]
    public List<GameObject> cubeList = new();//���݁A�X�e�[�W��ɒ~�ς���Ă��闧���̂̃��X�g

    private GameObject currentBlock;//���݃A�N�e�B�u�ȃu���b�N

    private BlockDataSO.BlockData holdBlockData;//�ۑ����ꂽ�u���b�N�̃f�[�^

    private GameObject ghost;//�S�[�X�g

    private bool endDigestion=true;//�����̏������I��������ǂ���

    /// <summary>
    /// �����I������擾�p
    /// </summary>
    public bool EndDigestion
    { get { return endDigestion; } }

    /// <summary>
    /// �ۑ����ꂽ�u���b�N�̃f�[�^�̎擾�p
    /// </summary>
    public BlockDataSO.BlockData HoldBlockData
    { get { return holdBlockData; } }

    /// <summary>
    /// ���݃A�N�e�B�u�ȃu���b�N�̎擾�E�ݒ�p
    /// </summary>
    public GameObject CurrentBlock
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
    /// <param name="gameManager">GameManager</param>
    public void SetUpBlockManager(GameManager gameManager)
    {
        //GameManager���擾
        this.gameManager = gameManager;
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

        //���݃A�N�e�B�u�ȃu���b�N����BlockController���擾�o������
        if (currentBlock.TryGetComponent(out BlockController blockController))
        {
            //BlockController��񊈐����i���ʂȏ�����h���j
            blockController.enabled = false;
        }
        //���݃A�N�e�B�u�ȃu���b�N����BlockController���擾�o���Ȃ�������
        else
        {
            //�����
            Debug.Log("���݃A�N�e�B�u�ȃu���b�N�����BlockController�̎擾�Ɏ��s");
        }

        //�X�e�[�W�ɒ~�ς���Ă��闧���̂̐������J��Ԃ�
        for (int j = 0; j < cubeList.Count; j++)
        {
            //���̗����̂����E���C���𒴂��Ă�����
            if (cubeList[j].transform.position.y > 20.5f)
            {
                //�Q�[���I�[�o�[�����̏������s��
                gameManager.PrepareGameOver();

                //�ȍ~�̏������s��Ȃ�
                return;
            }
        }

        //�����̂̏������s�����m�F����
        CheckDigested();

        //�u���b�N��1�x�������A���������u���b�N�̏����擾
        currentBlock = blockGenerator.GenerateBlock();
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
        Destroy(currentBlock);

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
    }

    /// <summary>
    /// �u���b�N�̃S�[�X�g���쐬����
    /// </summary>
    public void MakeGhost()
    {
        //���ɃS�[�X�g�����݂��Ă���Ȃ�
        if (ghost != null)
        {
            //���̃S�[�X�g������
            Destroy(ghost);
        }

        //MeshRenderer�̃��X�g
        List<MeshRenderer> meshRenderersList = new();

        ///�S�[�X�g�𐶐�
        ghost = Instantiate(CurrentBlock);

        //�S�[�X�g�����BlockController�̎擾�ɐ���������
        if (ghost.TryGetComponent(out BlockController blockController))
        {
            //BlockController��񊈐�������
            blockController.enabled = false;
        }
        //�S�[�X�g�����BlockController�̎擾�Ɏ��s������
        else
        {
            //�����
            Debug.Log("�S�[�X�g�����BlockController�̎擾�Ɏ��s");
        }

        //4��J��Ԃ�
        for (int i = 0; i < 4; i++)
        {
            //�S�[�X�g�̑�����̃R���C�_�[�̎擾�ɐ���������
            if (ghost.transform.GetChild(0).transform.GetChild(i).gameObject.TryGetComponent(out BoxCollider collider))
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
            if (ghost.transform.GetChild(0).transform.GetChild(i).gameObject.TryGetComponent(out MeshRenderer meshRenderer))
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
        ghost.AddComponent<GhostController>()
            
            //���������S�[�X�g�̏����ݒ���s��
            .SetUpGhost(meshRenderersList);
    }
}
