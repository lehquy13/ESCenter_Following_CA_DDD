namespace Matt.SharedKernel.Application.Contracts.Interfaces;

public interface IEntityDto<TId>
{
    TId Id { get; set; }
}