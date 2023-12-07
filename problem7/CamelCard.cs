class CamelCard(char Card) : IComparable<CamelCard>
{
    public char Label { get; private set; } = Card;
    public static List<char> CardRanks = [
        'A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J'
    ];

    public int CompareTo(CamelCard? other)
    {
        if (other == null) return -1;
        return CardRanks.IndexOf(this.Label) - CardRanks.IndexOf(other.Label);
    }
}