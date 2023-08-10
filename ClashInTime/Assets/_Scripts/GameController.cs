using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class GameController : MonoBehaviour
{
    private BattleController _battleCon = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //need to implement a way for the gameobject to recognize the enemy triggered
    public void StartBattle(GameObject firstHit)
    {
        _battleCon = new BattleController();
        GameObject parentCamp = firstHit.transform.parent.gameObject;

        while(!parentCamp.name.Contains("Camp"))
            parentCamp = parentCamp.transform.parent.gameObject;

        foreach(Transform child in parentCamp.GetComponentInChildren<Transform>())
        {
            _battleCon.battlers.Add(child.gameObject);
        }

        foreach(GameObject partyMem in GameObject.FindGameObjectsWithTag("Party"))
        {
            _battleCon.battlers.Add(partyMem);
        }

        _battleCon.battlers.Sort((b1, b2) => b1.GetComponent<Character>().Agility.CompareTo(b2.GetComponent<Character>().Agility));
    }

    public void EndBattle()
    {
        _battleCon.battlers.Clear();
        _battleCon = null;
    }

    public BattleController GetBattleController()
    {
        return _battleCon;
    }

    public class BattleController
    {
        private int currentTurn = 0;
        public List<GameObject> battlers = new List<GameObject>();

        public GameObject GetCurrentBattler()
        {
            return battlers[currentTurn];
        }

        public void TurnDone()
        {
            currentTurn++;
            if(currentTurn >= battlers.Count)
            {
                currentTurn = 0;
            }
        }

        public List<Character> GetPartyMembers()
        {
            GameObject[] obs = GameObject.FindGameObjectsWithTag("Party");
            List<Character> chars = new List<Character>();

            foreach(GameObject ob in obs)
            {
                chars.Add(ob.GetComponent<Character>());
            }
            
            return chars;
        }

        public List<Character> GetEnemies()
        {
            GameObject[] obs = GameObject.FindGameObjectsWithTag("Enemy");
            List<Character> chars = new List<Character>();

            foreach(GameObject ob in obs)
            {
                chars.Add(ob.GetComponent<Character>());
            }

            return chars;
        }
    }
}
