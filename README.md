# Projeto
 
Leilao

## Endpoints

********************************
Produto
********************************

[POST ]/api/v1/produto
[GET ]/api/v1/produto
[GET ]/api/v1/produto/{id}

{
	Id,
	Nome,
	Valor
}

********************************
Lances
********************************

[POST]/api/v1/lance/
[GET] /api/v1/lance/
[GET] /api/v1/lance/{produtoId}

{
	Cpf,
	ProdutoId,
	Valor
}

(Unidade)
+ Teste de Controller;
+ Teste de Serviços;
+++ Com validacoes

(Integrado)
+ Teste de Repositorios;

***********************************
Algumas regras para lance
***********************************

++ Não permitir lance menor do que já existe
++ Nao permitir lance menor do que o lance inicial estipulado no produto
++ 