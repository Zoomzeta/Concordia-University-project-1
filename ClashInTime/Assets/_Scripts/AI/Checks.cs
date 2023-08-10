using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviourTree;

namespace Checks
{
    public enum TargetPriority
    {
        LOWHEALTH,
        HIGHHEALTH
    }

    public class CheckEnemyInMoveRange : Node
    {
        private Transform _transform;
        private GameController _game;
        private TargetPriority _targetting;
        private string _myTag;

        private float maxDetectionRange = 5f;

        public CheckEnemyInMoveRange(Transform transform, GameController game, TargetPriority t, string tag)
        {
            _transform = transform;
            _game = game;
            _targetting = t;
            _myTag = tag;
        }

        public override NodeState Evaluate()
        {
            object enemy = GetData("EnemyMoveTarget");
            if(enemy == null)
            {
                if(_targetting == TargetPriority.LOWHEALTH)
                {
                    GameObject[] targets = (_myTag == "Enemy")? GameObject.FindGameObjectsWithTag("Party"): GameObject.FindGameObjectsWithTag("Enemy");
                    int lowestHealth = int.MaxValue;
                    int lowest = int.MaxValue;

                    for(int i = 0; i < targets.Length; i++)
                    {
                        Character c = targets[i].GetComponent<Character>();
                        Vector3 cPos = targets[i].transform.position;
                        
                        if(Vector3.Distance(_transform.position, cPos) <= maxDetectionRange && c.CurrentHealth < lowestHealth)
                        {
                            lowestHealth = c.CurrentHealth;
                            lowest = i;
                        }
                    }
                    
                    if(lowest < targets.Length)
                        GetRoot().SetData("EnemyMoveTarget", targets[lowest].transform);
                }else
                {
                    GameObject[] targets = (_myTag == "Enemy")? GameObject.FindGameObjectsWithTag("Party"): GameObject.FindGameObjectsWithTag("Enemy");
                    int highestHealth = int.MinValue;
                    int highest = int.MaxValue;

                    for(int i = 0; i < targets.Length; i++)
                    {
                        Character c = targets[i].GetComponent<Character>();
                        Vector3 cPos = targets[i].transform.position;
                        
                        if (Vector3.Distance(_transform.position, cPos) <= maxDetectionRange && c.CurrentHealth > highestHealth)
                        {
                            highestHealth = c.CurrentHealth;
                            highest = i;
                        }
                    }
                    
                    if(highest < targets.Length)
                        GetRoot().SetData("EnemyMoveTarget", targets[highest].transform);
                }
            }
            
            enemy = GetData("EnemyMoveTarget");
            if(enemy != null)
            {
                state = NodeState.SUCCESS;
                return state;
            }

            state = NodeState.FAILURE;
            return state;
        }
    }

    public class CheckIsBattle : Node
    {
        private GameController _game;

        private GameController.BattleController _battle;


        public CheckIsBattle(GameController game)
        {
            _game = game;
        }

        public override NodeState Evaluate()
        {
            _battle = _game.GetBattleController();
            if(_battle == null)
            {
                ClearData("BattleController");//testing
                state = NodeState.FAILURE;
                return state;
            }

            GetRoot().SetData("BattleController", _battle);
            
            state = NodeState.SUCCESS;
            return state;
        }
    }

    public class CheckIsMyTurn : Node
    {
        private Transform _transform;
        public CheckIsMyTurn(Transform trans)
        {
            _transform = trans;
        }

        public override NodeState Evaluate()
        {
            object b = GetData("BattleController");

            if(b != null)
            {
                GameController.BattleController battle = (GameController.BattleController) b;
                if(battle.GetCurrentBattler() == _transform.gameObject)
                {
                    state = NodeState.SUCCESS;
                    return state;
                }
            }

            state = NodeState.FAILURE;
            return state;
        }
    }

    public class CheckOOP : Node
    {

        private Character _character;
        public CheckOOP(Character _char)
        {
            _character = _char;
        }

        public override NodeState Evaluate()
        {
            // if(!_character.IsInPosition())
            // {
            //     GetRoot().SetData("InPos", false);
            //     state = NodeState.SUCCESS;
            //     return state;
            // }

            GetRoot().SetData("InPos", true);
            state = NodeState.FAILURE;
            return state;
        }
    }

    public class CheckHealTarget : Node
    {
        private GameController.BattleController _battle;
        private TargetPriority _targetting;
        private string _myTag;


        public CheckHealTarget(TargetPriority t, string tag)
        {
            _targetting = t;
            _myTag = tag;
        }

        public override NodeState Evaluate()
        {
            object b = GetData("BattleController");

            if(b != null)
            {
                if(_targetting == TargetPriority.LOWHEALTH)
                {
                    _battle = (GameController.BattleController) b;
                    List<Character> targets = (_myTag == "Enemy")? _battle.GetEnemies(): _battle.GetPartyMembers();
                    int lowestHealth = int.MaxValue;
                    int lowest = 0;

                    for(int i = 0; i < targets.Count; i++)
                    {
                        if(targets[i].CurrentHealth < lowestHealth)
                        {
                            lowestHealth = targets[i].CurrentHealth;
                            lowest = i;
                        }
                    }
                    
                    GetRoot().SetData("HealTarget", targets[lowest]);
                    //GetRoot().SetData("healtargetpos", targets[lowest].transform.position);
                    state = NodeState.SUCCESS;
                    return state;
                }else
                {
                    _battle = (GameController.BattleController) b;
                    List<Character> targets = (_myTag == "Enemy")? _battle.GetEnemies(): _battle.GetPartyMembers();
                    int highestHealth = int.MinValue;
                    int highest = 0;

                    for(int i = 0; i < targets.Count; i++)
                    {
                        if(targets[i].CurrentHealth < highestHealth)
                        {
                            highestHealth = targets[i].CurrentHealth;
                            highest = i;
                        }
                    }
                    
                    GetRoot().SetData("HealTarget", targets[highest]);
                    //GetRoot().SetData("healtargetpos", targets[highest].transform.position);
                    state = NodeState.SUCCESS;
                    return state;
                }
            }

                state = NodeState.FAILURE;
                return state;
        }
        
    }

    public class CheckAttackTarget : Node
    {
        private GameController.BattleController _battle;
        private TargetPriority _targetting;
        private string _myTag;

        public CheckAttackTarget(TargetPriority t, string tag)
        {
            _targetting = t;
            _myTag = tag;
        }

        public override NodeState Evaluate()
        {
            object b = GetData("BattleController");

            if(b != null)
            {
                if(_targetting == TargetPriority.LOWHEALTH)
                {
                    _battle = (GameController.BattleController) b;
                    List<Character> targets = (_myTag == "Enemy")? _battle.GetPartyMembers(): _battle.GetEnemies();
                    int lowestHealth = int.MaxValue;
                    int lowest = int.MaxValue;

                    for(int i = 0; i < targets.Count; i++)
                    {
                        if(targets[i].CurrentHealth < lowestHealth)
                        {
                            lowestHealth = targets[i].CurrentHealth;
                            lowest = i;
                        }
                    }
                    
                    if(lowest < targets.Count)
                    {
                        GetRoot().SetData("EnemyTarget", targets[lowest].transform);
                        state = NodeState.SUCCESS;
                        return state;
                    }
                }else
                {
                    _battle = (GameController.BattleController) b;
                    List<Character> targets = (_myTag == "Enemy")? _battle.GetPartyMembers(): _battle.GetEnemies();
                    int highestHealth = int.MinValue;
                    int highest = int.MaxValue;

                    for(int i = 0; i < targets.Count; i++)
                    {
                        if(targets[i].CurrentHealth > highestHealth)
                        {
                            highestHealth = targets[i].CurrentHealth;
                            highest = i;
                        }
                    }
                    
                    if(highest < targets.Count)
                    {
                        GetRoot().SetData("EnemyTarget", targets[highest].transform);
                        state = NodeState.SUCCESS;
                        return state;
                    }
                }
            }

            state = NodeState.FAILURE;
            return state;
        }
    }
}