namespace YAGO.World.Domain.Persons
{
    public class Person
    {
        public string Code { get; }
        public string Name { get; }
        public string Avatar { get; }
        public string WikiArticleCode { get; }

        public Person(
            string code,
            string name,
            string avatar,
            string wikiArticleCode)
        {
            Code = code;
            Name = name;
            Avatar = avatar;
            WikiArticleCode = wikiArticleCode;
        }
    }
}