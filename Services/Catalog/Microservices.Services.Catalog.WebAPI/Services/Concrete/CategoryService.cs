using AutoMapper;
using Microservices.Services.Catalog.WebAPI.Config;
using Microservices.Services.Catalog.WebAPI.Dtos;
using Microservices.Services.Catalog.WebAPI.Models;
using Microservices.Services.Catalog.WebAPI.Services.Abstract;
using Microservices.Shared.Dtos;
using MongoDB.Driver;

namespace Microservices.Services.Catalog.WebAPI.Services.Concrete;

public class CategoryService : ICategoryService
{
    private readonly IMongoCollection<Category> _categories;
    private readonly IMapper _mapper;

    public CategoryService(IMapper mapper, IDatabaseSettings databaseSettings)
    {
        var client = new MongoClient(databaseSettings.ConntectionString);

        var database = client.GetDatabase(databaseSettings.DatabaseName);
        _categories = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
        _mapper = mapper;
    }

    public async Task<ResponseDto<List<CategoryDto>>> GetAllAsync()
    {
        var categories = await _categories.Find(categories => true).ToListAsync();

        return ResponseDto<List<CategoryDto>>.Success(_mapper.Map<List<CategoryDto>>(categories), 200);
    }

    public async Task<ResponseDto<CategoryDto>> CreateAsync(CategoryCreateDto categoryCreateDto)
    {
        await _categories.InsertOneAsync(_mapper.Map<Category>(categoryCreateDto));

        return ResponseDto<CategoryDto>.Success(_mapper.Map<CategoryDto>(categoryCreateDto), 200);
    }

    public async Task<ResponseDto<CategoryDto>> GetByIdAsync(string id)
    {
        var category = await _categories.Find(c => c.Id.Equals(id)).SingleOrDefaultAsync();

        if (category is null)
            return ResponseDto<CategoryDto>.Fail($"Category with id:{id} not found", 404);

        return ResponseDto<CategoryDto>.Success(_mapper.Map<CategoryDto>(category), 200);
    }

    public async Task<ResponseDto<NoContent>> UpdateAsync(CategoryDto category)
    {
        var newCategory = _mapper.Map<Category>(category);

        var result = await _categories.FindOneAndReplaceAsync(c => c.Id.Equals(newCategory.Id), newCategory);

        if(result is null)
            return ResponseDto<NoContent>.Fail($"There is no record with this id:{category.Id}",404);

        return ResponseDto<NoContent>.Success(204);
    }

    public async Task<ResponseDto<NoContent>> DeleteAsync(string id)
    {
        var result = await _categories.FindOneAndDeleteAsync(c => c.Id.Equals(id));

        if(result is null)
            return ResponseDto<NoContent>.Fail($"There is no record with this id:{id}", 404);

        return ResponseDto<NoContent>.Success(204);
    }
}