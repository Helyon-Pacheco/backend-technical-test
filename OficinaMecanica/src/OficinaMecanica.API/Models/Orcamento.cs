namespace OficinaMecanica.API.Models;

public class Orcamento
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public int VeiculoId { get; set; }
    public string Status { get; set; } = "Aberto";
    public decimal ValorTotal { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataFinalizacao { get; set; }

    public List<OrcamentoItem> Itens { get; set; } = [];
}

public class OrcamentoItem
{
    public int Id { get; set; }
    public int OrcamentoId { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
    public decimal ValorTotal { get; set; }
}