using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{    
    public int damage;//�����˺�
    public float attackRange;//������Χ
    public float attackRate;//����Ƶ��
    private void OnTriggerStay2D(Collider2D other)
    {
        //ͨ��other���ʱ���������
        //��Ҫ����һ��attacker���͵Ĳ�����ȥ���ѵ�ǰ��attacker����ȥ(��this)        
        other.GetComponent<Character>()?.TakeDamage(this);
    //����Է�����û�����ϴ�����ܻᱨ������Ҫ��һ��?�ʺ�,�ж϶Է�������û��������룬����о�ִ��
    }
}
