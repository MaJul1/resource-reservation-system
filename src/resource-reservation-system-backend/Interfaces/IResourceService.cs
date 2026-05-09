using System;
using resource_reservation_system_backend.DTO.Reservation;
using resource_reservation_system_backend.DTO.Facility;

namespace resource_reservation_system_backend.Interfaces;

public interface IFacilityService
{
  Task<IEnumerable<FacilityDTO>> GetFacility(int page, int size, string sortBy);
  Task Create(CreateFacilityRequestDTO request);
  Task<FacilityDTO> GetFacilityById(int id); 
  Task<IEnumerable<NameAndIdDTO>> GetFacilityNamesAndId();
}
