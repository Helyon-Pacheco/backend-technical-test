using OficinaMecanica.API.DTOs;
using OficinaMecanica.API.Models;
using OficinaMecanica.API.Repositories;
using OficinaMecanica.API.Validators;

namespace OficinaMecanica.API.Services;

public class OrcamentoService
{
    private readonly OrcamentoRepository _repository;
    private readonly OrcamentoValidator _validator;

    public OrcamentoService(OrcamentoRepository repository, OrcamentoValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public (CriarOrcamentoResponse? response, List<string> erros) CriarOrcamento(CriarOrcamentoRequest request)
    {
        var erros = _validator.Validar(request);
        if (erros.Count > 0)
            return (null, erros);

        var itens = request.Itens.Select(i => new OrcamentoItem
        {
            Descricao = i.Descricao.Trim(),
            Quantidade = i.Quantidade,
            ValorUnitario = i.ValorUnitario,
            ValorTotal = i.Quantidade * i.ValorUnitario
        }).ToList();

        var orcamento = new Orcamento
        {
            ClienteId = request.ClienteId,
            VeiculoId = request.VeiculoId,
            Itens = itens,
            ValorTotal = itens.Sum(i => i.ValorTotal),
            DataCriacao = DateTime.UtcNow
        };

        var salvo = _repository.Criar(orcamento);

        var response = new CriarOrcamentoResponse
        {
            Id = salvo.Id,
            ClienteId = salvo.ClienteId,
            VeiculoId = salvo.VeiculoId,
            Status = salvo.Status,
            ValorTotal = salvo.ValorTotal,
            DataCriacao = salvo.DataCriacao,
            Itens = salvo.Itens.Select(i => new OrcamentoItemResponse
            {
                Id = i.Id,
                Descricao = i.Descricao,
                Quantidade = i.Quantidade,
                ValorUnitario = i.ValorUnitario,
                ValorTotal = i.ValorTotal
            }).ToList()
        };

        return (response, []);
    }
}
