namespace YAGO.World.Domain.Exceptions
{
    public class YagoNotVerifyOwnershipException : YagoException
    {
        public YagoNotVerifyOwnershipException(string type, int id)
            : base(string.Format("Объект {0} с Id={1} не находится под управлением текущего пользователя.", type, id))
        { }
    }
}
