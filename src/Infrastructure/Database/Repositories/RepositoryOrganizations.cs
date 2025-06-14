using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Factions.Enums;
using YAGO.World.Domain.YagoEntities;
using YAGO.World.Domain.YagoEntities.Enums;
using YAGO.World.Infrastructure.Database.Models.Domains;

namespace YAGO.World.Infrastructure.Database.Repositories
{
    public class RepositoryOrganizations : IRepositoryOrganizations
    {
        private readonly ApplicationDbContext _context;

        public RepositoryOrganizations(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyCollection<Domain.Factions.Faction>> GetAll()
        {
            return await _context.Domains
                .Select(d => d.ToDomain())
                .ToListAsync();
        }

        public async Task<Domain.Factions.Faction> Get(int? organizationId)
        {
            var dbOrganization = await _context.Domains.FindAsync(organizationId);
            return dbOrganization?.ToDomain();
        }

        public async Task<Domain.Factions.Faction> GetOrganizationByUser(string userId)
        {
            var dbOrganization = await _context.Domains
                .SingleOrDefaultAsync(o => o.UserId == userId);

            return dbOrganization?.ToDomain();
        }

        public async Task<ListItem[]> GetFactionList
            (int page, int pageSize, FactionOrderBy factionOrderBy, bool useOrderByDescending)
        {
            var domains = _context.Domains;
            var selector = GetSelector(factionOrderBy);

            var query = useOrderByDescending
                ? domains.OrderByDescending(selector)
                : domains.OrderBy(selector);

            var skip = (page - 1) * pageSize;

            var items = await query
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return items
                .Select((d, index) => ToListItem(d, index + skip + 1, factionOrderBy))
                .ToArray();
        }

        private Expression<Func<Organization, object>> GetSelector(FactionOrderBy factionOrderBy)
        {
            return factionOrderBy switch
            {
                FactionOrderBy.Name => o => o.Name,
                FactionOrderBy.WarriorCount => o => o.Units.Sum(u => u.Warriors),
                FactionOrderBy.Gold => o => o.Gold,
                FactionOrderBy.Investments => o => o.Investments,
                FactionOrderBy.Fortifications => o => o.Fortifications,
                FactionOrderBy.Suzerain => o => o.Suzerain == null ? "" : o.Suzerain.Name,
                FactionOrderBy.User => o => o.User == null ? "" : o.User.UserName,
                _ => o => o.Vassals.Count,
            };
        }

        private ListItem ToListItem(Organization organization, int number, FactionOrderBy factionOrderBy)
        {
            var value = factionOrderBy switch
            {
                FactionOrderBy.Name => null,
                FactionOrderBy.WarriorCount => YagoEntity.CreateFakeEntity(organization.Units.Sum(u => u.Warriors).ToString()),
                FactionOrderBy.Gold => YagoEntity.CreateFakeEntity(organization.Gold.ToString()),
                FactionOrderBy.Investments => YagoEntity.CreateFakeEntity(organization.Investments.ToString()),
                FactionOrderBy.Fortifications => YagoEntity.CreateFakeEntity(organization.Fortifications.ToString()),
                FactionOrderBy.Suzerain => YagoEntity.CreateFakeEntity(organization.Suzerain?.Name ?? ""),
                FactionOrderBy.User => YagoEntity.CreateFakeEntity(organization.User?.UserName ?? ""),
                _ => YagoEntity.CreateFakeEntity(organization.Vassals.Count.ToString()),
            };

            return new ListItem(
                number,
                new YagoEntity(organization.Id, YagoEntityType.Faction, organization.Name),
                value
            );
        }
    }
}
