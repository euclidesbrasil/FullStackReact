import React, { useState, useEffect } from "react";
import { Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, Typography } from "@mui/material";
import api, { setAuthToken } from "../../axiosConfig";

export default function ClientesPage() {
  const [dados, setDados] = useState([]);

  useEffect(() => {
    async function fetchData() {
      try {
        const token = localStorage.getItem("token");
        setAuthToken(token);
        const response = await api.get("/Customers");
        if (Array.isArray(response.data.data)) {
          setDados(response.data.data);
        } else {
          console.error("A resposta da API não é um array:", response.data);
        }
      } catch (error) {
        console.error("Erro ao buscar:", error);
      }
    }

    fetchData();
  }, []);

  return (
    <div>
      <Typography variant="h4" gutterBottom>
        Clientes
      </Typography>
      {Array.isArray(dados) && dados.length > 0 ? (
        <TableContainer component={Paper}>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>ID</TableCell>
                <TableCell>Nome</TableCell>
                <TableCell>Email</TableCell>
                <TableCell>Telefone</TableCell>
                <TableCell>Data de Criação</TableCell>
                <TableCell>Data de Atualização</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {dados.map(cliente => (
                <TableRow key={cliente.id}>
                  <TableCell>{cliente.id}</TableCell>
                  <TableCell>{cliente.name}</TableCell>
                  <TableCell>{cliente.identification.email}</TableCell>
                  <TableCell>{cliente.identification.phone}</TableCell>
                  <TableCell>{cliente.dateCreated}</TableCell>
                  <TableCell>{cliente.dateUpdated || "N/A"}</TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </TableContainer>
      ) : (
        "Nenhum dado disponível"
      )}
    </div>
  );
}