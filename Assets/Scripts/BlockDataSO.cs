using System.Collections;
using System.Collections.Generic;//���X�g���g�p
using UnityEngine;
using System;//Serializable�������g�p

//�A�Z�b�g���j���[�ŁuCreate BlockDataSO�v��I������ƁuBlockDataSO�v���쐬�ł���
[CreateAssetMenu(fileName = "BlockDataSO", menuName = "Create BlockDataSO")]
public class BlockDataSO : ScriptableObject
{
    /// <summary> 
    /// �u���b�N�̖��O
    /// </summary> 
    public enum BlockName
    {
        I,
        J,
        L,
        O,
        S,
        T,
        Z
    }

    /// <summary> 
    ///�u���b�N�̃f�[�^���Ǘ�����
    /// </summary> 
    [Serializable]
    public class BlockData
    {�@�@�@�@�@�@�@�@�@
        public BlockName name;//���O
        public GameObject prefab;//�v���t�@�u
        public Sprite sprite;//�X�v���C�g
        [Header("�I�u�W�F�N�g�̕�������ǂ���")]
        public bool isOddWidth;//�I�u�W�F�N�g�̕�������ǂ���
        public float maxRotPosX;//��]�\��x�̍ő�l
        public float minRotPosX;//��]�\��x�̍ŏ��l
    }

    public List<BlockData> blockDataList = new List<BlockData>();//�u���b�N�̃f�[�^�̃��X�g
}