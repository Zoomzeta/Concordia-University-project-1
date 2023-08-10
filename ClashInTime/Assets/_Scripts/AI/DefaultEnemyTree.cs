using System.Collections.Generic;
using BehaviourTree;
using Tasks;
using Checks;
using UnityEngine;

public class DefaultEnemyTree : Selector
{
    public DefaultEnemyTree(Transform transform, Character _char, GameController gameCon, string tag, BasicAnimationController _anim) : base()
    {
        children.Add(new Sequence(new List<Node>
        {
            new CheckEnemyInMoveRange(transform, gameCon, TargetPriority.HIGHHEALTH, tag),
            new Selector(new List<Node>
            {
                new TaskCharge(_char),
                new TaskWalk(_char)
            })
        }));
        children.Add(new TaskWander(_char));
    }
}