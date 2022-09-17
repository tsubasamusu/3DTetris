using System.Collections.Generic;//���X�g���g�p
using System.Collections;//IEnumerator���g�p
using UnityEngine;

public class GhostController : MonoBehaviour
{
    //MeshRenderer�̃��X�g
    List<MeshRenderer> meshRenderersList = new();

    /// <summary>
    /// �S�[�X�g���g�̏����ݒ���s��
    /// </summary>
    /// <param name="meshRenderersList">���g��MeshRenderer�̃��X�g</param>
    /// <returns>�҂�����</returns>
    public IEnumerator SetUpGhost(List<MeshRenderer> meshRenderersList)
    {
        //MeshRenderer�̃��X�g��ݒ�
        this.meshRenderersList = meshRenderersList;

        //���g�̃u���b�N�̏����擾
        BlockDataSO.BlockData myBlockData = BlockManager.instance.CurrentBlock.BlockData;

        //�K�؂�y���W���擾
        float posY = myBlockData.isEvenWidth ? 25.5f : 25f;

        //���g��K�؂ȍ~���ʒu�Ɉړ�������
        transform.position = new Vector3(transform.position.x, posY, 0f);

        //�u���b�N�̏������I���܂ő҂�
        yield return new WaitUntil(() => BlockManager.instance.EndDigestion);

        //�����ɌJ��Ԃ�
        while(true)
        {
            //�������̃u���b�N�ɐG��邩�A�u���b�N�����蔲���čŉ��w�ɍs���Ă��܂�����
            if(CheckContactedDown()||transform.position.y<0f)
            {
                //�J��Ԃ��������甲���o��
                break;
            }

            //�S�[�X�g�𗎉�������
            transform.Translate(0f, -1f, 0f);
        }

        //���n��̏������s��
        LandingMe();
    }

    /// <summary>
    /// �������̑��̃u���b�N�ɐڐG�������ǂ������ׂ�
    /// </summary>
    /// <returns>�������̑��̃u���b�N�ɐڐG������true</returns>
    private bool CheckContactedDown()
    {
        //4��J��Ԃ�
        for (int i = 0; i < 4; i++)
        {
            //������̌������쐬
            Ray ray = new(transform.GetChild(0).transform.GetChild(i).transform.position, Vector3.down);

            //���݃A�N�e�B�u�ȃu���b�N�ȊO�̃R���C�_�[�Ɍ������ڐG������
            if (Physics.Raycast(ray,out RaycastHit hit, 0.6f)&&hit.transform.gameObject!=BlockManager.instance.CurrentBlock.gameObject)
            {
                //true��Ԃ�
                return true;
            }
        }

        //false��Ԃ�
        return false;
    }

    /// <summary>
    /// ���n��̏������s��
    /// </summary>
    private void LandingMe()
    {
        //���g��K�؂Ȉʒu�Ɉړ�������
        SetMeRightPos();

        //MeshRenderer�̃��X�g�̗v�f�������J��Ԃ�
        for (int i = 0; i < meshRenderersList.Count; i++)
        {
            //MeshRenderer������������
            meshRenderersList[i].enabled = true;
        }

        //GhostController������
        Destroy(this);
    }

    /// <summary>
    /// ���n��Ɏ��g��K�؂Ȉʒu�Ɉړ�������
    /// </summary>
    private void SetMeRightPos()
    {
        //���g��y���W�̏��������i�덷�j���擾
        float excess = transform.position.y % 0.5f;

        //�덷���C�����邽�߂̒l���擾
        float valueY = excess < 0.25 ? -excess : 0.5f - excess;

        //���W���Đݒ�
        transform.position = new Vector3(transform.position.x, transform.position.y + valueY, 0f);
    }
}
