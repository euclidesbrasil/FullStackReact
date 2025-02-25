import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import { Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, Typography, Button } from "@mui/material";
import api, { setAuthToken } from "../../axiosConfig";

export default function OrdersList() {
  const [orders, setOrders] = useState([]);

  useEffect(() => {
    async function fetchOrders() {
      try {
        const token = localStorage.getItem("token");
        setAuthToken(token);
        const response = await api.get("/Orders");
        if (Array.isArray(response.data.data)) {
          setOrders(response.data.data);
        } else {
          console.error("A resposta da API não é um array:", response.data);
        }
      } catch (error) {
        console.error("Erro ao buscar pedidos:", error);
      }
    }

    fetchOrders();
  }, []);

  return (
    <div>
      <Typography variant="h4" gutterBottom>
        Pedidos
      </Typography>
      <Button variant="contained" color="primary" component={Link} to="/orders/create">
        Adicionar Pedido
      </Button>
      {Array.isArray(orders) && orders.length > 0 ? (
        <TableContainer component={Paper}>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>ID do Pedido</TableCell>
                <TableCell>Cliente</TableCell>
                <TableCell>Data do Pedido</TableCell>
                <TableCell>Valor Total</TableCell>
                <TableCell>Status</TableCell>
                <TableCell>Ações</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {orders.map(order => (
                <TableRow key={order.orderId}>
                  <TableCell>{order.orderId}</TableCell>
                  <TableCell>{order.customer.name}</TableCell>
                  <TableCell>{new Date(order.orderDate).toLocaleDateString()}</TableCell>
                  <TableCell>{order.totalAmount}</TableCell>
                  <TableCell>{order.status}</TableCell>
                  <TableCell>
                    <Button variant="contained" color="primary" component={Link} to={`/orders/edit/${order.orderId}`}>
                      Editar
                    </Button>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </TableContainer>
      ) : (
        "Nenhum pedido disponível"
      )}
    </div>
  );
}