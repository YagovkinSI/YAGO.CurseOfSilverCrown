namespace YAGO.World.Infrastructure.APIModels.BudgetModels
{
    public class BudgetLine
    {
        public BudgetLineType Type { get; set; }

        public BudgetLineSource CommandSourceTable { get; set; }

        public string Descripton { get; set; }

        public ParameterChanging<int?> Coffers { get; set; }

        public ParameterChanging<int?> Warriors { get; set; }

        public ParameterChanging<int?> Investments { get; set; }

        public ParameterChanging<int?> Fortifications { get; set; }

        public bool Editable { get; set; }

        public bool Deleteable { get; set; }

        public int CommandId { get; set; }
    }
}
