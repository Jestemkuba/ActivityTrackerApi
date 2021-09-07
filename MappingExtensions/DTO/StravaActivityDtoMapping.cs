using ActivityTrackerApi.Data.DTOs.Activities;
using ActivityTrackerApi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityTrackerApi.MappingExtensions.DTO
{
    public static class StravaActivityDtoMapping
    {
        public static Activity ToActivity(this StravaActivityDto dto)
        {
            return new Activity
            {
                StravaId = dto.Id,
                Name = dto.Name,
                Distance = dto.Distance,
                MovingTime = dto.MovingTime,
                ElapsedTime = dto.ElapsedTime,
                Type = dto.Type,
                AverageSpeed = dto.AverageSpeed,
                MaxSpeed = dto.MaxSpeed,
            };
        }
    }
}
