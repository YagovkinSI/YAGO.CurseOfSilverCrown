using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.Enums;

namespace YSI.CurseOfSilverCrown.Web.Models.DbModels
{
    public class Organization
    {
        public string Id { get; set; }

        [Display(Name = "Тип организации")]
        public enOrganizationType OrganizationType { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }
        public int ProvinceId { get; set; }
        public string SuzerainId { get; set; }

        [Obsolete]
        public int Power { get; set; }

        [Display(Name = "Казна")]
        public int Coffers { get; set; }

        [Display(Name = "Воины")]
        public int Warriors { get; set; }

        [Display(Name = "Провинция")]
        public Province Province { get; set; }

        [Display(Name = "Сюзерен")]
        public Organization Suzerain { get; set; }

        public User User { get; set; }

        [Display(Name = "Действие")]
        public List<Command> Commands { get; set; }

        public List<Command> ToOrganizationCommands { get; set; }

        [Display(Name = "Вассалы")]
        public List<Organization> Vassals { get; set; }
        public List<OrganizationEventStory> OrganizationEventStories { get; set; }
    }
}
