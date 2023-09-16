using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;//�����¼��������ռ�

public class Character : MonoBehaviour
{
    [Header("��������")]
    public float maxHealth;//���Ѫ��
    public float currentHealth;//��ǰѪ��

    [Header("�����޵�")]
    public float invulnerableDuration;
    private float invulnerableCounter;
    public bool invulnerable;

    [Header("�¼�")]
    public UnityEvent<Transform> OnTakeDamage;//�����¼�
    public UnityEvent OnDie;//�����¼�

    private void Start()
    {
        currentHealth = maxHealth;
    }
    private void Update()
    {
        if(invulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            //Time.deltatime�������һ�����õ�ʱ�䣨����Ϊ��λ����ֻ����
            if (invulnerableCounter <= 0 )
            {
                invulnerable = false; 
            }
        }        
    }

    //��һ��Attack���͵�ֵ����������������attacker
    public void TakeDamage(Attack attacker)    
    {
        if (invulnerable)
        {
            return;
            //����޵У�����ִ��return����Ĵ��루���������if-else��
        }
        if (currentHealth - attacker.damage > 0)
            //�����ǰѪ���ܳе�һ���˺���������˺�
        {
            currentHealth -= attacker.damage;
            TriggerInvulnerable();
            OnTakeDamage?.Invoke(attacker.transform);
            //OnTakeDamage������ʱ�ᴫ��һ��transform�Ĳ���,��PlayerController���GetHurt��������յ�
            //����Character���ܻ��attacker.transform��������Ϊ��Attack������
            //��������
        }
        else
        {
            currentHealth = 0;
            OnDie?.Invoke();
            //��������
        }
    }
    private void TriggerInvulnerable()
    {
        if (!invulnerable)
        {
            invulnerable = true;
            invulnerableCounter = invulnerableDuration;
        }
    }
}
