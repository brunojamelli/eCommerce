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
  - Testes para a camada de **Controle**: Simulação de serviços de estoque e pagamento, e validação dos endpoints.
  - Testes unitários para as regras de Cálculo de Frete, Cálculo de desconto do frete e Cálculo de desconto da compra.
- `/External` - Contém as subpastas/namespaces Fake e Stub e as interfaces IEstoqueExternal e IPagamento External.
- `/Repository` - Contém as classes e interfaces de repositório.
- `/Services` - Contém as classes e interfaces de serviços do projeto.

## Tecnologias Utilizadas
- **C#** e **.NET 7** como linguagem e framework base.
- **xUnit** para os testes automatizados.
- **Moq** para a criação de mocks.
- **FakeItEasy** para a criação de fakes nos testes.
## Como os testes foram implementados ?
  No diretório `/EcommerceTests` estão implementados os códigos de testes unitários, da camada de serviço e da camada de controle (usando ``test double`` e ``mock objects``).
  
  Os testes unitários que verificam o calculo do frete do carrinho estão na classe ``CalculaFreteCarrinhoTest`` e os que cobrem os métodos de calculo de desconto de frete 
  e desconto do total da compra estão na classe ``CalcularDescontoItemsCarrinhoTest``.
  
  Já os testes que cuidam da camada de controller estão presentes na classe ``CompraControllerTests`` e os que testam a camada de serviço estão presentes em ``CompraServiceTests`` e ``CompraServiceUnitTest`` e ``PagamentoServiceTests`` (que cuida especificamente dos testes da simulação do serviço de pagamentos).

Em relação à implementação do *test double* do tipo *fake* para simular o serviço de estoque e ao uso de *mock objects* para simular o comportamento do serviço de pagamentos, o diretório `External` contém duas subpastas: `Fake` e `Stub`.

- Na pasta `Fake`, estão implementadas as lógicas de simulação do estoque e do serviço externo de pagamento, nas classes `EstoqueSimulado` e `PagamentoSimulado`.
- Já na pasta `Stub`, estão as classes `EstoqueExternalStubSucesso` e `PagamentoSucessoServiceStub`, que implementam o comportamento dos *stubs* para simular o estoque e o serviço de pagamento.

Essas implementações fornecem uma base para testar o sistema sem a necessidade de conectar-se a serviços reais, permitindo simular diferentes cenários de disponibilidade de estoque e pagamento.


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
### Executar apenas uma classe específica
```
dotnet test --filter "FullyQualifiedName~YourNamespace.YourTestClassName"
```
#### Exemplo
```
dotnet test --filter "FullyQualifiedName~CalculaFreteProdutoTest"
```

```
dotnet test --filter "FullyQualifiedName~CalculaFreteCarrinhoTest"
```
#### Execução detalhada de uma classe específica
```
dotnet test --logger "console;verbosity=detailed" --filter "FullyQualifiedName~CalculaFreteProdutoTest"
```

## Licença
Este projeto é distribuído sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.


##  Executar os testes de caixa branca

**1-** No arquivo .csproj do seu projeto de teste, adicione a seguinte referência ao Coverlet:
```
<ItemGroup>
  <PackageReference Include="coverlet.collector" Version="3.1.2" />
</ItemGroup>
```
**2-** Execute os testes
```
dotnet test --collect:"XPlat Code Coverage"
```
**3-** Instale a ferramenta abaixo pra gerar o relatório visual:
```
dotnet tool install --global dotnet-reportgenerator-globaltool
```
4- Gerando o relarório:
```
reportgenerator -reports:./TestResults/**/coverage.cobertura.xml -targetdir:./coverage-report -reporttypes:Html
```
5- O resultado pode ser visualizado no diretório `/coverage-report`
