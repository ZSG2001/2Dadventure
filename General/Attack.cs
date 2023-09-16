using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{    
    public int damage;//攻击伤害
    public float attackRange;//攻击范围
    public float attackRate;//攻击频率
    private void OnTriggerStay2D(Collider2D other)
    {
        //通过other访问被攻击的人
        //需要传递一个attacker类型的参数进去，把当前的attacker传进去(用this)        
        other.GetComponent<Character>()?.TakeDamage(this);
    //如果对方身上没有以上代码可能会报错，所以要加一个?问号,判断对方身上有没有这个代码，如果有就执行
    }
}
