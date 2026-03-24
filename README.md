# 🏴‍☠️ Rei dos Piratas - Painel Administrativo (Sprint 3)

## Sobre esta Aplicação

Este repositório contém a evolução do **MVP (Produto Mínimo Viável)** de um painel administrativo para o e-commerce de mangás "Rei dos Piratas". A aplicação foi desenvolvida em **ASP.NET Core MVC** como parte da "Challenge Sprint" da faculdade pelo grupo CATECH.

Na **Sprint 3**, a aplicação evoluiu de um simples CRUD para uma arquitetura observável e testável, com a implementação de **Testes Automatizados (Padrão AAA)**, **Logging Estruturado**, **Tracing/Métricas** e endpoints de **Health Check**, garantindo maior confiabilidade e nível de produção para a plataforma.

## ✨ Funcionalidades e Arquitetura

## 📊 Monitoramento e Observabilidade

-   **Health Checks (`/health`):** Endpoint de monitoramento de saúde da API implementado com `Microsoft.Extensions.Diagnostics.HealthChecks`. Ele valida em tempo real o status da aplicação e a disponibilidade da conexão com o banco de dados Oracle.

-   **Logging Estruturado:** Integração com **Serilog** substituindo o logger padrão do .NET. Os logs são gravados tanto no console quanto em arquivo local (`/logs`), incluindo ID de correlação para rastrear o ciclo de vida completo de cada requisição.

-   **Tracing e Métricas:** Configuração do **OpenTelemetry** interceptando instrumentações do ASP.NET Core e HttpClient, permitindo análises futuras de performance e gargalos.


## 🧪 Testes Automatizados (Padrão AAA)

-   **Testes Unitários:** Utilização do **xUnit** com **Moq** e `Moq.EntityFrameworkCore` para simular o banco de dados, isolando a camada de Controller e garantindo que as regras funcionem sem dependência de infraestrutura externa.

-   **Testes de Integração:** Uso do `WebApplicationFactory` para levantar a aplicação em memória. Foi implementado um banco de dados `InMemory` nativo do EF Core isolado em um contêiner de injeção de dependências próprio, garantindo que as requisições HTTP de teste não interfiram com a conexão Oracle de produção.


## 📦 Gerenciamento de Produtos (CRUD) e Banco Real

-   Interface administrativa completa em Bootstrap 5 para Create, Read, Update e Delete de mangás.

-   Persistência de dados conectada a um banco **Oracle** utilizando **Entity Framework Core 8**.

-   Tratamento global de globalização (pt-BR) e captura de `DbUpdateException` e `OracleException` para garantir a integridade referencial ao usuário final.


## 🛠️ Tecnologias Utilizadas

-   **Backend:** ASP.NET Core 8 MVC, C# 11

-   **Testes e Mocking:** xUnit, Moq, Microsoft.AspNetCore.Mvc.Testing

-   **Observabilidade:** Serilog, OpenTelemetry, ASP.NET Core HealthChecks

-   **ORM & Banco de Dados:** Entity Framework Core 8, Oracle Database, InMemory Database (Testes)

-   **Frontend:** HTML5, CSS3, JavaScript, Bootstrap 5, jQuery


## 🚀 Como Executar a Aplicação

A aplicação requer uma conexão com um banco de dados Oracle para o ambiente de execução (embora os testes rodem em memória).

1.  **Clone o Repositório:**

    Bash

    ```
    git clone https://github.com/Dejota-04/Sprint1.git

    ```

2.  **Configure a String de Conexão:**

    -   No arquivo `appsettings.json`, atualize o valor de `OracleConnection` com os dados de acesso (Data Source, User Id, Password) do seu banco.

3.  **Rode as Migrations (Se Necessário):**

    -   No Console do Gerenciador de Pacotes do Visual Studio, rode: `Update-Database`

4.  **Execute o Projeto:**

    -   Rode a aplicação via Visual Studio (F5) ou via CLI com `dotnet run`.


## 📈 Como Monitorar e Testar

## Executando os Testes Automatizados

Os testes foram construídos para rodar independentemente da disponibilidade do banco de dados Oracle. Para rodar a suíte completa de testes de Integração e Unidade, abra o terminal na raiz do projeto e execute:

Bash

```
dotnet test

```

## Verificando a Saúde da Aplicação (Health Check)

Com a aplicação em execução, acesse o endpoint de Health Check pelo navegador ou via Postman/cURL:

-   **URL:** `http://localhost:<porta>/health`

-   **Retorno Esperado:** Um JSON detalhando o status da API e do Banco Oracle (Ex: `{"status":"Healthy","checks":[{"componente":"Banco_Oracle","status":"Healthy"}]}`).


## 👨‍💻 Integrantes do Grupo CATECH

-   **Daniel Santana Corrêa Batista** [RM559622]

-   **Wendell Nascimento Dourado** [RA559336]

-   **Jonas de Jesus Campos de Oliveira** [RM561144]