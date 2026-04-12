namespace YAGO.World.Domain.Entities.Episodes
{
    public class DilemmaTextInput : Dilemma
    {
        public override DilemmaType DilemmaType => DilemmaType.TextInput;

        public Slide Slide { get; }
        public string SubmitButtonName { get; }

        public DilemmaTextInput(
            Slide slide,
            string submitButtonName)
            : base()
        {
            Slide = slide;
            SubmitButtonName = submitButtonName;
        }
    }
}
