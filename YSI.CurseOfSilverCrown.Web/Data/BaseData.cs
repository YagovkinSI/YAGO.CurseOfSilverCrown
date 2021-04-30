using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.BL.EndOfTurn;
using YSI.CurseOfSilverCrown.Web.Models.DbModels;

namespace YSI.CurseOfSilverCrown.Web.Data
{
    public class BaseData
    {
        private Turn firstTurn = new Turn 
        { 
            Id = 1,
            Started = DateTime.UtcNow,
            IsActive = true
        };

        private BaseProvince[] BaseProvinces = new BaseProvince[]
        {
            new BaseProvince(1, "Оловянные шахты", "TinMines", new [] { 2, 3 }),
            new BaseProvince(2, "Мыс ящера", "CapeRaptor", new [] { 1, 3, 4 }),
            new BaseProvince(3, "Устье Полаймы", "MouthOfPolaima", new [] { 1, 2, 4 }),
            new BaseProvince(4, "Верещатник Диммории", "HeatherOfDimmoria", new [] { 2, 3, 5, 6 }),
            new BaseProvince(5, "Долина Диммории", "DimmoriaValley", new [] { 4, 6, 7, 8 }),
            new BaseProvince(6, "Летний берег", "SummerCoast", new [] { 4, 5, 7 }),
            new BaseProvince(7, "Фермы Диммории", "DimmoriaFarms", new [] { 5, 6, 8, 9 }),
            new BaseProvince(8, "Меловые скалы", "ChalRocks", new [] { 5, 7, 9 }),
            new BaseProvince(9, "Известняковые хребты", "LimestoneRidges", new [] { 7, 8 })
        };

        public Province[] GetProvinces()
        {
            return BaseProvinces
                .Select(p => new Province
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .ToArray();
        }

        public Organization[] GetOrganizations()
        {
            return BaseProvinces
                .Select(p => new Organization
                {
                    Id = p.OrganizationId,
                    Name = p.Name,
                    OrganizationType = Enums.enOrganizationType.Lord,
                    ProvinceId = p.Id,
                    Warriors =  100,
                    Coffers = 4000
                })
                .ToArray();
        }

        internal Command[] GetCommands()
        {
            return BaseProvinces
                .Select(p => new Command
                {
                    Id = Guid.NewGuid().ToString(),
                    OrganizationId = p.OrganizationId,
                    Type = Enums.enCommandType.Idleness
                })
                .ToArray();
        }

        internal Turn GetFirstTurn()
        {
            return firstTurn;
        }

        internal Route[] GetRotes()
        {
            return BaseProvinces
                .SelectMany(b => b.RoutesToProvinces
                    .Select(r => new Route
                    {
                        FromProvinceId = b.Id,
                        ToProvinceId = r
                    }))
                .ToArray();
        }

        private class BaseProvince
        {
            public int Id { get; set; }
            public string OrganizationId { get; set; }
            public string Name { get; set; }
            public int[] RoutesToProvinces { get; set; }


            public BaseProvince(int id, string name, string organizationId, int[] routesToProvinces)
            {
                Id = id;
                OrganizationId = organizationId;
                Name = name;
                RoutesToProvinces = routesToProvinces;
            }
        }
    }
}
