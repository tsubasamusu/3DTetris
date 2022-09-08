using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;//DOTween���g�p

public class BlockManager : MonoBehaviour
{
    public static BlockManager instance;//�C���X�^���X

    [SerializeField]
    private BlockGenerator blockGenerator;//BlockGenerator

    //[HideInInspector]
    public List<GameObject> cubeList=new();//���݁A�X�e�[�W��ɒ~�ς���Ă��闧���̂̃��X�g

    private GameObject currentBlock;//���݃A�N�e�B�u�ȃu���b�N

    private BlockDataSO.BlockData holdBlockData;//�ۑ����ꂽ�u���b�N�̃f�[�^

    /// <summary>
    /// �ۑ����ꂽ�u���b�N�̃f�[�^�̎擾�p
    /// </summary>
    public BlockDataSO.BlockData HoldBlockData
    { get { return holdBlockData; } }

    /// <summary>
    /// ���݃A�N�e�B�u�ȃu���b�N�̐ݒ�p
    /// </summary>
    public GameObject CurrentBlock
    { set { currentBlock = value; } }

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
    /// ���݃A�N�e�B�u�ȃu���b�N�̓������~�܂������ɌĂяo�����
    /// </summary>
    public void StoppedCurrentBlock()
    {
        //4��J��Ԃ�
        for(int i = 0; i < 4; i++)
        {
            //���݃A�N�e�B�u�ȃu���b�N�̑�0�������X�g�ɒǉ�
            cubeList.Add(currentBlock.transform.GetChild(0).transform.GetChild(0).gameObject);

            //���݃A�N�e�B�u�ȃu���b�N�̑�0���̐e�����g�ɐݒ�
            currentBlock.transform.GetChild(0).transform.GetChild(0).transform.SetParent(transform);
        }

        //���݃A�N�e�B�u�ȃu���b�N����BlockController���擾�o������
        if(currentBlock.TryGetComponent(out BlockController blockController))
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
            //�X�e�[�W��ɒ~�ς���Ă��闧���̂�1�ł����E���C���𒴂��Ă�����
            if (cubeList[i].transform.position.y>20.5f)
            {
                //TODO:GameManager����Q�[���I�[�o�[�������Ăяo��

                //�ȍ~�̏������s��Ȃ�
                return;
            }

            //����y���W�̗����̂̃��X�g���쐬
            List<GameObject> samePosYList = cubeList.FindAll(x => (x.transform.position.y > (i - 0.5f)) && (x.transform.position.y < (i + 0.5f)));
            
            //����y���W�̗����̂̐���10��菬����������i����񂪑����Ă��Ȃ�������j
            if(samePosYList.Count< 10)
            {
                //���̌J��Ԃ������ֈڂ�
                continue;
            }

            //10��J��Ԃ�
            for(int j = 0; j < 10; j++)
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
                    cubeList[k].transform.DOMoveY(cubeList[k].transform.position.y - digestedCount, 0.5f);
                }
            }
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

            //�u���b�N��1�x�������A���������u���b�N�̏����擾
            currentBlock = blockGenerator.GenerateBlock();
        }
        //�ۑ�����Ă���u���b�N�������
        else
        {
            //�ۑ�����Ă���u���b�N�𐶐�����
            currentBlock = blockGenerator.GenerateBlock(holdBlockData);

            //�ۑ�����Ă���u���b�N�̃f�[�^����ɂ���
            holdBlockData = null;
        }
    }
}
