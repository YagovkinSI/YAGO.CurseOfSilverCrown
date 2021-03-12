namespace YAGO.World.Domain.Exceptions
{
    public class YagoNotFoundException : YagoException
    {
        public YagoNotFoundException(string type, int id)
            : base(string.Format("Не удалось найти объект {0} с Id={1}.", type, id))
        { }
    }
}
