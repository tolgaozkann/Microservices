using AutoMapper;
using Microservices.Services.Catalog.WebAPI.Config;
using Microservices.Services.Catalog.WebAPI.Dtos;
using Microservices.Services.Catalog.WebAPI.Models;
using Microservices.Services.Catalog.WebAPI.Services.Abstract;
using Microservices.Shared.Dtos;
using MongoDB.Driver;

namespace Microservices.Services.Catalog.WebAPI.Services.Concrete;

public class CourseService : ICourseService
{
    private readonly IMongoCollection<Course> _courses;
    private readonly IMongoCollection<Category> _categories;
    private readonly IMapper _mapper;

    public CourseService(IMapper mapper, IDatabaseSettings databaseSettings)
    {
        var client = new MongoClient(databaseSettings.ConntectionString);

        var database = client.GetDatabase(databaseSettings.DatabaseName);

        _courses = database.GetCollection<Course>(databaseSettings.CourseCollectionName);
        _categories = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);

        _mapper = mapper;

    }


    public async Task<ResponseDto<List<CourseDto>>> GetAllAsync()
    {
        var courses = await _courses.Find(courses => true).ToListAsync();

        if (courses.Any())
        {
            foreach (var course in courses)
            {
                course.Category = await _categories.Find(c=>c.Id.Equals(course.CategoryId)).FirstAsync();
            }
        }
        else
        {
            courses = new List<Course>();
        }

        return ResponseDto<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
    }

    public async Task<ResponseDto<CourseDto>> CreateAsync(CourseCreateDto courseCreateDto)
    {
        var newCourse = _mapper.Map<Course>(courseCreateDto);

        newCourse.CreatedTime = DateTime.Now;

        await _courses.InsertOneAsync(newCourse);

        return ResponseDto<CourseDto>.Success(_mapper.Map<CourseDto>(newCourse), 200);
    }

    public async Task<ResponseDto<CourseDto>> GetByIdAsync(string id)
    {
        var course = await _courses.Find(c => c.Id.Equals(id)).FirstOrDefaultAsync();

        if (course is null)
            return ResponseDto<CourseDto>.Fail($"Course with id:{id} not found", 404);

        course.Category = await _categories.Find(c => c.Id.Equals(course.CategoryId)).FirstAsync();


        return ResponseDto<CourseDto>.Success(_mapper.Map<CourseDto>(course), 200);
    }

    public async Task<ResponseDto<List<CourseDto>>> GetAllByUserIdAsync(string userId)
    {
        var courses = await _courses.Find(c=>c.UserId.Equals(userId)).ToListAsync();

        if (courses.Any())
        {
            foreach (var course in courses)
            {
                course.Category = await _categories.Find(c => c.Id.Equals(course.CategoryId)).FirstAsync();
            }
        }
        else
        {
            courses = new List<Course>();
        }

        return ResponseDto<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses),200);
    }

    public async Task<ResponseDto<NoContent>> UpdateAsync(CourseUpdateDto courseUpdateDto)
    {
        var course = _mapper.Map<Course>(courseUpdateDto);

        var result = await _courses.FindOneAndReplaceAsync(c => c.Id.Equals(course.Id),course);

        if(result is null)
            return ResponseDto<NoContent>.Fail($"There is no record with this id:{courseUpdateDto.Id}",404);

        return ResponseDto<NoContent>.Success(204);
    }

    public async Task<ResponseDto<NoContent>> DeleteAsync(string id)
    {
        var result = await _courses.FindOneAndDeleteAsync(c => c.Id.Equals(id));

        if(result is null)
            return ResponseDto<NoContent>.Fail($"There is no record with this id:{id}", 404);

        return ResponseDto<NoContent>.Success(204);
    }
}