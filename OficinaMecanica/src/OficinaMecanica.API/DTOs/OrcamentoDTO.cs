namespace OficinaMecanica.API.DTOs;

public class CriarOrcamentoRequest
{
    public int ClienteId { get; set; }
    public int VeiculoId { get; set; }
    public List<OrcamentoItemRequest> Itens { get; set; } = [];
}

public class OrcamentoItemRequest
{
    public string Descricao { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
}

public class CriarOrcamentoResponse
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public int VeiculoId { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal ValorTotal { get; set; }
    public DateTime DataCriacao { get; set; }
    public List<OrcamentoItemResponse> Itens { get; set; } = [];
}

public class OrcamentoItemResponse
{
    public int Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
    public decimal ValorTotal { get; set; }
}

public class ErroResponse
{
    public string Mensagem { get; set; } = string.Empty;
    public List<string> Erros { get; set; } = [];
}
