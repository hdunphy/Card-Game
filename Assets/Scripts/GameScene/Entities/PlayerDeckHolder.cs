using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDeckHolder
{
    public List<CardData> AllCards { get; }
    public List<CardData> CurrentDeck { get; }
}

public class PlayerDeckHolder : IDeckHolder
{
    public List<CardData> AllCards { get; private set; }
    private List<List<CardData>> Decks { get; set; }
    private int CurrentDeckIndex { get; set; }
    public List<CardData> CurrentDeck => Decks[CurrentDeckIndex];

    public PlayerDeckHolder(List<CardData> allCards)
    {
        AllCards = allCards;
        Decks = new List<List<CardData>> {
            new List<CardData>()
        };
        CurrentDeckIndex = 0;
    }

    public PlayerDeckHolder(List<CardData> allCards, List<List<CardData>> decks, int currentDeck = 0)
    {
        AllCards = allCards;
        Decks = decks;
        CurrentDeckIndex = currentDeck;
    }
}
