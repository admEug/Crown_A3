using App.Application.Features.Employees.Queries.GetEmployees;
using App.Application.Features.Positions.Commands.CreatePosition;
using App.Application.Features.Positions.Queries.GetPositions;
using App.Domain.Entities;
using AutoMapper;

namespace App.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Position, GetPositionsViewModel>().ReverseMap();
            CreateMap<Employee, GetEmployeesViewModel>().ReverseMap();
            CreateMap<CreatePositionCommand, Position>();
        }
    }
}