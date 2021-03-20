using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.Models.DbModels;

namespace YSI.CurseOfSilverCrown.Web.Data
{
    public class BaseData
    {
        private Turn firstTurn = new Turn { Id = 1, Name = "587 год - Зима" };

        private Tuple<int, string, string>[] provinces = new []
        {
            new Tuple<int, string, string>(1, "Оловянные шахты", "TinMines"),
            new Tuple<int, string, string>(2, "Мыс ящера", "CapeRaptor"),
            new Tuple<int, string, string>(3, "Устье Полаймы", "MouthOfPolaima"),
            new Tuple<int, string, string>(4, "Верещатник Диммории", "HeatherOfDimmoria"),
            new Tuple<int, string, string>(5, "Долина Диммории", "DimmoriaValley"),
            new Tuple<int, string, string>(6, "Летний берег", "SummerCoast"),
            new Tuple<int, string, string>(7, "Фермы Диммории", "DimmoriaFarms"),
            new Tuple<int, string, string>(8, "Меловые скалы", "ChalRocks"),
            new Tuple<int, string, string>(9, "Известняковые хребты", "LimestoneRidges"),
        };

        public Province[] GetProvinces()
        {
            return provinces
                .Select(p => new Province
                {
                    Id = p.Item1,
                    Name = p.Item2
                })
                .ToArray();
        }

        public Organization[] GetOrganizations()
        {
            return provinces
                .Select(p => new Organization
                {
                    Id = p.Item3,
                    Name = p.Item2,
                    OrganizationType = Enums.enOrganizationType.Lord,
                    ProvinceId = p.Item1,
                    Power = 200000
                })
                .ToArray();
        }

        internal Command[] GetCommands()
        {
            return provinces
                .Select(p => new Command
                {
                    Id = Guid.NewGuid().ToString(),
                    TurnId = firstTurn.Id,
                    OrganizationId = p.Item3,
                    Type = Enums.enCommandType.Idleness
                })
                .ToArray();
        }

        internal Turn GetFirstTurn()
        {
            return firstTurn;
        }
    }
}
