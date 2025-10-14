using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Users;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Application.Colonies
{
    public class ColonyService : IColonyService
    {
        public readonly IUserService _userService;
        private readonly IColonyRepository _colonyRepository;

        public ColonyService(
            IUserService userService,
            IColonyRepository colonyRepository)
        {
            _userService = userService;
            _colonyRepository = colonyRepository;
        }

        public async Task<Colony> CreateColony(ClaimsPrincipal userClaimsPrincipal, string name, ColonyPresetType presetType, CancellationToken cancellationToken)
        {
            var myUser = await _userService.GetMyUser(userClaimsPrincipal, cancellationToken);
            if (myUser == null)
                throw new YagoNotAuthorizedException();

            var userColony = await _colonyRepository.FindByUserId(myUser.Id, cancellationToken);
            if (userColony != null)
                throw new YagoException(string.Format("Пользователь '{0}' уже имеет колонию '{1}'.", myUser.UserName, userColony.Name));

            var colonyWithName = await _colonyRepository.FindByName(name, cancellationToken);
            if (colonyWithName != null)
                throw new YagoException(string.Format("Название колонии '{0}' уже занято.", name));

            var createColonyDto = new CreateColonyDto(
                myUser.Id,
                name,
                presetType);
            return await _colonyRepository.CreateColomy(createColonyDto, cancellationToken);
        }

        public async Task<Colony?> GetMyColony(ClaimsPrincipal userClaimsPrincipal, CancellationToken cancellationToken)
        {
            var myUser = await _userService.GetMyUser(userClaimsPrincipal, cancellationToken);
            return myUser == null ? null : await _colonyRepository.FindByUserId(myUser.Id, cancellationToken);
        }
    }
}
