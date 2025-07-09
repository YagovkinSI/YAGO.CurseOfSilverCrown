using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Factions.Enums;
using YAGO.World.Domain.YagoEntities;
using YAGO.World.Domain.YagoEntities.Enums;
using YAGO.World.Infrastructure.Database.Models.Domains;

namespace YAGO.World.Infrastructure.Database.Repositories
{
    public class RepositoryFactions : IRepositoryFactions
    {
        private const int PAGE_SIZE = 10;

        private readonly ApplicationDbContext _context;

        public RepositoryFactions(ApplicationDbContext context)
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

        public async Task<ListData> GetFactionList(int page, FactionOrderBy factionOrderBy)
        {
            var query = GetQuery(factionOrderBy)
                .ThenBy(f => f.Name);

            var skip = (page - 1) * PAGE_SIZE;

            var factions = await query
                .Skip(skip)
                .Take(PAGE_SIZE)
                .ToListAsync();

            var items = factions
                .Select((d, index) => ToListItem(d, index + skip + 1, factionOrderBy))
                .ToArray();

            return new ListData(items, _context.Domains.Count());
        }

        private IOrderedQueryable<Organization> GetQuery(FactionOrderBy factionOrderBy)
        {
            return factionOrderBy switch
            {
                FactionOrderBy.Name => _context.Domains.OrderBy(o => o.Name),
                FactionOrderBy.WarriorCount => _context.Domains.OrderByDescending(o => o.Units.Sum(u => u.Warriors)),
                FactionOrderBy.Gold => _context.Domains.OrderByDescending(o => o.Gold),
                FactionOrderBy.Investments => _context.Domains.OrderByDescending(o => o.Investments),
                FactionOrderBy.Fortifications => _context.Domains.OrderByDescending(o => o.Fortifications),
                FactionOrderBy.Suzerain => _context.Domains.OrderBy(o => o.Suzerain == null ? "" : o.Suzerain.Name),
                FactionOrderBy.User => _context.Domains.OrderBy(o => o.User == null ? "- нет игрока -" : o.User.UserName),
                _ => _context.Domains.OrderByDescending(o => o.Vassals.Count),
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
                FactionOrderBy.Suzerain => organization.Suzerain == null
                    ? YagoEntity.CreateFakeEntity(" - независимый -")
                    : new YagoEntity(organization.Suzerain.Id, YagoEntityType.Faction, organization.Suzerain.Name),
                FactionOrderBy.User => YagoEntity.CreateFakeEntity(organization.User?.UserName ?? "- нет игрока -"),
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
