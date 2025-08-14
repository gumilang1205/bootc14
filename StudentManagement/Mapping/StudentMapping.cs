using AutoMapper;
using StudentManagement.DTOs;
using StudentManagement.Models;
namespace StudentManagement.Mapping
{
    public class StudentMappingProfile : Profile
    {
        public StudentMappingProfile()
        {
            CreateMap<Student, StudentDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.StudentID))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            CreateMap<StudentDto, Student>()
                .ForMember(dest => dest.StudentID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.StudentNumber, opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

        }
    }
}