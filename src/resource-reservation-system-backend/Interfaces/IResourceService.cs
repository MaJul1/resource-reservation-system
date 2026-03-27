using System;
using resource_reservation_system_backend.DTO.Reservation;
using resource_reservation_system_backend.DTO.Resource;

namespace resource_reservation_system_backend.Interfaces;

public interface IResourceService
{
  Task<IEnumerable<ResourceDTO>> GetResources(int page, int size, string sortBy);
  Task Create(CreateResourceRequestDTO request);
  Task<ResourceDTO> GetResourceById(int id); 
}
