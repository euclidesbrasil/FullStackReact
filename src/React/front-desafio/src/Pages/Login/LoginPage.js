import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { login } from "../../authService";
import { setAuthToken } from "../../axiosConfig";
import "./LoginPage.css"; // Importando o arquivo CSS específico para a página de login

export default function LoginPage() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const token = await login(username, password);
      if (token) {
        setAuthToken(token);
        localStorage.setItem("token", token); // Armazena o token no localStorage
        navigate("/home"); // Redireciona para a página de home
        window.location.reload(); // Força a recarga da página para atualizar o estado de autenticação
      } else {
        alert('Dados inválidos...');
      }
    } catch (error) {
      console.error("Erro ao fazer login:", error);
    }
  };

  return (
    <div className="login-container">
      <h1 className="login-title">Login</h1>
      <form className="login-form" onSubmit={handleSubmit}>
        <div className="login-field">
          <label>Usuário:</label>
          <input
            type="text"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
          />
        </div>
        <div className="login-field">
          <label>Senha:</label>
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
        </div>
        <button className="login-button" type="submit">Login</button>
      </form>
    </div>
  );
}