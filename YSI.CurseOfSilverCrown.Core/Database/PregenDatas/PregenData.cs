using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Enums;

namespace YSI.CurseOfSilverCrown.Core.Database.PregenDatas
{
    internal static class PregenData
    {
        private static Turn firstTurn = new Turn 
        { 
            Id = 1,
            Started = DateTime.UtcNow,
            IsActive = true
        };

        private static BaseProvince[] BaseProvinces = new BaseProvince[]
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

        public static Province[] Provinces =>
            BaseProvinces
                .Select(p => new Province
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .ToArray();

        public static Organization[] Organizations =>
            BaseProvinces
                .Select(p => new Organization
                {
                    Id = p.OrganizationId,
                    Name = p.Name,
                    OrganizationType = enOrganizationType.Lord,
                    ProvinceId = p.Id,
                    Warriors = 100, // RandomHelper.AddRandom(Constants.StartWarriors, randomNumber: (p.Id * p.Id) % 10 / 10.0),
                    Coffers = 4000, //RandomHelper.AddRandom(Constants.StartCoffers, randomNumber: ((p.Id + 1) * p.Id) % 10 / 10.0, roundRequest: -1)
                })
                .ToArray();

        internal static Command[] Commands =>
            BaseProvinces
                .Select(p => new Command
                {
                    Id = Guid.NewGuid().ToString(),
                    OrganizationId = p.OrganizationId,
                    Type = enCommandType.Idleness
                })
                .ToArray();

        internal static Turn GetFirstTurn()
        {
            return firstTurn;
        }

        internal static Route[] Routes =>        
            BaseProvinces
                .SelectMany(b => b.RoutesToProvinces
                    .Select(r => new Route
                    {
                        FromProvinceId = b.Id,
                        ToProvinceId = r
                    }))
                .ToArray();        

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
