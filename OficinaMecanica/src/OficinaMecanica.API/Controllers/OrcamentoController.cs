using Microsoft.AspNetCore.Mvc;
using OficinaMecanica.API.DTOs;
using OficinaMecanica.API.Services;

namespace OficinaMecanica.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrcamentoController : Controller
{
    private readonly OrcamentoService _service;

    public OrcamentoController(OrcamentoService orcamentoService)
    {
        _service = orcamentoService;
    }

    /// <summary>
    /// Cadastra um novo orçamento.
    /// </summary>
    /// <response code="201">Orçamento criado com sucesso.</response>
    /// <response code="400">Dados inválidos — retorna lista de erros.</response>
    [HttpPost]
    [ProducesResponseType(typeof(CriarOrcamentoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status400BadRequest)]
    public IActionResult CriarOrcamento([FromBody] CriarOrcamentoRequest request)
    {
        var (response, erros) = _service.CriarOrcamento(request);

        if (erros.Count > 0)
        { 
            return BadRequest(new ErroResponse
            {
                Mensagem = "Requisição inválida. Corrija os erros abaixo.",
                Erros = erros
            });
        }

        return CreatedAtAction(
            nameof(BuscarPorId),
            new { id = response!.Id },
            response
        );
    }

    /// <summary>
    /// Busca um orçamento pelo Id.
    /// </summary>
    /// <response code="200">Orçamento encontrado.</response>
    /// <response code="404">Orçamento não encontrado.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CriarOrcamentoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult BuscarPorId(int id)
    {
        return NotFound();
    }
}
