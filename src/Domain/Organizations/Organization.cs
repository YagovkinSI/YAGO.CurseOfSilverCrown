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

        public Organization(int id)
        {
            Id = id;
        }
    }
}
