using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;//�C���X�^���X

    private int score;//���_

    [SerializeField]
    private float normalFallSpeed;//�u���b�N�̍~�����x�i���ʁj

    [SerializeField]
    private float specialFallSpeed;//�u���b�N�̍~�����x�i���ʁj

    [SerializeField]
    private float timeLimit;//��������

    [SerializeField, Header("�u���b�N�̐����\��̐�")]
    private int appointmentsNumber;//�u���b�N�̐����\��̐�

    /// <summary>
    /// ���_�擾�p
    /// </summary>
    public int Score
    { get { return score; } }

    /// <summary>
    /// �u���b�N�̍~�����x�i���ʁj�擾�p
    /// </summary>
    public float NormalFallSpeed
    { get { return normalFallSpeed; } }

    /// <summary>
    /// �u���b�N�̍~�����x�i���ʁj�擾�p
    /// </summary>
    public float SpecialFallSpeed
    { get { return specialFallSpeed; } }

    /// <summary>
    /// �������Ԏ擾�p
    /// </summary>
    public float TimeLimit
    { get { return timeLimit; } }

    /// <summary>
    /// �u�u���b�N�̐����\��̐��v�擾�p
    /// </summary>
    public int AppointmentsNumber
    { get { return appointmentsNumber; } }

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
}
