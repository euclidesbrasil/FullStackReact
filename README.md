# API Geral - Gestão de Pedidos.

## 📌 Descrição

Esta API fornece recursos para gerenciar autenticação, pedidos e as leituras via banco não relacional.

## 🛠 Tecnologias Utilizadas

- .NET 8
- SqlServer Express
- MongoDB
- RabbitMQ

## 🗉 Pré-requisitos

Clone este projeto usando a URL: [https://github.com/euclidesbrasil/FullStackReact.git](https://github.com/euclidesbrasil/FullStackReact.git)

Antes de baixar o projeto, certifique-se de ter instalado:

- **Visual Studio** (Versão utilizada: Microsoft Visual Studio Community 2022 - Versão 17.10.1, preferencialmente após a versão 17.8)
- **SqlServer Express 2022 ** (Versão utilizada: 17.2-3) [Baixar aqui](https://www.microsoft.com/pt-br/download/details.aspx?id=104781)
- **MongoDB Community** (Versão utilizada: 7.0.16) [Baixar aqui](https://www.mongodb.com/try/download/community-edition/releases)
- **RabbitMQ** (Versão utilizada: 4.0.5) [Baixar aqui](https://www.rabbitmq.com/docs/install-windows)
- **Visual Code** (Versão utilizada: 1.97.2)
## 🚀 Configuração Antes da Execução

### 1. Configuração do SQL SERVER

No projeto **ArquiteturaDesafio.General.Api**, abra o arquivo `appsettings.json` e ajuste a seção `DefaultConnection` com as credenciais do seu banco de dados local:

```json
"DefaultConnection": "Data Source=DESKTOP-784BBJ9\\SQLEXPRESS;Initial Catalog=ARQ_FULLSTACK;User ID=sa;Password=admin;MultipleActiveResultSets=True;TrustServerCertificate=True"
```

### 2. Configuração do MongoDB

No mesmo arquivo `appsettings.json`, há uma seção `ConnectionString` que define a conexão com o MongoDB. Ajuste conforme necessário para o seu ambiente.

### 3. Executando o Projeto API

Basta executar o projeto para iniciar a API. Na primeira execução, o banco de dados será criado automaticamente e os dados iniciais serão carregados. Poderá ser usado via Swagger;

Caso queira rodar via docker, abra o "PowerShell do Desenvolvedor" referente a raiz da solução e execute o comando:
```json
docker-compose up --build -d
```
Isso fará que o docker build a aplicação e suba as imagens necessárias.

Isso irá subir a api, que estará acessivel no link: http://localhost:5000/swagger/index.html

ATENÇÃO! 
Em ambos os casos, há um Worker responsável por ler as mensagens enviadas via RabbitMQ para poder 
gerar a versão do consulta via MongoDB;

Localmente, você deve executar o exe manualmente, pelo visual studio (Depurar nova insância sem inicializar) ou 
navegar até a pasta do proejto, apos efetuar o Rebuild da aplicação e executar o ArquiteturaDesafio.Worker.exe: 
src\ArquiteturaDesafio.Worker\bin\Debug\net8.0 ou em src\ArquiteturaDesafio.Worker\bin\Release\net8.0

Já no Docker, caso o serviço não seja iniciado automaticamente, inicar o mesmo, mas está configurado para iniciar sozinho.
Apos o imagem SqlServer subir e estiver funcional, pode ser que tenha que habilitar  as Transações Distribuidas (TALVEZ...); Para isso, acesse localmente o servidor "localhost, 1433" com o usuario "sa" e senha "Admin@123" (sugetsão: via SqlManagement);
Clique com o botão direito no servidor e seleciona a opções Propriedades->Conexões-> e marque a opção "Requer transações distribuídas para a comunicação servidor a servidor". Salve.
Se for necessário realizar o passo acima, mando restartar a api, via docker, pois o migration irá criar o banco para você.

### 3.1 Executando o Projeto REACT
Para rodar a aplicação (que está configuarada para consumir a api do docker), 
Navegue até a pasta do project react ( src\React\front-desafio ) e abra via VS Code;
No terminal execute o comando "npm start" e a aplicação será inciada.
Caso rode localmente via Visual Studio a API, ajuste os arquivos axiosConfig.js e authService.js, para ajustar a url de destino da API.


## 🔐 Autenticação

Para utilizar os endpoints, é necessário obter um token de autenticação. Utilize as credenciais iniciais:

- **Usuário:** admin
- **Senha:** s3nh@


## 📌 Endpoints

- [Visão Geral](#visão-geral)
- [Autenticação](#autenticação)
- [Endpoints de Clientes](#endpoints-de-clientes)
- [Endpoints de Pedidos](#endpoints-de-pedidos)
- [Endpoints de Produtos](#endpoints-de-produtos)
- [Endpoints de Usuários](#endpoints-de-usuários)
- [Esquemas e Componentes](#esquemas-e-componentes)
- [Segurança](#segurança)
- [Exemplos de Uso](#exemplos-de-uso)
- [Observações Finais](#observações-finais)

---

## Visão Geral

A API foi projetada para o controle de pedidos e integra duas fontes de dados:
- **SQL Server:** para o controle de pedidos e clientes.
- **MongoDB:** para armazenar informações complementares.

O projeto utiliza autenticação via JWT (Bearer) para proteger os endpoints.

---

## Autenticação

### POST `/auth/login`

**Descrição:**  
Realiza o login de um usuário.

**Request Body:**  
Formato `application/json` com o esquema `AuthenticateUserRequest`:
- `username` (string)
- `password` (string)

**Respostas:**
- **200 Success:** Login realizado com sucesso.
- **400 Bad Request:** Dados inválidos (retorna um objeto `ProblemDetails`).
- **500 Server Error:** Erro no servidor.

---

## Endpoints de Clientes

### POST `/Customers`

**Descrição:**  
Cria um novo cliente.

**Request Body:**  
- Schema: `CreateCustomerRequest`  
  Campos incluem: `id` (UUID), `name`, `identification` (usando o esquema `InfoContactDTO`) e datas de criação/atualização.

**Resposta:**
- **200 Success:** Retorna um objeto `CreateCustomerResponse` contendo o `id` do novo cliente.

---

### PUT `/Customers`

**Descrição:**  
Atualiza os dados de um cliente existente.

**Parâmetros:**
- Query Parameter: `id` (string, UUID)

**Request Body:**  
- Schema: `UpdateCustomerRequest`

**Resposta:**
- **200 Success:** Atualização realizada com sucesso.

---

### DELETE `/Customers`

**Descrição:**  
Exclui um cliente.

**Parâmetros:**
- Query Parameter: `id` (string, UUID)

**Resposta:**
- **200 Success:** Exclusão realizada com sucesso.

---

### GET `/Customers`

**Descrição:**  
Lista os clientes com suporte a paginação, filtros e ordenação.

**Parâmetros de Query:**
- `_page`: Número da página (integer, padrão: 1)
- `_size`: Quantidade de itens por página (integer, padrão: 10)
- `filters`: Objeto com filtros (chave/valor)
- `_order`: Ordem de classificação (string, padrão: "id asc")

**Resposta:**
- **200 Success:** Retorna um array de `GetCustomersQueryResponse`.

---

### GET `/Customers/{id}`

**Descrição:**  
Retorna os detalhes de um cliente específico.

**Parâmetros de Path:**
- `id`: Identificador do cliente (string, UUID)

**Resposta:**
- **200 Success:** Dados do cliente.

---

## Endpoints de Pedidos

### POST `/Orders`

**Descrição:**  
Cria um novo pedido.

**Request Body:**  
- Schema: `CreateOrderRequest`  
  Campos incluem: `orderDate`, `customerId` (UUID) e uma lista de itens (array de `OrderItemBaseDTO`).

**Respostas:**
- **201 Created:** Pedido criado, retorna `CreateOrderResponse` com o `id` do pedido.
- **400 Bad Request / 401 Unauthorized / 500 Server Error:** Em caso de erro.

---

### PUT `/Orders`

**Descrição:**  
Atualiza um pedido existente.

**Parâmetros:**
- Query Parameter: `id` (string, UUID)

**Request Body:**  
- Schema: `UpdateOrderRequest`

**Respostas:**
- **204 No Content:** Atualização realizada com sucesso.
- **400/401/500:** Em caso de erro.

---

### DELETE `/Orders`

**Descrição:**  
Exclui um pedido.

**Parâmetros:**
- Query Parameter: `id` (string, UUID)

**Respostas:**
- **204 No Content:** Exclusão realizada com sucesso.
- **400/401/500:** Em caso de erro.

---

### GET `/Orders`

**Descrição:**  
Lista os pedidos com suporte a paginação, filtros e ordenação.

**Parâmetros de Query:**
- `_page`: Número da página (integer, padrão: 1)
- `_size`: Quantidade de itens por página (integer, padrão: 10)
- `filters`: Objeto com filtros (chave/valor)
- `_order`: Ordem de classificação (string, padrão: "id asc")

**Respostas:**
- **200 Success:** Retorna um array de `GetOrdersQueryResponse`.
- **400/401/500:** Em caso de erro.

---

### GET `/Orders/{id}`

**Descrição:**  
Retorna os detalhes de um pedido específico.

**Parâmetros de Path:**
- `id`: Identificador do pedido (string, UUID)

**Respostas:**
- **200 Success:** Dados do pedido, conforme o esquema `GetOrderByIdResponse`.
- **400/401/500:** Em caso de erro.

---

## Endpoints de Produtos

### POST `/Products`

**Descrição:**  
Cria um novo produto.

**Request Body:**  
- Schema: `CreateProductRequest`  
  Campos incluem: `name` (string) e `price` (double).

**Respostas:**
- **201 Created:** Produto criado, retorna `CreateProductResponse` com o `id` do produto.
- **400/401/500:** Em caso de erro.

---

### PUT `/Products`

**Descrição:**  
Atualiza um produto existente.

**Parâmetros:**
- Query Parameter: `id` (string, UUID)

**Request Body:**  
- Schema: `UpdateProductRequest`

**Respostas:**
- **204 No Content:** Atualização realizada com sucesso.
- **400/401/500:** Em caso de erro.

---

### DELETE `/Products`

**Descrição:**  
Exclui um produto.

**Parâmetros:**
- Query Parameter: `id` (string, UUID)

**Respostas:**
- **204 No Content:** Exclusão realizada com sucesso.
- **400/401/500:** Em caso de erro.

---

### GET `/Products`

**Descrição:**  
Lista os produtos com suporte a paginação, filtros e ordenação.

**Parâmetros de Query:**
- `_page`: Número da página (integer, padrão: 1)
- `_size`: Quantidade de itens por página (integer, padrão: 10)
- `filters`: Objeto com filtros (chave/valor)
- `_order`: Ordem de classificação (string, padrão: "id asc")

**Respostas:**
- **200 Success:** Retorna um array de `GetProductsQueryResponse`.
- **400/401/500:** Em caso de erro.

---

### GET `/Products/{id}`

**Descrição:**  
Retorna os detalhes de um produto específico.

**Parâmetros de Path:**
- `id`: Identificador do produto (string, UUID)

**Respostas:**
- **200 Success:** Dados do produto.
- **400/401/500:** Em caso de erro.

---

## Endpoints de Usuários

### POST `/Users`

**Descrição:**  
Cria um novo usuário.

**Request Body:**  
- Schema: `CreateUserRequest`  
  Campos incluem: `email`, `username`, `firstname`, `lastname`, `address` (com o esquema `AddressDto`), `phone`, `status`, `role`, `password` e `id` (UUID).

**Respostas:**
- **201 Created:** Usuário criado, retorna `CreateUserResponse` com o `id` do usuário.
- **400/401/500:** Em caso de erro.

---

### PUT `/Users`

**Descrição:**  
Atualiza os dados de um usuário.

**Parâmetros:**
- Query Parameter: `id` (string, UUID)

**Request Body:**  
- Schema: `UpdateUserRequest`

**Respostas:**
- **204 No Content:** Atualização realizada com sucesso.
- **400/401/500:** Em caso de erro.

---

### DELETE `/Users`

**Descrição:**  
Exclui um usuário.

**Parâmetros:**
- Query Parameter: `id` (string, UUID)

**Respostas:**
- **204 No Content:** Exclusão realizada com sucesso.
- **400/401/500:** Em caso de erro.

---

### GET `/Users`

**Descrição:**  
Lista os usuários com suporte a paginação, filtros e ordenação.

**Parâmetros de Query:**
- `_page`: Número da página (integer, padrão: 1)
- `_size`: Quantidade de itens por página (integer, padrão: 10)
- `filters`: Objeto com filtros (chave/valor)
- `_order`: Ordem de classificação (string, padrão: "id asc")

**Respostas:**
- **200 Success:** Retorna um array de `GetUsersQueryResponse`.
- **400/500:** Em caso de erro.

---

### GET `/Users/{id}`

**Descrição:**  
Retorna os detalhes de um usuário específico.

**Parâmetros de Path:**
- `id`: Identificador do usuário (string, UUID)

**Respostas:**
- **200 Success:** Dados do usuário, conforme o esquema `GetUsersByIdResponse`.
- **400/401/500:** Em caso de erro.

---

## Esquemas e Componentes

A API utiliza diversos esquemas para definir a estrutura dos dados. Entre os principais estão:

- **AuthenticateUserRequest:**  
  - `username` (string)  
  - `password` (string)

- **CreateCustomerRequest / CreateCustomerResponse / UpdateCustomerRequest:**  
  Estruturas para criação e atualização de clientes, incluindo identificação, nome e datas.

- **CreateOrderRequest / CreateOrderResponse / UpdateOrderRequest:**  
  Estruturas para gerenciamento de pedidos. O pedido inclui a data, o ID do cliente e uma lista de itens (`OrderItemBaseDTO` ou `OrderItemRead`).

- **CreateProductRequest / CreateProductResponse / UpdateProductRequest:**  
  Estruturas para gerenciamento de produtos, com campos como nome e preço.

- **CreateUserRequest / CreateUserResponse / UpdateUserRequest:**  
  Estruturas para gerenciamento de usuários, que incluem dados pessoais e informações de endereço (via `AddressDto`).

- **Outros DTOs:**  
  `CustomerDTO`, `CustomerOrderDTO`, `GeolocationDto`, `InfoContactDTO`, `OrderItemBaseDTO`, `OrderItemRead`, `ProductDTO`, `SaleItemDTO`, `SaleWithDetaislsDTO`, `UserDTO` e enums como `UserRole`, `UserStatus` e `OrderStatus`.

---

## Segurança

A API utiliza autenticação **Bearer** com JWT. Para acessar os endpoints protegidos, envie o token no header:

