namespace RetroFinder.Models
{
    public record FastaSequence(string Id, string Sequence)
    {
        public string ID = Id;
        public string Sequence = Sequence;
    }
}
