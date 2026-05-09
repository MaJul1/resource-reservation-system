using System;
using resource_reservation_system_backend.DTO.Reservation;
using resource_reservation_system_backend.DTO.Resource;

namespace resource_reservation_system_backend.Interfaces;

public interface IFacilityService
{
  Task<IEnumerable<ResourceDTO>> GetFacility(int page, int size, string sortBy);
  Task Create(CreateFacilityRequestDTO request);
  Task<ResourceDTO> GetFacilityById(int id); 
  Task<IEnumerable<NameAndIdDTO>> GetFacilityNamesAndId();
}
