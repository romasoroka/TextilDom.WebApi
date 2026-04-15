using Luzanov.Application.NovaPoshta.Commands;
using Luzanov.Application.NovaPoshta.Dtos;

namespace Luzanov.Application.Services.Abstractions
{
    public interface INovaPoshtaService
    {
        Task<IEnumerable<CityDto>> SearchCitiesAsync(string query);
        Task<IEnumerable<WarehouseDto>> GetWarehousesAsync(string cityRef);
        Task<NovaTtnResultDto> CreateShipmentAsync(CreateTtnCommand command);
        Task<(string RecipientRef, string ContactRef)> CreateRecipientAsync(string lastName, string firstName, string phone, string cityRef);
    }
}
