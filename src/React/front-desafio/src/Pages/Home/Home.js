import React from "react";
import { Container, Typography } from "@mui/material";

export default function Home() {
  return (
    <Container>
      <Typography variant="h4" gutterBottom>
        Avaliação Técnica (C# + Clean Architecture + CQRS + DDD + SQL + MongoDB)
      </Typography>
      <Typography variant="body1" paragraph>
        💡 Subir código no Github e nos enviar o link.
      </Typography>
      <Typography variant="body1" paragraph>
        <strong>O Desafio:</strong> Desenvolver uma aplicação em C#, na plataforma .NET Core 5 ou superior, obedecendo os seguintes requisitos:
      </Typography>
      <Typography variant="body1" paragraph>
        <strong>Instruções:</strong>
        <ol>
          <li>Desenvolver back-end utilizando Web Api, seguindo o padrão RESTful.</li>
          <li>Construir o front-end com Single Page Application. (VueJs, React ou Angular)</li>
          <li>Não é necessário se preocupar com o layout nem com boas práticas visuais, html, css, etc.</li>
        </ol>
      </Typography>
      <Typography variant="body1" paragraph>
        <strong>Contexto do Projeto:</strong> Você vai construir uma API RESTful que gerencia um sistema de Pedidos de uma loja online. Os pedidos podem ser realizados pelos clientes, e o sistema deve armazenar tanto os dados principais dos pedidos quanto dados complementares relacionados a itens do pedido e status.
      </Typography>
      <Typography variant="body1" paragraph>
        A API terá que interagir com um banco de dados relacional (SQL Server) para armazenar os dados do pedido, e com um banco de dados NoSQL (MongoDB) para armazenar os dados para leitura.
      </Typography>
      <Typography variant="body1" paragraph>
        A aplicação deve ser desenvolvida com Clean Architecture, utilizando CQRS (Command Query Responsibility Segregation) para separar os fluxos de leitura e escrita e deve atender ao DDD. Você também utilizará um ORM (como o Entity Framework) para o acesso ao banco relacional.
      </Typography>
      <Typography variant="body1" paragraph>
        <strong>Observações:</strong>
        <ul>
          <li>Customer pode ser sempre fixo, consideremos que todos os pedidos serão feitos apenas por um cliente;</li>
          <li>A tabela de produto deve conter apenas três opções.</li>
        </ul>
      </Typography>
      <Typography variant="body1" paragraph>
        <strong>Especificação de ações:</strong>
        <ul>
          <li>Adicionar;</li>
          <li>Remover;</li>
          <li>Atualizar;</li>
          <li>Listar;</li>
          <li>Detalhar pedidos.</li>
        </ul>
      </Typography>
      <Typography variant="body1" paragraph>
        <strong>Entidades Relacionais:</strong>
        <ul>
          <li><strong>Customer</strong>
            <ul>
              <li>Id (PK)</li>
              <li>Name</li>
              <li>Email</li>
              <li>Phone</li>
            </ul>
          </li>
          <li><strong>Order</strong>
            <ul>
              <li>Id (PK)</li>
              <li>CustomerId (FK para Customer)</li>
              <li>OrderDate</li>
              <li>TotalAmount</li>
              <li>Status (Enum - mapeado para OrderStatus)</li>
            </ul>
          </li>
          <li><strong>OrderItem</strong>
            <ul>
              <li>Id (PK)</li>
              <li>OrderId (FK para Order)</li>
              <li>ProductId</li>
              <li>ProductName</li>
              <li>Quantity</li>
              <li>UnitPrice</li>
              <li>TotalPrice</li>
            </ul>
          </li>
          <li><strong>Produto (Product)</strong>
            <ul>
              <li>Id (PK)</li>
              <li>Name</li>
              <li>Price</li>
            </ul>
          </li>
        </ul>
      </Typography>
    </Container>
  );
}