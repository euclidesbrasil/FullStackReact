import React, { useState, useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { TextField, Button, MenuItem, Typography, Container, Divider, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper } from "@mui/material";
import api, { setAuthToken } from "../../axiosConfig";

export default function EditOrder() {
  const [customers, setCustomers] = useState([]);
  const [products, setProducts] = useState([]);
  const [customerId, setCustomerId] = useState("");
  const [items, setItems] = useState([]);
  const [newItem, setNewItem] = useState({ productId: "", quantity: 1 });
  const [status, setStatus] = useState(1);
  const navigate = useNavigate();
  const { orderId } = useParams();

  useEffect(() => {
    async function fetchData() {
      try {
        const token = localStorage.getItem("token");
        setAuthToken(token);
        const customersResponse = await api.get("/Customers");
        const productsResponse = await api.get("/Products");
        const orderResponse = await api.get(`/Orders/${orderId}`);
        setCustomers(customersResponse.data.data);
        setProducts(productsResponse.data.data);
        setCustomerId(orderResponse.data.customer.id);
        setItems(orderResponse.data.items);
        setStatus(
            orderResponse.data.status === "Active" ? 1 :
            orderResponse.data.status === "Canceled" ? 2 : 0
        );
      } catch (error) {
        console.error("Erro ao buscar dados:", error);
      }
    }

    fetchData();
  }, [orderId]);

  const handleAddItem = () => {
    setItems([...items, newItem]);
    setNewItem({ productId: "", quantity: 1 });
  };

  const handleRemoveItem = (index) => {
    const newItems = items.filter((_, i) => i !== index);
    setItems(newItems);
  };

  const handleNewItemChange = (field, value) => {
    setNewItem({ ...newItem, [field]: value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const orderData = {
        id: orderId,
        orderDate: new Date().toISOString(),
        customerId,
        items,
        status,
      };
      await api.put(`/Orders?id=${orderId}`, orderData);
      navigate("/orders");
    } catch (error) {
      console.error("Erro ao editar pedido:", error);
    }
  };

  return (
    <Container>
      <Typography variant="h4" gutterBottom>
        Editar Pedido
      </Typography>
      <form onSubmit={handleSubmit}>
        <Typography variant="h6" gutterBottom>
          Inclusão de Campos
        </Typography>
        <TextField
          select
          label="Cliente"
          value={customerId}
          onChange={(e) => setCustomerId(e.target.value)}
          fullWidth
          margin="normal"
        >
          {customers.map((customer) => (
            <MenuItem key={customer.id} value={customer.id}>
              {customer.name}
            </MenuItem>
          ))}
        </TextField>
        <TextField
          select
          label="Status"
          value={status}
          onChange={(e) => setStatus(e.target.value)}
          fullWidth
          margin="normal"
        >
          <MenuItem value={1}>Active</MenuItem>
          <MenuItem value={2}>Canceled</MenuItem>
        </TextField>
        <TextField
          select
          label="Produto"
          value={newItem.productId}
          onChange={(e) => handleNewItemChange("productId", e.target.value)}
          fullWidth
          margin="normal"
        >
          {products.map((product) => (
            <MenuItem key={product.id} value={product.id}>
              {product.name}
            </MenuItem>
          ))}
        </TextField>
        <TextField
          label="Quantidade"
          type="number"
          value={newItem.quantity}
          onChange={(e) => handleNewItemChange("quantity", e.target.value)}
          fullWidth
          margin="normal"
        />
        <Button variant="contained" color="primary" onClick={handleAddItem}>
          Adicionar Item
        </Button>
        <Divider sx={{ margin: "20px 0" }} />
        <Typography variant="h6" gutterBottom>
          Itens Adicionados
        </Typography>
        <TableContainer component={Paper}>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>Produto</TableCell>
                <TableCell>Quantidade</TableCell>
                <TableCell>Ações</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {items.map((item, index) => (
                <TableRow key={index}>
                  <TableCell>{products.find((product) => product.id === item.productId)?.name || "N/A"}</TableCell>
                  <TableCell>{item.quantity}</TableCell>
                  <TableCell>
                    <Button variant="contained" color="secondary" onClick={() => handleRemoveItem(index)}>
                      Remover
                    </Button>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </TableContainer>
        <Button variant="contained" color="primary" type="submit" sx={{ marginTop: "20px" }}>
          Editar Pedido
        </Button>
      </form>
    </Container>
  );
}