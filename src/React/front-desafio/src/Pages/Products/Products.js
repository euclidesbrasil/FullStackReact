import React, { useState, useEffect } from "react";
import { Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, Typography } from "@mui/material";
import api, { setAuthToken } from "../../axiosConfig";

export default function ProdutosPage() {
  const [dados, setDados] = useState([]);

  useEffect(() => {
    async function fetchData() {
      try {
        const token = localStorage.getItem("token");
        setAuthToken(token);
        const response = await api.get("/Products");
        if (Array.isArray(response.data.data)) {
          setDados(response.data.data);
        } else {
          console.error("A resposta da API não é um array:", response.data);
        }
      } catch (error) {
        console.error("Erro ao buscar produtos:", error);
      }
    }

    fetchData();
  }, []);

  return (
    <div>
      <Typography variant="h4" gutterBottom>
        Produtos
      </Typography>
      {Array.isArray(dados) && dados.length > 0 ? (
        <TableContainer component={Paper}>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>ID</TableCell>
                <TableCell>Nome</TableCell>
                <TableCell>Preço</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {dados.map(produto => (
                <TableRow key={produto.id}>
                  <TableCell>{produto.id}</TableCell>
                  <TableCell>{produto.name}</TableCell>
                  <TableCell>{produto.price}</TableCell>
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