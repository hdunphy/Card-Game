using System.Collections.Generic;

public interface IDeckHolder
{
    public List<CardData> AllCards { get; }
    public List<CardData> CurrentDeck { get; }
}
