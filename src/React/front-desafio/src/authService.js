import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5000',
  headers: {
    'Content-Type': 'application/json',
    'Access-Control-Allow-Origin': 'http://localhost:3000', // Substitua pelo endereço do seu frontend
  },
});

export async function login(username, password) {
  try {
    const response = await api.post('/Auth/Login', {
      username,
      password,
    });
    return response.data.token; 
  } catch (error) {
    console.error('Erro ao autenticar:', error);
    throw error;
  }
}