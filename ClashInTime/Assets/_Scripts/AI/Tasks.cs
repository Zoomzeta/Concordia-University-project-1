using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviourTree;

namespace Tasks
{
    public class TaskIdle : Node
    {
        private BasicAnimationController _animatorController;

        public TaskIdle(BasicAnimationController anim)
        {
            _animatorController = anim;
        }
        public override NodeState Evaluate()
        {
            // Animator Controller go to idle
            _animatorController.StopWalking();
            _animatorController.StopRunning();
            state = NodeState.RUNNING;
            return state;
        }
    }

    public class TaskWalk : Node
    {
        private Character _character;

        public TaskWalk(Character ch)
        {
            _character = ch;
        }

        public override NodeState Evaluate()
        {
            Transform enemy = (Transform)GetData("EnemyMoveTarget");

            if(enemy != null)
            {
                _character.gameObject.GetComponent<Arrive>().ArriveScript(enemy);
                state = NodeState.RUNNING;
                return state;
            }

            state = NodeState.FAILURE;
            return state;
        }
    }

    public class TaskCharge : Node
    {
        private Character _character;

        public TaskCharge(Character ch)
        {
            _character = ch;
        }

        public override NodeState Evaluate()
        {
            object e = GetData("EnemyMoveTarget");

            if(e != null)
            {
                Transform enemy = (Transform)e;

                if(_character.Anger == 3 && _character.CurrentHealth > _character.Health/3)
                {
                    _character.gameObject.GetComponent<Arrive>().ArriveScript(enemy);
                    state = NodeState.RUNNING;
                    return state;
                }

                state = NodeState.FAILURE;
                return state;
            }

            state = NodeState.FAILURE;
            return state;
        }
    }

    public class TaskWander : Node
    {
        private Character _character;

        public TaskWander(Character ch)
        {
            _character = ch;
        }

        public override NodeState Evaluate()
        {
            _character.gameObject.GetComponent<CampWander>().WanderInCamp();
            state = NodeState.RUNNING;
            return state;
        }
    }

    public class TaskFlee : Node
    {
        private Character _character;
        private BasicAnimationController _anim;

        public TaskFlee(Character ch, BasicAnimationController anim)
        {
            _character = ch;
            _anim = anim;
        }

        public override NodeState Evaluate()
        {
            Transform enemy = (Transform)GetData("EnemyTarget");

            if(enemy != null)
            {
                if(_character.Bravery == 1 && _character.CurrentHealth < _character.Health/2)
                {
                    _character.GetComponent<Flee>().FleeScript();
                    _anim.StartRunning();
                    state = NodeState.RUNNING;
                    return state;
                }

                state = NodeState.FAILURE;
                return state;
            }

            state = NodeState.FAILURE;
            return state;
        }
    }

    public class TaskMoveToSocket : Node
    {
        private Character _character;
        private BasicAnimationController _anim;

        public TaskMoveToSocket(Character ch, BasicAnimationController anim)
        {
            _character = ch;
            _anim = anim;
        }

        public override NodeState Evaluate()
        {
            _character.GetComponent<Formation>().StayInFormation(_anim);
            state = NodeState.RUNNING;
            return state;
        }
    }

    public class TaskAttackTarget : Node
    {
        private BasicAnimationController _animatorController;
        private GameController.BattleController _battle;
        private Character _character;

        public TaskAttackTarget(BasicAnimationController animatorCon, Character c)
        {
            _animatorController = animatorCon;
            _character = c;
        }

        public override NodeState Evaluate()
        {
            Transform enemy = (Transform)GetData("EnemyTarget");
            object b = GetData("BattleController");

            if(enemy != null && b != null)
            {
                _battle = (GameController.BattleController)b;

                // AnimatorController play attack
                _animatorController.Attack();
                enemy.gameObject.GetComponent<Character>().receiveDamage(_character.AttackValue);
                _battle.TurnDone();
                state = NodeState.RUNNING;
                return state;
            }

            state = NodeState.FAILURE;
            return state;
        }
    }

    public class TaskPass : Node
    {
        private BasicAnimationController _animatorController;
        private GameController.BattleController _battle;
        private Character _char;
        private int passCounter = 0;

        public TaskPass(BasicAnimationController animatorCon, Character character)
        {
            _animatorController = animatorCon;
            _char = character;
        }

        public override NodeState Evaluate()
        {
            passCounter++;

            object b = GetData("BattleController");

            if(b != null)
            {
                _battle = (GameController.BattleController)b;
                int bravery = _char.Bravery;
                if(_char.Class == "Warrior")
                {
                    if(bravery == 2)
                    {
                        if(passCounter > 2)
                        {
                            _battle.TurnDone();
                            passCounter = 0;
                            state = NodeState.RUNNING;
                            return state;
                        }
                    }else if(bravery == 1)
                    {
                        if(passCounter > 0)
                        {
                            _battle.TurnDone();
                            passCounter = 0;
                            state = NodeState.RUNNING;
                            return state;
                        }
                    }
                }else if(_char.Class == "Archer")
                {
                    if(bravery == 1)
                    {
                        if(passCounter > 2)
                        {
                            _battle.TurnDone();
                            passCounter = 0;
                            state = NodeState.RUNNING;
                            return state;
                        }
                    }
                }else if(_char.Class == "Mage")
                {
                    if(bravery == 3)
                    {
                        if(passCounter > 3)
                        {
                            _battle.TurnDone();
                            passCounter = 0;
                            state = NodeState.RUNNING;
                            return state;
                        }
                    }else if(bravery == 2)
                    {
                        if(passCounter > 2)
                        {
                            _battle.TurnDone();
                            passCounter = 0;
                            state = NodeState.RUNNING;
                            return state;
                        }
                    }else if(bravery == 1)
                    {
                        if(passCounter > 1)
                        {
                            _battle.TurnDone();
                            passCounter = 0;
                            state = NodeState.RUNNING;
                            return state;
                        }
                    }
                }
            }
            
            
            state = NodeState.FAILURE;
            return state;
        }
    }

    public class TaskReload : Node
    {
        private GameController.BattleController _battle;
        private Character _character;
        public TaskReload(Character character)
        {
            _character = character;
        }

        public override NodeState Evaluate()
        {
            object b = GetData("BattleController");

            if(b != null)
            {
                _battle = (GameController.BattleController)b;

                /*
                    RELOAD CODE: Uncomment if ammunition for archers is implemented
                */
                // if(_character.AmmoCount <= 0)
                // {
                //     _character.Reload();
                //     _battle.TurnDone();
                //     state = NodeState.RUNNING;
                //     return state;
                // }
            }

            state = NodeState.FAILURE;
            return state;
        }
    }

    public class TaskHeal : Node
    {
        private GameController.BattleController _battle;
        private Character _character;

        private BasicAnimationController _anim;

        public TaskHeal(Character c, BasicAnimationController anim)
        {
            _character = c;
            _anim = anim;
        }

        public override NodeState Evaluate()
        {
            object t = GetData("HealTarget");
            object b = GetData("BattleController");

            if(t != null && b != null)
            {
                Character healTarget = (Character)t;
                _battle = (GameController.BattleController)b;
                _anim.Heal();
                healTarget.heal(_character.Intelligence);
                state = NodeState.RUNNING;
                return state;
            }

            state = NodeState.FAILURE;
            return state;
        }
    }
}