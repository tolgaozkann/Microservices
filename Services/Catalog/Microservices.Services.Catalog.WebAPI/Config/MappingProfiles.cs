using AutoMapper;
using Microservices.Services.Catalog.WebAPI.Dtos;
using Microservices.Services.Catalog.WebAPI.Models;

namespace Microservices.Services.Catalog.WebAPI.Config;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Course, CourseDto>().ReverseMap();
        CreateMap<Category,CategoryDto>().ReverseMap();
        CreateMap<Category, CategoryCreateDto>().ReverseMap();
        CreateMap<CategoryDto, CategoryCreateDto>().ReverseMap();
        CreateMap<Feature,FeatureDto>().ReverseMap();

        CreateMap<Course, CourseUpdateDto>().ReverseMap();
        CreateMap<Course, CourseCreateDto>().ReverseMap();

    }
}