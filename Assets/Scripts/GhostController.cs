using System.Collections.Generic;//���X�g���g�p
using UnityEngine;

public class GhostController : MonoBehaviour
{
    //MeshRenderer�̃��X�g
    List<MeshRenderer> meshRenderersList = new();

    /// <summary>
    /// ���t���[���Ăяo�����
    /// </summary>
    private void Update()
    {
        //�������̑��̃u���b�N�ɐG��Ă��Ȃ��Ȃ�
        if (!CheckContactedDown())
        {
            //�ȍ~�̏������s��Ȃ�
            return;
        }

        //���g��K�؂Ȉʒu�Ɉړ�������
        SetMeRightPos();

        //MeshRenderer�̃��X�g�̗v�f�������J��Ԃ�
        for (int i = 0; i < meshRenderersList.Count; i++)
        {
            //MeshRenderer������������
            meshRenderersList[i].enabled = true;
        }

        //���g��GhostController�̎擾�ɐ���������
        if (TryGetComponent(out GhostController ghostController))
        {
            //GhostController��񊈐�������
            ghostController.enabled = false;
        }
        //���g��GhostController�̎擾�Ɏ��s������
        else
        {
            //�����
            Debug.Log("���g��GhostController�̎擾�Ɏ��s");
        }
    }

    /// <summary>
    /// ��莞�Ԃ��ƂɌĂяo�����
    /// </summary>
    private void FixedUpdate()
    {
        //�������̑��̃u���b�N�ɐG�ꂽ��
        if (CheckContactedDown())
        {
            //�ȍ~�̏������s��Ȃ�
            return;
        }

        //�u���b�N�𗎉�������
        transform.Translate(new Vector3(0f, -GameData.instance.GhostFallSpeed, 0f));
    }

    /// <summary>
    /// �S�[�X�g���g�̏����ݒ���s��
    /// </summary>
    /// <param name="meshRenderersList"></param>
    public void SetUpGhost(List<MeshRenderer> meshRenderersList)
    {
        //MeshRenderer�̃��X�g��ݒ�
        this.meshRenderersList = meshRenderersList;
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

            //���݃A�N�e�B�u�ȃu���b�N�ȊO�̃R���C�_�[�ɁA�������ڐG������
            if (Physics.Raycast(ray,out RaycastHit hit, 0.5f)&&hit.transform.root.gameObject!=BlockManager.instance.CurrentBlock)
            {
                //true��Ԃ�
                return true;
            }
        }

        //false��Ԃ�
        return false;
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
