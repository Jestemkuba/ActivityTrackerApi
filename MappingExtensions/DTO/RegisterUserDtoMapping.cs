using ActivityTrackerApi.Data.DTOs;
using ActivityTrackerApi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityTrackerApi.MappingExtensions.DTO
{
    public static class RegisterUserDtoMapping
    {
        public static ApplicationUser ToApplicationUser(this RegisterUserDto dto)
        {
            return new ApplicationUser
            {
                UserName = dto.Username,
                Email = dto.Email,
            };
        }
    }
}
