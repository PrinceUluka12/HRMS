using HRMS.Application.Features.Equipments.Dtos;
using HRMS.Domain.Aggregates.OnboardingAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Common.Mapping
{
    public static class EquipmentMapper
    {
        public static EquipmentDto ToDto(this Equipment equipment)
        {
            return new EquipmentDto
            {
                SerialNumber = equipment.SerialNumber,
                AssetTag = equipment.AssetTag,
                Type = equipment.Type,
                Brand = equipment.Brand,
                Model = equipment.Model,
                Condition = equipment.Condition,
                PurchaseDate = equipment.PurchaseDate,
                WarrantyExpiry = equipment.WarrantyExpiry,
                Status = equipment.Status,
                Notes = equipment.Notes
            };
        }
        public static List<EquipmentDto> ToDtoList(this IEnumerable<Equipment> equipments)
        {
            return equipments.Select(e => e.ToDto()).ToList();
        }
    }
}
