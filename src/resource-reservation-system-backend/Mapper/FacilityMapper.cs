using System;
using resource_reservation_system_backend.DTO.Facility;
using resource_reservation_system_backend.Models;

namespace resource_reservation_system_backend.Mapper;

public static class FacilityMapper
{
  public static DetailedFacilityDTO ToFacilityDTO(this Facility facility)
  {
    DetailedFacilityDTO dto = new ()
    {
      Id = facility.Id,
      Name = facility.Name,
      Type = facility.Type,
      Location = facility.Location,
      Capacity = facility.Capacity,
      Description = facility.Description,
      ItemsAllocated = facility.ItemsAllocated.Select(itemAllocation => itemAllocation.ToItemAllocationDTO())
    };

    return dto;
  }

  public static Facility ToFacility(this CreateFacilityRequestDTO dto)
  {
    Facility facility = new ()
    {
      Name = dto.Name,
      Type = dto.Type,
      Location = dto.Location,
      Capacity = dto.Capacity,
      Description = dto.Description
    };

    return facility;
  }

  public static SummarizedFacilityDTO ToSummarizedFacilityDTO(this Facility facility)
  {
    SummarizedFacilityDTO dto = new ()
    {
      Id = facility.Id,
      Name = facility.Name,
      Type = facility.Type,
      Location = facility.Location,
      Capacity = facility.Capacity,
      Description = facility.Description
    };

    return dto;
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
