using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tactics.Core{
public class UnitManager : MonoBehaviour
{

    const int _MAXATBCOUNT = 100;
    const int _QUEUESIZE = 10;
    public static UnitManager Instance{ get; private set; }
    [SerializeField] private List<Unit> unitList;
    private List<Unit> friendlyUnitList;
    private List<Unit> enemyUnitList;
    private List<int> unitATBList;
    private List<int> queueOrderList;
    private int turnQueue = 0;

    private void Awake() {

        if (Instance != null)
        {
            Debug.LogError("There is more than one UnitManager! " + transform + "-" + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        unitList = new List<Unit>();
        friendlyUnitList = new List<Unit>();
        enemyUnitList = new List<Unit>();
        
        unitATBList = new List<int>();
        queueOrderList = new List<int>();
    }

    private void Start() {
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }

    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        unitList.Add(unit);
        unitATBList.Add(0);


        if (unit.IsEnemy())
        {
            enemyUnitList.Add(unit);
        }
        else
        {
            friendlyUnitList.Add(unit);
        }

    }

    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        RemoveUnitFromList(unit);
    }

    private void RemoveUnitFromList(Unit unit)
    {
        int index = 0;
        for (int _index = 0; _index < unitList.Count; _index++)
        {
            if (unitList[_index] == unit)
            {
                index = _index;
                break;
            }
        }
        unitList.Remove(unit);
        unitATBList.RemoveAt(index);

        if (unit.IsEnemy())
        {
            enemyUnitList.Remove(unit);
        }
        else
        {
            friendlyUnitList.Remove(unit);
            CheckDeadUnit(unit);
        }

        if (queueOrderList.Contains(index))
        {
            //queueOrderList = new List<int>();
            //turnQueue = 0;
            //UpdateQueueTurns(queueOrderList);
        }

        queueOrderList = new List<int>();
        turnQueue = 0;
        TurnSystem.Instance.NextTurn();
    }

    private void CheckDeadUnit(Unit unit)
    {
        if (friendlyUnitList.Count == 0)
        {
            GameOverSystem.Instance.GameOver();
        }
        //else if (unit == UnitActionSystem.Instance.GetSelectedUnit())
        //{
        //    UnitActionSystem.Instance.SetSelectedUnit(friendlyUnitList[0]);
        //}
        
    }

    public List<Unit> GetUnitList()
    {
        return unitList;
    }

    public List<Unit> GetFriendlyUnitList()
    {
        return friendlyUnitList;
    }

    public List<Unit> GetEnemyUnitList()
    {
        return enemyUnitList;
    }


    public Unit CheckTurnOrder()
    {
        if (unitATBList.Count == 0)
        {
            foreach(Unit unit in unitList)
            {
                unitATBList.Add(0);
            }
        }
        Debug.Log("Turn Queue: " + turnQueue + "_________________________________________");
        int actualUnitIndex = UpdateActualTurn(ref unitATBList, ref queueOrderList, ref turnQueue);
        Debug.Log("Playing unit: " + actualUnitIndex + "/Turn Queue: " + turnQueue);
        UpdateQueueTurns(unitATBList);

        Unit actualUnit = unitList[actualUnitIndex];
        return actualUnit;

    }

    private int UpdateActualTurn(ref List<int> ATBList, ref List<int> _queueOrderList, ref int turnQueue)
    {
        int returnIndex = -1;
        while (returnIndex == -1)
        {
            if (turnQueue == 0)
            {
                for (int index = 0; index < ATBList.Count; index++)
                {
                    ATBList[index] += unitList[index].GetTurnSpeed();
                }

                for (int index = 0; index < ATBList.Count; index++)
                {
                    if (ATBList[index] >=  _MAXATBCOUNT)
                    {
                        InsertQueueList(ref _queueOrderList, index, ref turnQueue);
                    }
                }

                if(_queueOrderList.Count != 0)
                {
                    //function
                    returnIndex = _queueOrderList[0];
                    _queueOrderList.RemoveAt(0);
                    turnQueue -=1;
                }
            }

            else
            {
                //Create function
                returnIndex = _queueOrderList[0];
                _queueOrderList.RemoveAt(0);
                turnQueue -= 1;
            }


        }

        ATBList[returnIndex] -= _MAXATBCOUNT;
        return returnIndex;
    }

    private void InsertQueueList(ref List<int> queueOrderList, int newIndex, ref int turnQueue)
    {
        if(queueOrderList.Count == 0)
        {
            queueOrderList.Add(newIndex);
            turnQueue +=1;
            return;
        }
        for (int index = 0; index < queueOrderList.Count; index++)
        {
            if(unitList[newIndex].GetTurnSpeed() > unitList[index].GetTurnSpeed())
            {
                queueOrderList.Insert(index, newIndex);
                turnQueue +=1;
                return;
            }
        }

        if(unitList[newIndex].GetTurnSpeed() <= unitList[queueOrderList.Count-1].GetTurnSpeed())
        {
            queueOrderList.Add(newIndex);
            turnQueue += 1;
            return;
        }

    }

    private void UpdateQueueTurns(List<int> ATBList)
    {
        List<int> queueATBList = new List<int>(ATBList);
        List<int> possibeQueueOrderList = new List<int>(queueOrderList);
        int possibleTurnQueue = turnQueue;


        for (int i = 0; i < turnQueue; i++)
        {
            Debug.Log("Queue " + i + ":" + possibeQueueOrderList[i]);
        }

        for (int i = turnQueue; i < _QUEUESIZE; i++)
        {
            int queueUnitIndex = UpdateActualTurn(ref queueATBList, ref possibeQueueOrderList, ref possibleTurnQueue);
            Debug.Log("Queue " + i + ":" + queueUnitIndex + " _ " + possibleTurnQueue);
        }

    }
}
}
