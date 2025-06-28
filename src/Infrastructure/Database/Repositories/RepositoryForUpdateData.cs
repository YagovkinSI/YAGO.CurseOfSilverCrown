using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Domain.Units.Enums;

namespace YAGO.World.Infrastructure.Database.Repositories
{
    internal class RepositoryForUpdateData : IRepositoryForUpdateData
    {
        private readonly ApplicationDbContext _context;

        public RepositoryForUpdateData(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Update(CancellationToken cancellationToken)
        {
            var unitsForUpdate = await _context.Units
                .Where(u => u.Type == UnitCommandType.Disbandment)
                .ToListAsync(cancellationToken);

            foreach (var unit in unitsForUpdate)
            {
                unit.Type = UnitCommandType.WarSupportDefense;
                unit.TargetDomainId = unit.DomainId;
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
