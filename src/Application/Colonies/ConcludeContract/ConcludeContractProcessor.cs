using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Contracts;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Application.Colonies.ConcludeContract
{
    public class ConcludeContractProcessor : IConcludeContractProcessor
    {
        private readonly IColonyRepository _colonyRepository;
        private readonly IColonyWithShipAndContractsRepository _colonyWithShipAndContractsRepository;

        public ConcludeContractProcessor(
            IColonyRepository colonyRepository,
            IColonyWithShipAndContractsRepository colonyWithShipAndContractsRepository)
        {
            _colonyRepository = colonyRepository;
            _colonyWithShipAndContractsRepository = colonyWithShipAndContractsRepository;
        }

        public async Task<ConcludeContractResult> Execute(ConcludeContractCommand command, CancellationToken cancellationToken)
        {
            var colony = await _colonyRepository.FindByUserId(command.UserId, cancellationToken)
                ?? throw new YagoException("Пользователь не имеет колонии.");

            var allContracts = ContractDataset.Get().ToList();
            var contract = allContracts.Find(x => x.Id == command.СontractId)
                ?? throw new YagoNotFoundException(nameof(Contract), command.СontractId);

            var colonyWithShipAndContractsDto = await _colonyWithShipAndContractsRepository.Find(colony.Id, cancellationToken)
                ?? throw new YagoNotFoundException(nameof(ColonyWithShipAndContracts), colony.Id);

            colonyWithShipAndContractsDto.СoncludeСontract(contract, colonyWithShipAndContractsDto);
            await _colonyRepository.Update(colonyWithShipAndContractsDto.Colony, cancellationToken);

            return new ConcludeContractResult(colonyWithShipAndContractsDto);
        }
    }
}
