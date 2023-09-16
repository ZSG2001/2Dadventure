public abstract class BaseState 
{
    protected Enemy currentEnemy;//调用Enemy脚本中的数据
    public abstract void OnEnter(Enemy enemy);
    public abstract void LogicUpdate();//逻辑判断：update相关
    public abstract void PhysicsUpdate();//物理判断：fixedupdate相关
    public abstract void OnExit();    
}

