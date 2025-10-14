namespace YAGO.World.Domain.Exceptions
{
    public class YagoUnknownTypeException : YagoException
    {
        public YagoUnknownTypeException(string type)
            : base(string.Format("Тип '{0}' имеет недопустимое значение 'Unknown'.", type))
        { }
    }
}
