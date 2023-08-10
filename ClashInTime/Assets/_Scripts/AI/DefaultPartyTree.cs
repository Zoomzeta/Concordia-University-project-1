using System.Collections.Generic;
using BehaviourTree;
using Tasks;
using Checks;
using UnityEngine;

public class DefaultPartyTree : Selector
{
    public DefaultPartyTree(Transform transform, Character _char, GameController gameCon, string tag, BasicAnimationController _anim) : base()
    {
        children.Add(new Sequence(new List<Node>
        {

            new CheckEnemyInMoveRange(transform, gameCon, TargetPriority.LOWHEALTH, tag),
            new Selector(new List<Node>
            {
                new TaskCharge(_char),
                new TaskFlee(_char, _anim)
            })
        }));
        children.Add(new TaskMoveToSocket(_char, _anim));
    }
}