namespace YSI.CurseOfSilverCrown.Core.MainModels
{
    internal class StartingDataMapModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public int[] BorderingDomainModelIds { get; set; }
        public int? SuzerainId { get; set; }

        public StartingDataMapModel(int id, int? suzerainId, int size, string name, int[] borderingDomainModelIds)
        {
            Id = id;
            Name = name;
            Size = size;
            BorderingDomainModelIds = borderingDomainModelIds;
            SuzerainId = suzerainId;
        }
    }
}
