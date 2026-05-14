using OficinaMecanica.API.DTOs;

namespace OficinaMecanica.API.Validators;

public class OrcamentoValidator
{
    public List<string> Validar(CriarOrcamentoRequest request)
    {
        var erros = new List<string>();

        if (request.ClienteId <= 0)
            erros.Add("clienteId é obrigatório e deve ser maior que zero.");

        if (request.VeiculoId <= 0)
            erros.Add("veiculoId é obrigatório e deve ser maior que zero.");

        if (request.Itens == null || request.Itens.Count == 0)
        {
            erros.Add("O orçamento deve conter pelo menos 1 item.");
            return erros;
        }

        for (int i = 0; i < request.Itens.Count; i++)
        {
            var item = request.Itens[i];
            var prefixo = $"Item {i + 1}";

            if (string.IsNullOrWhiteSpace(item.Descricao))
                erros.Add($"{prefixo}: descrição é obrigatória.");

            if (item.Quantidade <= 0)
                erros.Add($"{prefixo}: quantidade deve ser maior que zero.");

            if (item.ValorUnitario <= 0)
                erros.Add($"{prefixo}: valorUnitario deve ser maior que zero.");
        }

        return erros;
    }
}
