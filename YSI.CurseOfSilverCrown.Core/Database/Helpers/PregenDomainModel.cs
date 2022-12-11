namespace YSI.CurseOfSilverCrown.Core.Database.Helpers
{
    internal class PregenDomainModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public int[] BorderingDomainModelIds { get; set; }

        public PregenDomainModel(int id, int size, string name, int[] borderingDomainModelIds)
        {
            Id = id;
            Name = name;
            Size = size;
            BorderingDomainModelIds = borderingDomainModelIds;
        }
    }
}
