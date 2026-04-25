using Textildom.Application.NovaPoshta.Commands;
using Textildom.Application.NovaPoshta.Dtos;

namespace Textildom.Application.Services.Abstractions
{
    public interface INovaPoshtaService
    {
        Task<IEnumerable<CityDto>> SearchCitiesAsync(string query);
        Task<IEnumerable<WarehouseDto>> GetWarehousesAsync(string cityRef);
        Task<NovaTtnResultDto> CreateShipmentAsync(CreateTtnCommand command);
        Task<(string RecipientRef, string ContactRef)> CreateRecipientAsync(string lastName, string firstName, string phone, string cityRef);
    }
}
