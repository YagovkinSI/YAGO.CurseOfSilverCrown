using Microsoft.EntityFrameworkCore;

namespace YSI.CurseOfSilverCrown.Core.Database.Errors
{
    public class Error
    {
        public int Id { get; set; }
        public string RequestId { get; set; }
        public string TypeFullName { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }

        internal static void CreateModel(ModelBuilder builder)
        {
            var model = builder.Entity<Error>();
            model.HasKey(m => new { m.Id });
        }
    }
}
