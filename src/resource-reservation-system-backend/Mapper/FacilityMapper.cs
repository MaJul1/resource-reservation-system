using System;
using resource_reservation_system_backend.DTO.Resource;
using resource_reservation_system_backend.Models;

namespace resource_reservation_system_backend.Mapper;

public static class FacilityMapper
{
  public static ResourceDTO ToResourceDTO(this Facility facility)
  {
    ResourceDTO dto = new ()
    {
      Id = facility.Id,
      Name = facility.Name,
      Type = facility.Type,
      Description = facility.Description
    };

    return dto;
  }

  public static Facility ToResource(this CreateResourceRequestDTO dto)
  {
    Facility facility = new ()
    {
      Name = dto.Name,
      Type = dto.Type,
      Description = dto.Description
    };

    return facility;
  }

  public static NameAndIdDTO ToNameAndIdDTO(this Facility facility)
  {
    NameAndIdDTO dto = new ()
    {
      Id = facility.Id,
      Name = facility.Name
    };

    return dto;
  }
}
