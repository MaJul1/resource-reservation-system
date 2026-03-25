using System;
using resource_reservation_system_backend.DTO.User;
using resource_reservation_system_backend.Models;

namespace resource_reservation_system_backend.Mapper;

public static class UserMapper
{
  public static UserDTO ToUserDTO(this User user)
  {
    UserDTO dto = new ()
    {
      Id = user.Id,
      FirstName = user.FirstName,
      LastName = user.LastName,
      Email = user.Email,
      PhoneNumber = user.PhoneNumber
    };

    return dto;
  }
}
