public abstract class BaseState 
{
    protected Enemy currentEnemy;//����Enemy�ű��е�����
    public abstract void OnEnter(Enemy enemy);
    public abstract void LogicUpdate();//�߼��жϣ�update���
    public abstract void PhysicsUpdate();//�����жϣ�fixedupdate���
    public abstract void OnExit();    
}

