using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;//引入事件的命名空间

public class Character : MonoBehaviour
{
    [Header("基本属性")]
    public float maxHealth;//最大血量
    public float currentHealth;//当前血量

    [Header("受伤无敌")]
    public float invulnerableDuration;
    private float invulnerableCounter;
    public bool invulnerable;

    [Header("事件")]
    public UnityEvent<Transform> OnTakeDamage;//受伤事件
    public UnityEvent OnDie;//死亡事件

    private void Start()
    {
        currentHealth = maxHealth;
    }
    private void Update()
    {
        if(invulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            //Time.deltatime，完成上一秒所用的时间（以秒为单位）（只读）
            if (invulnerableCounter <= 0 )
            {
                invulnerable = false; 
            }
        }        
    }

    //把一个Attack类型的值传进来，起名叫作attacker
    public void TakeDamage(Attack attacker)    
    {
        if (invulnerable)
        {
            return;
            //如果无敌，不会执行return后面的代码（包括后面的if-else）
        }
        if (currentHealth - attacker.damage > 0)
            //如果当前血量能承担一次伤害，便接受伤害
        {
            currentHealth -= attacker.damage;
            TriggerInvulnerable();
            OnTakeDamage?.Invoke(attacker.transform);
            //OnTakeDamage被激活时会传入一个transform的参数,在PlayerController里的GetHurt函数会接收到
            //而在Character中能获得attacker.transform，则是因为与Attack的联动
            //触发受伤
        }
        else
        {
            currentHealth = 0;
            OnDie?.Invoke();
            //触发死亡
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
