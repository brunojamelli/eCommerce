# Projeto de Testes de Software: Carrinho de Compras

## Descrição
Este projeto tem como objetivo testar a lógica de um sistema de carrinho de compras, implementando testes automatizados para garantir o correto funcionamento das camadas de Serviço e Controller. O foco está em validar o cálculo do preço total da compra, simular o comportamento de serviços externos como o de estoque e pagamentos, e garantir uma alta cobertura de código.

## Objetivos do Trabalho

### 1. Testar a Camada de Serviço
- Implementar testes automatizados para o método de cálculo do **preço total da compra** na camada de serviço.
- Aplicar critérios de testes de:
  - **Caixa preta**: Particionamento em classes de equivalência e análise de valor limite.
  - **Caixa branca**: Atingir 100% de cobertura de arestas nos métodos testados.

### 2. Testar a Camada de Controller
- Implementar testes para a camada de **controller** da funcionalidade de finalizar compra, validando o comportamento correto do **endpoint HTTP**.
- Os testes devem incluir:
  - Um **fake** do serviço de estoque para simular seu comportamento.
  - Uso de **mock objects** para simular o comportamento do serviço de pagamentos.
  - Garantir 100% de **cobertura de arestas** nos métodos `finalizarCompra` das classes `CompraController` e `CompraService`.

## Estrutura do Projeto

- `/Controllers` - Contém todos os arquivos de Controllers do projeto.
  - **CompraController**: Controlador responsável por gerenciar a finalização das compras.
- `/Services` - Contém todos os arquivos de Services do projeto.
  - **CompraService**: Serviço que contém a lógica de negócio, como cálculo do preço total da compra e interação com serviços externos.
- `/Domain` - Contém as subpastas/namespaces DTO e Entity do projeto.
   - `/Entity` - Contém todos os arquivos de Entidades.
   - `/DTOs` - Contém todos os arquivos de DTOs.
- `/EcommerceTests` - Contém os arquivos de teste automatizados.
  - Testes para a camada de **Serviço**: Validação do cálculo do preço total.
  - Testes para a camada de **Controller**: Simulação de serviços de estoque e pagamento, e validação dos endpoints.
- `/External` - Contém a subpastas/namespace Fake e as interfaces IEstoqueExternal e IPagamento External.
- `/Repository` - Contém as classes e interfaces de repositório.
- `/Services` - Contém as classes e interfaces de serviços do projeto.

## Tecnologias Utilizadas
- **C#** e **.NET 7** como linguagem e framework base.
- **xUnit** para os testes automatizados.
- **Moq** para a criação de mocks.
- **FakeItEasy** para a criação de fakes nos testes.
  

## Como executar o projeto
``
dotnet watch run
``
- Com esse comando o projeto ira executar a aplicação e abrir uma aba do swagger em seu navegador

## Como executar os testes

### Executar os testes do projeto
``
dotnet test
``
### Executar os testes do projeto de modo mais detalhado
``
dotnet test --logger "console;verbosity=detailed"
``
### Executar apenas uma classe especifica
```
dotnet test --filter "FullyQualifiedName~YourNamespace.YourTestClassName"
```
#### Exemplo
```
dotnet test --filter "FullyQualifiedName~CalculaFreteProdutoTest"
```

## Licença
Este projeto é distribuído sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.
