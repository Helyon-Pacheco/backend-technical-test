CREATE TABLE IF NOT EXISTS Orcamento (
    Id              SERIAL PRIMARY KEY,
    ClienteId       INT            NOT NULL,
    VeiculoId       INT            NOT NULL,
    Status          VARCHAR(50)    NOT NULL DEFAULT 'Aberto',
    ValorTotal      NUMERIC(18, 2) NOT NULL DEFAULT 0,
    DataCriacao     TIMESTAMP      NOT NULL DEFAULT NOW(),
    DataFinalizacao TIMESTAMP      NULL
);
 
CREATE TABLE IF NOT EXISTS OrcamentoItem (
    Id             SERIAL PRIMARY KEY,
    OrcamentoId    INT            NOT NULL REFERENCES Orcamento(Id),
    Descricao      VARCHAR(255)   NOT NULL,
    Quantidade     INT            NOT NULL,
    ValorUnitario  NUMERIC(18, 2) NOT NULL,
    ValorTotal     NUMERIC(18, 2) NOT NULL DEFAULT 0
);
 
 
CREATE OR REPLACE FUNCTION sp_finalizar_orcamento(p_orcamento_id INT)
RETURNS TABLE (codigo INT, mensagem TEXT)
LANGUAGE plpgsql
AS $$
DECLARE
    v_status     VARCHAR(50);
    v_total_itens INT;
    v_valor_total NUMERIC(18, 2);
BEGIN
 
    SELECT o.Status
    INTO   v_status
    FROM   Orcamento o
    WHERE  o.Id = p_orcamento_id
    FOR UPDATE;
 
    IF NOT FOUND THEN
        RETURN QUERY SELECT
            1,
            'Erro: Orcamento ' || p_orcamento_id || ' nao encontrado.'::TEXT;
        RETURN;
    END IF;
 
    IF v_status <> 'Aberto' THEN
        RETURN QUERY SELECT
            2,
            ('Erro: O orcamento nao esta com status Aberto. Status atual: ' || v_status || '.')::TEXT;
        RETURN;
    END IF;
 
    SELECT COUNT(*)
    INTO   v_total_itens
    FROM   OrcamentoItem
    WHERE  OrcamentoId = p_orcamento_id;
 
    IF v_total_itens = 0 THEN
        RETURN QUERY SELECT
            3,
            'Erro: O orcamento nao possui itens. Adicione ao menos 1 item antes de finalizar.'::TEXT;
        RETURN;
    END IF;
 
    UPDATE OrcamentoItem
    SET    ValorTotal = Quantidade * ValorUnitario
    WHERE  OrcamentoId = p_orcamento_id;
 
    SELECT SUM(ValorTotal)
    INTO   v_valor_total
    FROM   OrcamentoItem
    WHERE  OrcamentoId = p_orcamento_id;
 
    UPDATE Orcamento
    SET    Status          = 'Finalizado',
           ValorTotal      = v_valor_total,
           DataFinalizacao = NOW()
    WHERE  Id = p_orcamento_id;
 
    RETURN QUERY SELECT
        0,
        ('Sucesso: Orcamento ' || p_orcamento_id ||
         ' finalizado. Valor total: R$ ' || v_valor_total || '.')::TEXT;
 
EXCEPTION
    WHEN OTHERS THEN
        RETURN QUERY SELECT
            99,
            ('Erro inesperado: ' || SQLERRM)::TEXT;
END;
$$;
 
 
TRUNCATE TABLE OrcamentoItem RESTART IDENTITY CASCADE;
TRUNCATE TABLE Orcamento     RESTART IDENTITY CASCADE;
 
INSERT INTO Orcamento (Id, ClienteId, VeiculoId, Status, DataCriacao)
VALUES
    (1, 10, 5, 'Aberto',     NOW()),
    (2, 11, 6, 'Finalizado', NOW()),
    (3, 12, 7, 'Aberto',     NOW());
 
INSERT INTO OrcamentoItem (OrcamentoId, Descricao, Quantidade, ValorUnitario)
VALUES
    (1, 'Troca de oleo', 1,  80.00),
    (1, 'Filtro de ar',  2,  45.00),
    (1, 'Alinhamento',   1, 120.00);
 
-- ─────────────────────────────────────────
-- Caso 1: Sucesso
-- Esperado: codigo=0, ValorTotal=290.00
-- ─────────────────────────────────────────
SELECT * FROM sp_finalizar_orcamento(1);
 
-- ─────────────────────────────────────────
-- Caso 2: Orcamento nao encontrado
-- Esperado: codigo=1
-- ─────────────────────────────────────────
SELECT * FROM sp_finalizar_orcamento(999);
 
-- ─────────────────────────────────────────
-- Caso 3: Status invalido
-- Esperado: codigo=2
-- ─────────────────────────────────────────
SELECT * FROM sp_finalizar_orcamento(2);
 
-- ─────────────────────────────────────────
-- Caso 4: Sem itens
-- Esperado: codigo=3
-- ─────────────────────────────────────────
SELECT * FROM sp_finalizar_orcamento(3);
 
-- ─────────────────────────────────────────
-- Caso 5: Chamar o mesmo orcamento duas vezes
-- Esperado: codigo=2 (ja esta Finalizado)
-- ─────────────────────────────────────────
SELECT * FROM sp_finalizar_orcamento(1);
 
-- ─────────────────────────────────────────
-- Verifica o estado final das tabelas
-- ─────────────────────────────────────────
SELECT Id, Status, ValorTotal, DataFinalizacao FROM Orcamento;
SELECT Id, OrcamentoId, Descricao, Quantidade, ValorUnitario, ValorTotal FROM OrcamentoItem;