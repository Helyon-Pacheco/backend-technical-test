using OficinaMecanica.API.DTOs;
using OficinaMecanica.API.Repositories;
using OficinaMecanica.API.Services;
using OficinaMecanica.API.Validators;

namespace OficinaMecanica.Tests;

public class OrcamentoServiceTests
{
    private static OrcamentoService CriarService() =>
        new(new OrcamentoRepository(), new OrcamentoValidator());

    private static CriarOrcamentoRequest RequestValido() => new()
    {
        ClienteId = 10,
        VeiculoId = 25,
        Itens =
        [
            new() { Descricao = "Troca de óleo", Quantidade = 1, ValorUnitario = 120.00m },
            new() { Descricao = "Filtro de óleo", Quantidade = 1, ValorUnitario = 45.00m }
        ]
    };

    [Fact]
    public void CriarOrcamento_DeveRetornarSucesso_QuandoDadosValidos()
    {
        var service = CriarService();
        var (response, erros) = service.CriarOrcamento(RequestValido());

        Assert.Empty(erros);
        Assert.NotNull(response);
        Assert.Equal(10, response.ClienteId);
        Assert.Equal(25, response.VeiculoId);
        Assert.Equal("Aberto", response.Status);
    }

    [Fact]
    public void CriarOrcamento_DeveCalcularValorTotalCorretamente()
    {
        var service = CriarService();
        var (response, _) = service.CriarOrcamento(RequestValido());

        Assert.Equal(165.00m, response!.ValorTotal);
    }

    [Fact]
    public void CriarOrcamento_DeveCalcularValorTotalDeItensComQuantidadeMaiorQue1()
    {
        var service = CriarService();
        var request = RequestValido();
        request.Itens[0].Quantidade = 3;

        var (response, _) = service.CriarOrcamento(request);

        Assert.Equal(405.00m, response!.ValorTotal);
        Assert.Equal(360.00m, response.Itens[0].ValorTotal);
    }

    [Fact]
    public void CriarOrcamento_DeveRetornarIdIncrementalACadaCriacao()
    {
        var service = CriarService();
        var (r1, _) = service.CriarOrcamento(RequestValido());
        var (r2, _) = service.CriarOrcamento(RequestValido());

        Assert.Equal(r1!.Id + 1, r2!.Id);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CriarOrcamento_DeveRetornarErro_QuandoClienteIdInvalido(int clienteId)
    {
        var service = CriarService();
        var request = RequestValido();
        request.ClienteId = clienteId;

        var (response, erros) = service.CriarOrcamento(request);

        Assert.Null(response);
        Assert.Contains(erros, e => e.Contains("clienteId"));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void CriarOrcamento_DeveRetornarErro_QuandoVeiculoIdInvalido(int veiculoId)
    {
        var service = CriarService();
        var request = RequestValido();
        request.VeiculoId = veiculoId;

        var (response, erros) = service.CriarOrcamento(request);

        Assert.Null(response);
        Assert.Contains(erros, e => e.Contains("veiculoId"));
    }

    [Fact]
    public void CriarOrcamento_DeveRetornarErro_QuandoSemItens()
    {
        var service = CriarService();
        var request = RequestValido();
        request.Itens = [];

        var (response, erros) = service.CriarOrcamento(request);

        Assert.Null(response);
        Assert.Contains(erros, e => e.Contains("pelo menos 1 item"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void CriarOrcamento_DeveRetornarErro_QuandoDescricaoVazia(string descricao)
    {
        var service = CriarService();
        var request = RequestValido();
        request.Itens[0].Descricao = descricao;

        var (response, erros) = service.CriarOrcamento(request);

        Assert.Null(response);
        Assert.Contains(erros, e => e.Contains("descrição"));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CriarOrcamento_DeveRetornarErro_QuandoQuantidadeInvalida(int quantidade)
    {
        var service = CriarService();
        var request = RequestValido();
        request.Itens[0].Quantidade = quantidade;

        var (response, erros) = service.CriarOrcamento(request);

        Assert.Null(response);
        Assert.Contains(erros, e => e.Contains("quantidade"));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void CriarOrcamento_DeveRetornarErro_QuandoValorUnitarioInvalido(decimal valor)
    {
        var service = CriarService();
        var request = RequestValido();
        request.Itens[0].ValorUnitario = valor;

        var (response, erros) = service.CriarOrcamento(request);

        Assert.Null(response);
        Assert.Contains(erros, e => e.Contains("valorUnitario"));
    }

    [Fact]
    public void CriarOrcamento_DeveRetornarTodosOsErros_QuandoMultiplosItensInvalidos()
    {
        var service = CriarService();
        var request = new CriarOrcamentoRequest
        {
            ClienteId = 0,
            VeiculoId = 0,
            Itens =
            [
                new() { Descricao = "",  Quantidade = 0, ValorUnitario = 0 },
                new() { Descricao = "Ok", Quantidade = 0, ValorUnitario = 0 }
            ]
        };

        var (response, erros) = service.CriarOrcamento(request);

        Assert.Null(response);
        Assert.True(erros.Count >= 5);
    }
}