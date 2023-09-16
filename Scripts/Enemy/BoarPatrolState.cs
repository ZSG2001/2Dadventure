using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarPatrolState : BaseState
{
    public override void OnEnter(Enemy enemy)//调用Enemy脚本中的数据
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
    }
    public override void LogicUpdate()
    {
        if (currentEnemy.FoundPlayer())//发现敌人，切换追击状态
        {
            currentEnemy.SwitchState(NPCState.Chase);            
        }
        if (!currentEnemy.physicsCheck.isGround|| (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x < 0) || (currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x > 0))
        {
            currentEnemy.wait = true;            
            currentEnemy.rb.velocity = new Vector2(0,currentEnemy.rb.velocity.y);
            currentEnemy.anim.SetBool("walk", false);
        }
        else
        {
            currentEnemy.anim.SetBool("walk", true);
        }        
        if (currentEnemy.isHurt && currentEnemy.wait)
        {
            currentEnemy.wait = false;
            currentEnemy.waitTimeCounter = currentEnemy.waitTime;
        }
    }
    public override void PhysicsUpdate()
    {
        
    }
    public override void OnExit()
    {
        currentEnemy.anim.SetBool("walk", false);        
    }
}
