using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public enum TurnState
{
    PlayerTurn,
    EnemyTurn,
    Busy
}
public class TurnManager : Singleton<TurnManager>, IInitializable
{
    public TurnState State { get; private set; }
    public int TurnCount { get; private set; }

    public async Task Init()
    {
        TurnCount = 1;
        await StartPlayerTurn();
    }

    public async Task StartPlayerTurn()
    {
        State = TurnState.PlayerTurn;

        Debug.Log($"Player Turn {TurnCount}시작");

        DeckManager.Instance.Draw(5); //여기도 일단은 값 기입. 나중에는 Player 클래스라던가 넣어서 거기서 덱 관리하고 할듯?

        await Task.Yield();
    }
    public async Task EndPlayerTurn()
    {
        if (State != TurnState.PlayerTurn)
            return;

        State = TurnState.Busy;
        Debug.Log("플레이어 턴 종료");

        DiscardAllCard();

        await StartEnemyTurn();
    }
    public async Task StartEnemyTurn()
    {
        State = TurnState.EnemyTurn;
        Debug.Log($"Enemy Turn {TurnCount} 시작");

        //Todo : 적 행동 로직

        await Task.Delay(500); //간단한 딜레이

        await EndEnemyTurn();
    }
    public async Task EndEnemyTurn()
    {
        Debug.Log("적 턴 종료");

        TurnCount++;

        await StartPlayerTurn();
    }

    private void DiscardAllCard() //이기능은 여기에 있는게 맞는가? 생각
    {
        var hand = DeckManager.Instance.Hand;

        for(int i = hand.Count - 1; i >= 0; i--)
        {
            var card = hand[i];
            hand.RemoveAt(i);

            card.State = CardState.InDiscard;
            DeckManager.Instance.Discard.Add(card);
        }
    }
}
