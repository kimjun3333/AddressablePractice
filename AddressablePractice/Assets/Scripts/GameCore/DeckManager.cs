using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DeckManager : Singleton<DeckManager>, IInitializable
{
    public List<CardInstance> Library = new();
    public List<CardInstance> Hand = new();
    public List<CardInstance> Discard = new();
    public List<CardInstance> Exhaust = new();

    //임시 추가
    public Transform HandArea;
    public GameObject CardPrefab;

    public async Task Init() //여기서 최초 덱 초기화 하고
    {
        List<CardSO> allCards = DataManager.Instance.GetAllDataOfType<CardSO>();

        Library.Clear();
        Hand.Clear();
        Discard.Clear();
        Exhaust.Clear();
        
        foreach(var so in allCards)
            Library.Add(new CardInstance(so));

        await Task.Yield();
    }

    public void Shuffle(List<CardInstance> list)
    {
        for(int i = 0; i  < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }
       
    public void Draw(int count)
    {
        for(int i = 0; i < count; i++)
        {
            if (Library.Count == 0)
                Reshuffle();

            if(Library.Count == 0)
            {
                Debug.LogWarning("뽑을 카드가 없습니다.");
                return;
            }

            var card = Library[0];
            Library.RemoveAt(0);

            card.State = CardState.InHand;
            Hand.Add(card);

            //임시추가
            CreateCardUI(card);
        }
    }

    public void UseCard(CardInstance card)
    {
        if (!Hand.Contains(card))
            return;

        Hand.Remove(card);

        if(card.Template.isExhaust)
        {
            card.State = CardState.InExhaust;
            Exhaust.Add(card);
        }
        else
        {
            card.State = CardState.InDiscard;
            Discard.Add(card);
        }
    }

    public void Reshuffle()
    {
        if (Discard.Count == 0)
            return;

        Debug.Log("Discard카드 Library로 재편성");

        Library.AddRange(Discard);
        Discard.Clear();

        foreach (var c in Library)
            c.State = CardState.InLibrary;

        Shuffle(Library);
    }

    public void CreateCardUI(CardInstance card)
    {
        GameObject obj = Instantiate(CardPrefab, HandArea);
        CardUI ui = obj.GetComponent<CardUI>();
        ui.Bind(card);
    }
}
