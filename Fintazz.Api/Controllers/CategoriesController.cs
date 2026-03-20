namespace Fintazz.Api.Controllers;

using Fintazz.Api.Infrastructure;
using Fintazz.Application.Categories.Commands.CreateCategory;
using Fintazz.Application.Categories.Commands.DeleteCategory;
using Fintazz.Application.Categories.Commands.UpdateCategory;
using Fintazz.Application.Categories.Queries.GetCategoriesByHouseHold;
using Fintazz.Application.Categories.Queries.GetCategoryById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Gerenciamento de categorias financeiras de um HouseHold.
/// </summary>
[Authorize]
[Route("api/categories")]
public class CategoriesController : BaseApiController
{
    private readonly ISender _sender;

    public CategoriesController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Cria uma nova categoria financeira.
    /// </summary>
    /// <response code="200">Retorna o ID da categoria criada.</response>
    /// <response code="400">Nome duplicado ou dados inválidos.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateCategoryCommand(request.HouseHoldId, request.Name, request.Type, CurrentUserId);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(new { id = result.Value });
    }

    /// <summary>
    /// Lista todas as categorias de um HouseHold.
    /// </summary>
    /// <response code="200">Lista de categorias ordenada por nome.</response>
    [HttpGet("house-hold/{houseHoldId}")]
    [ProducesResponseType(typeof(IEnumerable<CategoryResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByHouseHold(Guid houseHoldId, CancellationToken cancellationToken)
    {
        var query = new GetCategoriesByHouseHoldQuery(houseHoldId);
        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    /// <summary>
    /// Retorna uma categoria específica pelo ID.
    /// </summary>
    /// <response code="200">Categoria encontrada.</response>
    /// <response code="404">Categoria não encontrada.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetCategoryByIdQuery(id);
        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            if (result.Error.Code == "Category.NotFound")
                return NotFound(result.Error);

            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Renomeia uma categoria existente.
    /// </summary>
    /// <response code="204">Categoria atualizada com sucesso.</response>
    /// <response code="400">Nome duplicado ou dados inválidos.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateCategoryCommand(id, request.Name);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return NoContent();
    }

    /// <summary>
    /// Exclui uma categoria. Não é possível excluir categorias em uso.
    /// </summary>
    /// <response code="204">Categoria excluída com sucesso.</response>
    /// <response code="400">Categoria em uso ou não encontrada.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteCategoryCommand(id);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return NoContent();
    }
}

public record CreateCategoryRequest(Guid HouseHoldId, string Name, Fintazz.Domain.Entities.CategoryType Type);
public record UpdateCategoryRequest(string Name);
