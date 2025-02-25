import React, { useState, useEffect } from "react";
import { BrowserRouter as Router, Routes, Route, Link, Navigate, useNavigate } from "react-router-dom";
import { AppBar, Toolbar, Typography, Button, Container } from "@mui/material";
import ProdutosPage from "./Pages/Products/Products";
import ClientesPage from "./Pages/Clients/Clients";
import UsuariosPage from "./Pages/Users/Users";
import OrderList from "./Pages/Orders/OrderList";
import CreateOrder from "./Pages/Orders/CreateOrder";
import EditOrder from "./Pages/Orders/EditOrder";
import LoginPage from "./Pages/Login/LoginPage";
import Home from "./Pages/Home/Home";
import { setAuthToken } from "./axiosConfig";
import "./styles.css"; // Importando o arquivo CSS

const isAuthenticated = () => {
  const token = localStorage.getItem("token");
  if (token) {
    setAuthToken(token);
    return true;
  }
  return false;
};

const LogoutButton = ({ onLogout }) => {
  const navigate = useNavigate();

  const handleLogout = () => {
    localStorage.removeItem("token");
    onLogout();
    navigate("/login");
  };

  return (
    <Button color="inherit" onClick={handleLogout}>
      Logout
    </Button>
  );
};

export default function App() {
  const [authenticated, setAuthenticated] = useState(isAuthenticated());

  useEffect(() => {
    setAuthenticated(isAuthenticated());
  }, []);

  return (
    <Router>
      <Container>
        {authenticated ? (
          <>
            <AppBar position="static">
              <Toolbar>
                <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
                  Avaliação Técnica Full Stack - Euclides Brasil
                </Typography>
                <Button color="inherit" component={Link} to="/home">Home</Button>
                <Button color="inherit" component={Link} to="/produtos">Produtos</Button>
                <Button color="inherit" component={Link} to="/clientes">Clientes</Button>
                <Button color="inherit" component={Link} to="/usuarios">Usuários</Button>
                <Button color="inherit" component={Link} to="/orders">Pedidos</Button>
                <LogoutButton onLogout={() => setAuthenticated(false)} />
              </Toolbar>
            </AppBar>
            <Routes>
              <Route path="/home" element={<Home />} />
              <Route path="/produtos" element={<ProdutosPage />} />
              <Route path="/clientes" element={<ClientesPage />} />
              <Route path="/usuarios" element={<UsuariosPage />} />
              <Route path="/orders" element={<OrderList />} />
              <Route path="/orders/create" element={<CreateOrder />} />
              <Route path="/orders/edit/:orderId" element={<EditOrder />} />
              <Route path="/login" element={<LoginPage />} />
              <Route path="*" element={<Navigate to="/home" />} />
            </Routes>
          </>
        ) : (
          <Routes>
            <Route path="/login" element={<LoginPage />} />
            <Route path="/home" element={<Home />} />
            <Route path="*" element={<Navigate to="/login" />} />
          </Routes>
        )}
      </Container>
    </Router>
  );
}