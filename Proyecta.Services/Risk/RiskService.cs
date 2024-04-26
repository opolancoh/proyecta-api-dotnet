using Proyecta.Core.Contracts.Repositories.Risk;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.DTOs.Risk;
using Proyecta.Core.Entities.Risk;
using Proyecta.Core.Results;

namespace Proyecta.Services.Risk;

public sealed class RiskService : IRiskService
{
    private readonly IRiskRepository _repository;

    public RiskService(IRiskRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApplicationResult> GetAll()
    {
        var items = await _repository.GetAll();

        return new ApplicationResult
        {
            Success = true,
            Code = "200",
            Data = items
        };
    }

    public async Task<ApplicationResult> GetById(Guid id)
    {
        var item = await _repository.GetById(id);

        if (item == null)
        {
            return new ApplicationResult
            {
                Success = false,
                Code = "404",
                Message = $"The entity with id '{id}' doesn't exist in the database."
            };
        }

        return new ApplicationResult
        {
            Success = true,
            Code = "200",
            Data = item
        };
    }

    public async Task<ApplicationResult> Create(RiskCreateOrUpdateDto item, string currentUserId)
    {
        var newItem = MapDtoToEntity(item, currentUserId);

        await _repository.Create(newItem);

        return new ApplicationResult
        {
            Success = true,
            Code = "201",
            Message = "Risk created successfully.",
            Data = new { newItem.Id }
        };
    }

    public async Task<ApplicationResult> Update(Guid id, RiskCreateOrUpdateDto item, string currentUserId)
    {
        var itemToUpdate = MapDtoToEntity(item, currentUserId);
        itemToUpdate.Id = id;

        await _repository.Update(itemToUpdate);

        return new ApplicationResult
        {
            Success = true,
            Code = "204",
            Message = "Item updated successfully.",
        };
    }

    public async Task<ApplicationResult> Remove(Guid id, string currentUserId)
    {
        await _repository.Remove(id);

        return new ApplicationResult
        {
            Success = true,
            Code = "204",
            Message = "Item deleted successfully.",
        };
    }

    public async Task<ApplicationResult> AddRange(IEnumerable<RiskCreateOrUpdateDto> items, string currentUserId)
    {
        var newItems = items.Select(item => MapDtoToEntity(item, currentUserId)).ToList();

        await _repository.AddRange(newItems);

        return new ApplicationResult
        {
            Success = true,
            Code = "204",
            Message = "Items added successfully.",
        };
    }

    private Core.Entities.Risk.Risk MapDtoToEntity(RiskCreateOrUpdateDto item, string currentUserId)
    {
        var now = DateTime.UtcNow;

        var entity = new Core.Entities.Risk.Risk
        {
            Name = item.Name,
            Code = item.Code,
            CategoryId = item.Category,
            Type = (RiskType)item.Type,
            OwnerId = item.Owner,
            Phase = (RiskPhase)item.Phase,
            Manageability = (RiskManageability)item.Manageability,
            TreatmentId = item.Treatment,
            DateFrom = item.DateFrom,
            DateTo = item.DateTo,
            State = item.State,
            CreatedAt = now,
            CreatedById = currentUserId,
            UpdatedAt = now,
            UpdatedById = currentUserId
        };

        return entity;
    }
}