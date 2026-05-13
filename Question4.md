## Teste 4: Diferenças entre View, Table-valued Function, Stored Procedure, Trigger

### View
Uma view é uma tabela virtual que possui linhas e colunas vindo de outras tabelas do banco de dados através de uma Query.
Os dados destas linhas e colunas são gerados dinamicamente todas as vezes que é feita uma referencia a View.
É ideal se precisa reutilizar uma consulta complexa sem parametros.

#### Exemplo
```sql
CREATE VIEW vw_PedidosAtivos AS
SELECT 
    p.Id,
    p.Data,
    c.Nome AS Cliente,
    p.Total
FROM Pedidos p
INNER JOIN Clientes c ON c.Id = p.ClienteId
WHERE p.Status = 'Ativo';

-- Uso: exatamente como uma tabela
SELECT * FROM vw_PedidosAtivos WHERE Total > 1000;
```

### Table-valued Function
Similar a View, é uma função que retorna uma tabela de dados.
Diferente das Views, uma Table-valued Function pode receber parametros, passando valores de forma dinamica, permitindo mudar como a query funciona.
Isso a torna ideal para reutilizar uma consulta complexa que necessite de parametros para manipular os dados.

#### Exemplo
```sql
CREATE FUNCTION fn_PedidosPorCliente(@ClienteId INT)
RETURNS TABLE
AS
RETURN
(
    SELECT 
        p.Id,
        p.Data,
        p.Total
    FROM Pedidos p
    WHERE p.ClienteId = @ClienteId
    AND   p.Status = 'Ativo'
);

-- Uso: como uma tabela parametrizada
SELECT * FROM fn_PedidosPorCliente(42) WHERE Total > 500;

-- Pode fazer JOIN com outras tabelas
SELECT c.Nome, f.Total
FROM Clientes c
CROSS APPLY fn_PedidosPorCliente(c.Id) f;
```

### Stored Procedure
É um conjunto de comandos SQL armazenado para que possam ser executados todos de uma só vez. 
Isso a torna ideal para fazer uma chamada explicita em uma operação, 
permitindo ganhar tempo com tarefas repetitivas, executar tarefas agendadas e melhorar o desempenho do banco de dados, etc.

#### Exemplo
```sql
CREATE PROCEDURE sp_FinalizarPedido
    @PedidoId  INT,
    @UsuarioId INT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

            -- Valida se o pedido existe e está aberto
            IF NOT EXISTS (
                SELECT 1 FROM Pedidos 
                WHERE Id = @PedidoId AND Status = 'Ativo'
            )
                THROW 50001, 'Pedido não encontrado ou já finalizado.', 1;

            -- Atualiza o pedido
            UPDATE Pedidos
            SET Status            = 'Finalizado',
                DataFechamento    = GETDATE(),
                UsuarioFechamento = @UsuarioId
            WHERE Id = @PedidoId;

            -- Registra no histórico
            INSERT INTO HistoricoPedidos (PedidoId, Acao, Data)
            VALUES (@PedidoId, 'Finalizado', GETDATE());

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;

-- Uso
EXEC sp_FinalizarPedido @PedidoId = 101, @UsuarioId = 7;
```

### Trigger
Triggers são um tipo de procedimento armazenado que é executado automaticamente pelo banco de dados quando é feito mudanças em uma tabela.
É ideal para executar operações de lógica de negócio automaticamente sempre que tem uma modificação nos dados.

#### Exemplo
```sql
CREATE TRIGGER trg_LogAlteracaoPreco
ON Produtos
AFTER UPDATE
AS
BEGIN
    -- Verifica se o campo Preco foi alterado
    IF UPDATE(Preco)
    BEGIN
        INSERT INTO LogAlteracoes (Tabela, Descricao, DataAlteracao)
        SELECT 
            'Produtos',
            'Preço alterado de R$' + CAST(d.Preco AS VARCHAR) +
            ' para R$' + CAST(i.Preco AS VARCHAR) +
            ' no produto: ' + i.Nome,
            GETDATE()
        FROM inserted i                       -- linha com os novos valores
        INNER JOIN deleted d ON d.Id = i.Id   -- linha com os valores anteriores
        WHERE i.Preco <> d.Preco;
    END
END;

-- Uso: automático ao executar qualquer UPDATE na tabela Produtos
UPDATE Produtos SET Preco = 59.90 WHERE Id = 5;
-- ↑ o trigger dispara sozinho, sem chamada explícita
```
