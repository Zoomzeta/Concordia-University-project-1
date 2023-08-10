using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUI : MonoBehaviour
{
    //This script needs to acess GameController.cs/BattlController

    private GameController gController;
    //private BattleController bController = null;

    //click enemy/ally to select a target
    //TODO: make each enemy/ally clickable to get targeted character during a battle.
    private Character targetCharacter;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Update()
    {
        //bController = gController.GetBattleController();
    }

    public void OnSelectTarget()
    { 
    }

    public void OnAttackButton()
    {
    }
    public void OnHealButton()
    {
    }

    //target character with this.
    void OnMouseDown()
    { 
    }

}
