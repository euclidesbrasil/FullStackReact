import axios from "axios";
import { useNavigate } from "react-router-dom";

const api = axios.create({
  baseURL: "http://localhost:5000/", // Substitua pela URL base da sua API
});

// Função para configurar o token Bearer
export function setAuthToken(token) {
  if (token) {
    api.defaults.headers.common["Authorization"] = `Bearer ${token}`;
  } else {
    delete api.defaults.headers.common["Authorization"];
  }
}

// Interceptor para lidar com respostas de erro
api.interceptors.response.use(
  response => response,
  error => {
    if (error.response && error.response.status === 401) {
      // Limpar o token do localStorage
      localStorage.removeItem("token");
      // Redirecionar para a página de login
      window.location.href = "/login";
    }
    return Promise.reject(error);
  }
);

export default api;