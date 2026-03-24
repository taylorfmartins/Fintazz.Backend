using Fintazz.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Fintazz.Api.Controllers;

/// <summary>
/// Endpoints de diagnóstico interno — verificação de conectividade e saúde da infraestrutura.
/// </summary>
[ApiController]
[Route("api/diagnostic")]
public class DiagnosticController : ControllerBase
{
    private readonly MongoContext _mongoContext;

    public DiagnosticController(MongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }

    /// <summary>
    /// Verifica a conectividade com o banco de dados MongoDB Atlas.
    /// </summary>
    /// <response code="200">Conexão estabelecida com sucesso.</response>
    /// <response code="500">Falha ao conectar ao banco de dados.</response>
    [HttpGet("ping-database")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
