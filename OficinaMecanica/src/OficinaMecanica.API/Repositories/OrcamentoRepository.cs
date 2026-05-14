using OficinaMecanica.API.Models;

namespace OficinaMecanica.API.Repositories;

public class OrcamentoRepository
{
    private static readonly List<Orcamento> _orcamentos = [];
    private static int _nextId = 0;

    public Orcamento Criar(Orcamento orcamento)
    {
        orcamento.Id = _nextId++;

        int itemId = 0;
        foreach (var item in orcamento.Itens)
        {
            item.Id = itemId++;
            item.OrcamentoId = orcamento.Id;
        }

        _orcamentos.Add(orcamento);
        return orcamento;
    }

    public Orcamento? BuscarPorId(int id) =>
        _orcamentos.FirstOrDefault(o => o.Id == id);

    public List<Orcamento> ListarTodos() =>
        _orcamentos.ToList();
}
