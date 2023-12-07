
using System.Runtime.InteropServices;

class CamelPokerHand : IComparable<CamelPokerHand>
{
    public int Bid { get; private set; }
    public int Rank { get; private set; }
    public String CardString { get; private set; }
    public List<CamelCard> Hand { get; private set; }
    public Dictionary<char, int> Dict { get; private set; }

    public CamelPokerHand(String Cards, int Bid)
    {
        this.CardString = Cards;
        this.Bid = Bid;
        this.Hand = Cards.ToCharArray().Select(c => new CamelCard(c)).ToList();
        this.Rank = DetermineRank(this.Hand);
        this.Dict = CardDict(this.Hand);
    }

    static List<String> RankNames = [
       "Five of a Kind", "Four of a Kind", "Full House", "Three of a Kind",
        "Two Pair", "One Pair", "High",
     ];

    private int DetermineRank(List<CamelCard> hand)
    {
        if (NofAKind(hand, 5) == 1)
        {
            return 0;
        }
        else if (NofAKind(hand, 4) == 1)
        {
            return 1;
        }
        else if (NofAKind(hand, 3) == 1 && NofAKind(hand, 2) == 1)
        {
            return 2;
        }
        else if (NofAKind(hand, 3) == 1)
        {
            return 3;
        }
        else if (NofAKind(hand, 2) == 2)
        {
            return 4;
        }
        else if (NofAKind(hand, 2) == 1)
        {
            return 5;
        }

        return 6;
    }

    public static int NofAKind(List<CamelCard> hand, int n)
    {
        return CardDict(hand).ToList().Where(p => p.Value == n).Count();
    }

    public static Dictionary<char, int> CardDict(List<CamelCard> hand)
    {
        Dictionary<char, int> cardDict = [];
        foreach (CamelCard c in hand)
        {
            cardDict.TryAdd(c.Label, 0);
            cardDict[c.Label]++;
        }
        // apply jokers to the largest values:
        int numJokers = 0;
        if (cardDict.TryGetValue('J', out int nJ))
        {
            numJokers = nJ;
            if (numJokers == 5) return cardDict;
            cardDict.Remove('J');
        }
        while (numJokers > 0)
        {
            var maxC = cardDict.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
            cardDict[maxC]++;
            numJokers--;
        }
        return cardDict;
    }



    public override string ToString()
    {
        return this.CardString + ": " + RankNames[this.Rank] +
        "( " + string.Join(", ", this.Dict.ToArray()) + " )";
    }

    public int CompareTo(CamelPokerHand? other)
    {
        if (other == null) return -1;
        if (this.Rank != other.Rank)
        {
            return this.Rank - other.Rank;
        }
        for (var i = 0; i < this.Hand.Count; i++)
        {
            if (this.Hand[i].Label != other.Hand[i].Label)
            {
                return this.Hand[i].CompareTo(other.Hand[i]);
            }
        }
        return 0;
    }
}
