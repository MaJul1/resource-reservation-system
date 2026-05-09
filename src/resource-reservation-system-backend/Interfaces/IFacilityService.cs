using System;
using resource_reservation_system_backend.DTO.Reservation;
using resource_reservation_system_backend.DTO.Facility;

namespace resource_reservation_system_backend.Interfaces;

public interface IFacilityService
{
  Task<IEnumerable<SummarizedFacilityDTO>> GetFacility(int page, int size, string sortBy);
  Task Create(CreateFacilityRequestDTO request);
  Task<DetailedFacilityDTO> GetFacilityById(int id); 
  Task<IEnumerable<NameAndIdDTO>> GetFacilityNamesAndId();
  Task Update(UpdateFacilityRequestDTO request);
  Task Delete(int id);
}
