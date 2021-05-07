using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.Enums;

namespace YSI.CurseOfSilverCrown.Core.Database.Models
{
    public class Organization
    {
        public string Id { get; set; }

        public enOrganizationType OrganizationType { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }
        public int ProvinceId { get; set; }
        public string SuzerainId { get; set; }

        [Display(Name = "Казна")]
        public int Coffers { get; set; }

        [Display(Name = "Воины")]
        public int Warriors { get; set; }

        [Display(Name = "Имущество провинции")]
        public int Investments { get; set; }

        [Display(Name = "Укрепления")]
        public int Fortifications { get; set; }

        [Display(Name = "Провинция")]
        public Province Province { get; set; }

        [Display(Name = "Сюзерен")]
        public Organization Suzerain { get; set; }

        public User User { get; set; }

        [Display(Name = "Действие")]
        public List<Command> Commands { get; set; }

        public List<Command> ToOrganizationCommands { get; set; }
        public List<Command> ToOrganization2Commands { get; set; }

        [Display(Name = "Вассалы")]
        public List<Organization> Vassals { get; set; }
        public List<OrganizationEventStory> OrganizationEventStories { get; set; }
    }
}
