using System;
public abstract class DTNode
{
    public string Name { protected set; get; }
    public abstract void Run();
}

class DTCondition : DTNode
{
    private DTNode _left, _right;
    private Func<bool> condition;

    public override void Run()
    {
        if (condition())
            _left.Run();
        else
            _right.Run();
    }
   
    public DTCondition(string conditionName, Func<bool> function, DTNode left, DTNode right)
    {
        Name = conditionName;
        condition = function;
        _left = left;
        _right = right;
    }
}

class DTAction : DTNode
{
    private Action _action;

    public override void Run()
    {
        _action();
    }

    public DTAction(string actionName, Action action)
    {
        Name = actionName;
        _action = action;
    }
}

