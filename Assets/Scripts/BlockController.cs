using UnityEngine;

public class BlockController : MonoBehaviour
{
    private float currentFallSpeed;//�u���b�N�̍��̍~�����x

    private Transform mainCameraTran;//���C���J�����̈ʒu���

    private BlockDataSO.BlockData myBlockData;//�����̃u���b�N�̃f�[�^

    private bool setUp;//�����ݒ肪�I��������ǂ���

    /// <summary>
    /// ���g�̃u���b�N�̏��̎擾�p
    /// </summary>
    public BlockDataSO.BlockData BlockData
    { get { return myBlockData; } }

    /// <summary>
    /// BlockController�̏����ݒ���s��
    /// </summary>
    /// <param name="blockGenerator">BlockGenerator</param>
    /// <param name="blockData">�u���b�N�̃f�[�^</param>
    public void SetUpBlockController(BlockDataSO.BlockData blockData)
    {
        //���g�̃f�[�^���擾
        myBlockData = blockData;

        //���C���J�����̈ʒu�����擾
        mainCameraTran = Camera.main.transform;

        //�����ݒ芮����Ԃɐ؂�ւ���
        setUp = true;
    }

    /// <summary>
    /// ���t���[���Ăяo�����
    /// </summary>
    private void Update()
    {
        //�����ݒ肪�I����Ă��Ȃ��Ȃ�
        if(!setUp)
        {
            //�ȍ~�̏������s��Ȃ�
            return;
        }

        //�������̑��̃u���b�N�ɐG�ꂽ��
        if (CheckContactedDown())
        {
            //���g��K�؂Ȉʒu�Ɉړ�������
            SetMeRightPos();

            //BlockManager����K�؂ȏ������Ăяo��
            BlockManager.instance.StoppedCurrentBlock();

            //�ȍ~�̏������s��Ȃ�
            return;
        }

        //�u���b�N�̗������x��ݒ�
        currentFallSpeed = Input.GetKey(KeyCode.DownArrow) ? GameData.instance.SpecialFallSpeed : GameData.instance.NormalFallSpeed;

        //�u���b�N�𗎉�������
        transform.Translate(0f, -(currentFallSpeed * Time.deltaTime), 0f);

        //�E��󂪉����ꂽ��
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //�J�������猩�ĉE�ɂ��āA���̃R���C�_�[�ɐG��Ă�����
            if ((mainCameraTran.position.z < 0f && CheckContactedRight()) || (mainCameraTran.position.z >= 0f && CheckContactedLeft()))
            {
                //�ȍ~�̏������s��Ȃ�
                return;
            }

            //�J�����̈ʒu�ɉ����Ĉړ�������ݒ�
            float moveValue = mainCameraTran.position.z < 0f ? 1f : -1f;

            //�J�������猩�ĉE�Ɉړ�����
            transform.Translate(new Vector3(moveValue, 0f, 0f));

            //�S�[�X�g�̐����������s��
            BlockManager.instance.PrepareMakeGhost();
        }
        //����󂪉����ꂽ��
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //�J�������猩�č��ɂ��āA���̃R���C�_�[�ɐG��Ă�����
            if ((mainCameraTran.position.z < 0f && CheckContactedLeft()) || (mainCameraTran.position.z >= 0f && CheckContactedRight()))
            {
                //�ȍ~�̏������s��Ȃ�
                return;
            }

            //�J�����̈ʒu�ɉ����Ĉړ�������ݒ�
            float moveValue = mainCameraTran.position.z < 0f ? -1f : 1f;

            //�J�������猩�č��Ɉړ�����
            transform.Translate(new Vector3(moveValue, 0f, 0f));

            //�S�[�X�g�̐����������s��
            BlockManager.instance.PrepareMakeGhost();
        }

        //���󂪉����ꂽ��
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //�u���b�N�̕ۑ��E�g�p���s��
            BlockManager.instance.HoldBlock(myBlockData);
        }

        //���g����]�ł��Ȃ����W�ɂ�����
        if (Mathf.Abs(transform.position.x) > (5f - myBlockData.rotLength) || transform.position.y < (0.5f + myBlockData.rotLength) || !CheckLengthToOtherCube())
        {
            //�v���C���[���u���b�N����]�����悤�Ƃ�����
            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
            {
                //���ʉ����Đ�
                SoundManager.instance.PlaySound(SoundDataSO.SoundName.CannotRotSE);
            }

            //�ȍ~�̏������s��Ȃ�
            return;
        }

        //���N���b�N���ꂽ��
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //�J�����̈ʒu�ɉ����ĉ�]������ݒ�
            float rotateValue = mainCameraTran.position.z < 0 ? 90f : -90f;

            //�J�������猩�Ĕ����v���ɉ�]������
            transform.GetChild(0).transform.eulerAngles = new Vector3(0f, 0f, transform.GetChild(0).transform.eulerAngles.z + rotateValue);

            //�S�[�X�g�̐����������s��
            BlockManager.instance.PrepareMakeGhost();
        }
        //�E�N���b�N���ꂽ��
        else if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            //�J�����̈ʒu�ɉ����ĉ�]������ݒ�
            float rotateValue = mainCameraTran.position.z < 0 ? -90f : 90f;

            //�J�������猩�Ď��v���ɉ�]������
            transform.GetChild(0).transform.eulerAngles = new Vector3(0f, 0f, transform.GetChild(0).transform.eulerAngles.z + rotateValue);

            //�S�[�X�g�̐����������s��
            BlockManager.instance.PrepareMakeGhost();
        }
    }

    /// <summary>
    /// ���ʂ��猩�ĉE�����̑��̃u���b�N�ɐڐG�������ǂ������ׂ�
    /// </summary>
    /// <returns>���ʂ��猩�ĉE�����̑��̃u���b�N�ɐڐG������true</returns>
    private bool CheckContactedRight()
    {
        //���g�̑��̐������J��Ԃ�
        for (int i = 0; i < transform.GetChild(0).transform.childCount; i++)
        {
            //������̌������쐬
            Ray ray = new(transform.GetChild(0).transform.GetChild(i).transform.position, new Vector3(1f, 0f, 0f));

            //���������̃R���C�_�[�ɐڐG���Ȃ�������
            if (!Physics.Raycast(ray,out RaycastHit hit,0.5f))
            {
                //���̌J��Ԃ������ֈڂ�
                continue;
            }

            //�G�ꂽ���肪���ł͂Ȃ�������
            int isNotGrandchildCount = 0;

            //���g�̑��̐������J��Ԃ�
            for (int j = 0; j < transform.GetChild(0).transform.childCount; j++)
            {
                //�G�ꂽ���肪���g�̑���1�l�Ȃ�
                if (hit.transform.gameObject == transform.GetChild(0).transform.GetChild(j).gameObject)
                {
                    //���̌J��Ԃ������ֈڂ�
                    continue;
                }

                //�񐔂��L�^
                isNotGrandchildCount++;

                //�G�ꂽ���肪���g�̑S�Ă̑��ȊO�Ȃ�
                if (isNotGrandchildCount == transform.GetChild(0).transform.childCount)
                {
                    //true��Ԃ�
                    return true;
                }
            }
        }

        //false��Ԃ�
        return false;
    }

    /// <summary>
    /// ���ʂ��猩�č������̑��̃u���b�N�ɐڐG�������ǂ������ׂ�
    /// </summary>
    /// <returns>���ʂ��猩�č������̑��̃u���b�N�ɐڐG������true</returns>
    private bool CheckContactedLeft()
    {
        //���g�̑��̐������J��Ԃ�
        for (int i = 0; i < transform.GetChild(0).transform.childCount; i++)
        {
            //������̌������쐬
            Ray ray = new(transform.GetChild(0).transform.GetChild(i).transform.position, new Vector3(-1f, 0f, 0f));

            //���������̃R���C�_�[�ɐڐG���Ȃ�������
            if (!Physics.Raycast(ray, out RaycastHit hit, 0.5f))
            {
                //���̌J��Ԃ������ֈڂ�
                continue;
            }

            //�G�ꂽ���肪���ł͂Ȃ�������
            int isNotGrandchildCount = 0;

            //���g�̑��̐������J��Ԃ�
            for (int j = 0; j < transform.GetChild(0).transform.childCount; j++)
            {
                //�G�ꂽ���肪���g�̑���1�l�Ȃ�
                if (hit.transform.gameObject == transform.GetChild(0).transform.GetChild(j).gameObject)
                {
                    //���̌J��Ԃ������ֈڂ�
                    continue;
                }

                //�񐔂��L�^
                isNotGrandchildCount++;

                //�G�ꂽ���肪���g�̑S�Ă̑��ȊO�Ȃ�
                if (isNotGrandchildCount == transform.GetChild(0).transform.childCount)
                {
                    //true��Ԃ�
                    return true;
                }
            }
        }

        //false��Ԃ�
        return false;
    }

    /// <summary>
    /// �������̑��̃u���b�N�ɐڐG�������ǂ������ׂ�
    /// </summary>
    /// <returns>�������̑��̃u���b�N�ɐڐG������true</returns>
    private bool CheckContactedDown()
    {
        //���g�̑��̐������J��Ԃ�
        for (int i = 0; i < transform.GetChild(0).transform.childCount; i++)
        {
            //������̌������쐬
            Ray ray = new(transform.GetChild(0).transform.GetChild(i).transform.position, Vector3.down);

            //���������̃R���C�_�[�ɐڐG���Ȃ�������
            if (!Physics.Raycast(ray, out RaycastHit hit, 0.7f))
            {
                //���̌J��Ԃ������ֈڂ�
                continue;
            }

            //�G�ꂽ���肪���ł͂Ȃ�������
            int isNotGrandchildCount = 0;

            //���g�̑��̐������J��Ԃ�
            for (int j = 0; j < transform.GetChild(0).transform.childCount; j++)
            {
                //�G�ꂽ���肪���g�̑���1�l�Ȃ�
                if (hit.transform.gameObject == transform.GetChild(0).transform.GetChild(j).gameObject)
                {
                    //���̌J��Ԃ������ֈڂ�
                    continue;
                }

                //�񐔂��L�^
                isNotGrandchildCount++;

                //�G�ꂽ���肪���g�̑S�Ă̑��ȊO�Ȃ�
                if (isNotGrandchildCount == transform.GetChild(0).transform.childCount)
                {
                    //true��Ԃ�
                    return true;
                }
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
        //���g�̑��̐������J��Ԃ�
        for (int i = 0; i < transform.GetChild(0).transform.childCount; i++)
        {
            //���g�̑���1�l����̌������쐬
            Ray ray = new(transform.GetChild(0).transform.GetChild(i).transform.position, Vector3.down);

            //���������̃R���C�_�[�ɐڐG���Ȃ�������
            if (!Physics.Raycast(ray, out RaycastHit hit, 0.7f))
            {
                //���̌J��Ԃ������ֈڂ�
                continue;
            }

            //�G�ꂽ���肪���g�̑���1�l�Ȃ�
            if (hit.transform.gameObject == transform.GetChild(0).transform.GetChild(i).gameObject)
            {
                //���̌J��Ԃ������ֈڂ�
                continue;
            }

            //���g�̐^���ɂ��闧���̂Ƃ̋������擾
            float length = Mathf.Abs(transform.GetChild(0).transform.GetChild(i).transform.position.y - hit.transform.position.y);

            //���W���Đݒ�
            transform.position = new Vector3(transform.position.x, transform.position.y + (1f-length), 0f);
        }
    }

    /// <summary>
    /// ���̗����̂Ƃ̋������m�F����
    /// </summary>
    /// <returns>���̗����̂Ƃ̋������m�F���������ŉ�]�\�Ȃ�true</returns>
    private bool CheckLengthToOtherCube()
    {
        //���݃X�e�[�W�ɒ~�ς���Ă��闧���̂̐������J��Ԃ�
        for(int i = 0; i < BlockManager.instance.cubeList.Count; i++)
        {
            //���̗����̂Ƃ�x�����̋������߂����邩�A
            if(Mathf.Abs(BlockManager.instance.cubeList[i].transform.position.x-transform.position.x)<(myBlockData.rotLength+1)&&

                //���̗����̂Ƃ�y�����̋������߂�������
                Mathf.Abs(BlockManager.instance.cubeList[i].transform.position.y - transform.position.y)< (myBlockData.rotLength+1))
            {
                //false��Ԃ�
                return false;
            }
        }

        //true��Ԃ�
        return true;
    }
}
