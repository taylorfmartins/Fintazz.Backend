using Fintazz.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Fintazz.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DiagnosticController : ControllerBase
{
    private readonly MongoContext _mongoContext;

    public DiagnosticController(MongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }

    [HttpGet("ping-database")]
    public async Task<IActionResult> PingDatabase()
    {
        try
        {
            // O comando 'ping' é a forma padrão de testar conexões vitais com o MongoDB
            var database = _mongoContext.GetCollection<object>("system.profile").Database;
            var command = new MongoDB.Bson.BsonDocument { { "ping", 1 } };
            
            await database.RunCommandAsync<MongoDB.Bson.BsonDocument>(command);

            return Ok(new 
            { 
                Status = "Success", 
                Message = "Conexão com o banco de dados MongoDB Atlas (FintazzDb) estabelecida com sucesso! 🚀",
                Timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new 
            { 
                Status = "Error", 
                Message = "Falha ao conectar no MongoDB Atlas.",
                Details = ex.Message
            });
        }
    }
}
