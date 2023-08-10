using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using Tasks;
using Checks;

public class ArcherTree : Tree
{
    protected override Node SetupTree()
    {
        string tag = gameObject.tag;
        Node root;

        if(tag == "Party")
        {
            root = new Selector(new List<Node>{
                new Sequence(new List<Node>
                {
                    new CheckIsBattle(_gameCon),
                    new CheckIsMyTurn(transform),
                    new Selector(new List<Node>
                    {
                        new TaskPass(_charAnim, _char),
                        new TaskReload(_char),
                        new Sequence(new List<Node>
                        {
                            new CheckAttackTarget(TargetPriority.HIGHHEALTH, tag),
                            new TaskAttackTarget(_charAnim, _char)
                        })
                    })
                }),
                new DefaultPartyTree(transform, _char, _gameCon, tag, _charAnim)
            });
        }else if(tag == "Enemy")
        {
            root = new Selector(new List<Node>{
                new Sequence(new List<Node>
                {
                    new CheckIsBattle(_gameCon),
                    new CheckIsMyTurn(transform),
                    new Selector(new List<Node>
                    {
                        new TaskPass(_charAnim, _char),
                        new TaskReload(_char),
                        new Sequence(new List<Node>
                        {
                            new CheckAttackTarget(TargetPriority.LOWHEALTH, tag),
                            new TaskAttackTarget(_charAnim, _char)
                        })
                    })
                }),
                new DefaultEnemyTree(transform, _char, _gameCon, tag, _charAnim)
            });
        }else
        {
            root = new Node();
        }
        return root;
    }
}
