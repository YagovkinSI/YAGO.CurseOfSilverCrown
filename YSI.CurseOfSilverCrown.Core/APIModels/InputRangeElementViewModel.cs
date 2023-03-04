namespace YSI.CurseOfSilverCrown.Core.APIModels
{
    public class InputRangeElementViewModel
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string ObjectNameId { get; set; }
        public int ObjectId { get; set; }
        public string CountName { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public int Step { get; set; }
        public string LabelText { get; set; }
        public string ButtonName { get; set; }

        private int? startValue;
        public int StartValue
        {
            get => startValue ?? MinValue + (MaxValue - MinValue) / 2;
            set => startValue = value;
        }

        public InputRangeElementViewModel(int step = 1)
        {
            Step = step;
        }

    }
}
