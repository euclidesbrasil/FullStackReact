import React, { useState, useEffect } from "react";
import { Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, Typography } from "@mui/material";
import api, { setAuthToken } from "../../axiosConfig";

export default function UsuariosPage() {
  const [dados, setDados] = useState([]);

  useEffect(() => {
    async function fetchData() {
      try {
        const token = localStorage.getItem("token");
        setAuthToken(token);
        const response = await api.get("/Users");
        if (Array.isArray(response.data.data)) {
          setDados(response.data.data);
        } else {
          console.error("A resposta da API não é um array:", response.data);
        }
      } catch (error) {
        console.error("Erro ao buscar usuários:", error);
      }
    }

    fetchData();
  }, []);

  return (
    <div>
      <Typography variant="h4" gutterBottom>
        Usuários
      </Typography>
      {Array.isArray(dados) && dados.length > 0 ? (
        <TableContainer component={Paper}>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>ID</TableCell>
                <TableCell>Username</TableCell>
                <TableCell>First Name</TableCell>
                <TableCell>Last Name</TableCell>
                <TableCell>Email</TableCell>
                <TableCell>Phone</TableCell>
                <TableCell>City</TableCell>
                <TableCell>Street</TableCell>
                <TableCell>Number</TableCell>
                <TableCell>Zipcode</TableCell>
                <TableCell>Latitude</TableCell>
                <TableCell>Longitude</TableCell>
                <TableCell>Status</TableCell>
                <TableCell>Role</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {dados.map(usuario => (
                <TableRow key={usuario.id}>
                  <TableCell>{usuario.id}</TableCell>
                  <TableCell>{usuario.username}</TableCell>
                  <TableCell>{usuario.firstname}</TableCell>
                  <TableCell>{usuario.lastname}</TableCell>
                  <TableCell>{usuario.email}</TableCell>
                  <TableCell>{usuario.phone}</TableCell>
                  <TableCell>{usuario.address.city}</TableCell>
                  <TableCell>{usuario.address.street}</TableCell>
                  <TableCell>{usuario.address.number}</TableCell>
                  <TableCell>{usuario.address.zipcode}</TableCell>
                  <TableCell>{usuario.address.geolocation.lat}</TableCell>
                  <TableCell>{usuario.address.geolocation.long}</TableCell>
                  <TableCell>{usuario.status}</TableCell>
                  <TableCell>{usuario.role}</TableCell>
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