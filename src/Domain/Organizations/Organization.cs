namespace YAGO.World.Domain.Organizations
{
    /// <summary>
    /// Организация
    /// </summary>
    public class Organization
    {
        /// <summary>
        /// Идентификатор организации
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Ресурсы организации
        /// </summary>
        public int Gold { get; set; }

        public Organization(
            int id, 
            int resourses)
        {
            Id = id;
            Gold = resourses;
        }
    }
}
