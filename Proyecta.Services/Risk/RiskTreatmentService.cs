using Proyecta.Core.Contracts.Repositories.Risk;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.DTOs;
using Proyecta.Core.DTOs.Risk;
using Proyecta.Core.Entities.Risk;
using Proyecta.Core.Results;

namespace Proyecta.Services.Risk;

public sealed class RiskTreatmentService : IRiskTreatmentService
{
    private readonly IRiskTreatmentRepository _repository;

    public RiskTreatmentService(IRiskTreatmentRepository repository)
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

    public async Task<ApplicationResult> Create(RiskTreatmentCreateOrUpdateDto item, string currentUserId)
    {
        var newItem = MapDtoToEntity(item, currentUserId);

        await _repository.Create(newItem);

        return new ApplicationResult
        {
            Success = true,
            Code = "201",
            Message = "User created successfully.",
            Data = new { newItem.Id }
        };
    }

    public async Task<ApplicationResult> Update(Guid id, RiskTreatmentCreateOrUpdateDto item, string currentUserId)
    {
        var itemToUpdate = MapDtoToEntity(item, currentUserId);

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

    public async Task<ApplicationResult> AddRange(IEnumerable<RiskTreatmentCreateOrUpdateDto> items, string currentUserId)
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
    
    private RiskTreatment MapDtoToEntity(RiskTreatmentCreateOrUpdateDto item, string currentUserId)
    {
        var now = DateTime.UtcNow;

        var entity = new RiskTreatment
        {
            Name = item.Name,
            CreatedAt = now,
            CreatedById = currentUserId,
            UpdatedAt = now,
            UpdatedById = currentUserId
        };

        return entity;
    }
}