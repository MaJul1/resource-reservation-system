using System;
using resource_reservation_system_backend.DTO.Resource;
using resource_reservation_system_backend.Models;

namespace resource_reservation_system_backend.Mapper;

public static class ResourceMapper
{
  public static ResourceDTO ToResourceDTO(this Resource resource)
  {
    ResourceDTO dto = new ()
    {
      Id = resource.Id,
      Name = resource.Name,
      Type = resource.Type,
      Description = resource.Description
    };

    return dto;
  }
}
