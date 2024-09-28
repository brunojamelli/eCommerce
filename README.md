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

- `/src` - Contém os arquivos de código-fonte do projeto.
  - `/Controllers` - Contém todos os arquivos de Controllers do projeto.
   - **CompraController**: Controlador responsável por gerenciar a finalização das compras.
  - `/Controllers` - Contém todos os arquivos de Services do projeto.
   - **CompraService**: Serviço que contém a lógica de negócio, como cálculo do preço total da compra e interação com serviços externos.
- `/Ecommerce.tests` - Contém os arquivos de teste automatizados.
  - Testes para a camada de **Serviço**: Validação do cálculo do preço total.
  - Testes para a camada de **Controller**: Simulação de serviços de estoque e pagamento, e validação dos endpoints.

## Tecnologias Utilizadas
- **C#** e **.NET 6** como linguagem e framework base.
- **xUnit** para os testes automatizados.
- **Moq** para a criação de mocks.
- **FakeItEasy** para a criação de fakes nos testes.
  
## Contribuições
Este projeto foi desenvolvido em equipe, com cada integrante sendo responsável por diferentes partes do código e dos testes.

## Como executar o projeto
``
dotnet watch run
``
- Com esse comando o projeto ira executar a aplicação e abrir uma aba do swagger em seu navegador

## Como executar os testes

``
dotnet test
``

``
dotnet test --logger "console;verbosity=detailed"
``


## Autores
- Nome 1 - Responsável pela camada de serviço.
- Nome 2 - Responsável pela camada de controller.
- Nome 3 - Implementação dos testes de caixa preta e branca.

## Licença
Este projeto é distribuído sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.
