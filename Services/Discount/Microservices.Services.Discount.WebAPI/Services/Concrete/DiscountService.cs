using System.Data;
using Dapper;
using Microservices.Services.Discount.WebAPI.Services.Abstract;
using Microservices.Shared.Dtos;
using Npgsql;

namespace Microservices.Services.Discount.WebAPI.Services.Concrete;

public class DiscountService : IDiscountService
{
    private readonly IConfiguration _configuration;
    private readonly IDbConnection _dbConnection;

    public DiscountService(IConfiguration configuration)
    {
        _configuration = configuration;
        _dbConnection = new NpgsqlConnection(configuration.GetConnectionString("PostgreSql"));
    }

    public async Task<ResponseDto<List<Models.Discount>>> GetAll()
    {
        var discounts = await _dbConnection.QueryAsync<Models.Discount>("SELECT * FROM discount");
        return ResponseDto<List<Models.Discount>>.Success(discounts.ToList(), 200);
    }

    public async Task<ResponseDto<Models.Discount>> GetById(int id)
    {
        var discount = (await _dbConnection
            .QueryAsync<Models.Discount>("SELECT * FROM discount WHERE id = @Id",
                new { id })).SingleOrDefault();

        return discount is not null ? ResponseDto<Models.Discount>.Success(discount, 200) :
                                      ResponseDto<Models.Discount>.Fail("Discount not found", 404);
    }

    public async Task<ResponseDto<NoContent>> Save(Models.Discount discount)
    {
        var result = await _dbConnection
            .ExecuteAsync("INSERT INTO discount (userid,rate,code) VALUES(@UserId,@Rate,@Code)", discount);

        return result > 0 ? ResponseDto<NoContent>.Success(202) : 
                            ResponseDto<NoContent>.Fail("Discount Can Not Be Added", 500);
    }

    public async Task<ResponseDto<NoContent>> DeleteById(int id)
    {
        var discount = (await _dbConnection
            .QueryAsync<Models.Discount>("SELECT * FROM discount WHERE id=@Id",
                new { id })).SingleOrDefault();

        if (discount is null)
            return ResponseDto<NoContent>.Fail("Discount Not Found", 400);

        var result = (await _dbConnection.ExecuteAsync("DELETE FROM discount WHERE id=@Id", new { id }));


        return result > 0 ? ResponseDto<NoContent>.Success(204) : 
                            ResponseDto<NoContent>.Fail("Discount can not be deleted.", 500);
    }

    public async Task<ResponseDto<NoContent>> Update(Models.Discount discount)
    {
        var discountCheck = (await _dbConnection.QueryAsync<Models.Discount>("SELECT * FROM discount WHERE id=@Id", new { Id = discount.Id })).SingleOrDefault();

        if (discountCheck is null)
            return ResponseDto<NoContent>.Fail("Discount Not Found.", 404);

        var result = await _dbConnection
                .ExecuteAsync("UPDATE discount SET userid=@UserId,code=@Code,rate=@Rate WHERE id=@Id", discount);

        return result > 0 ? ResponseDto<NoContent>.Success(202) : 
                            ResponseDto<NoContent>.Fail("Discount can not be updated", 500);
    }

    public async Task<ResponseDto<Models.Discount>> GetByCodeAndUserId(string userId, string code)
    {
        var discount = (await _dbConnection.QueryAsync<Models.Discount>(
            "SELECT * FROM discount WHERE userid=@UserId AND code=@Code", new
            {
                UserId = userId,
                Code = code
            })).FirstOrDefault();

        return discount is not null ? ResponseDto<Models.Discount>.Success(discount, 200) :
                                      ResponseDto<Models.Discount>.Fail("Discount Not Found.", 404);
    }
}