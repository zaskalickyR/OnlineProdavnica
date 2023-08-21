import logo from './logo.svg';
import './App.css';
import { Routes, Route, useNavigate } from "react-router-dom";
import Menu from "./components/menu-component/menu";
import Register from "./components/register";
import Login from "./components/login";
import ProductTable from './components/product-comp/product-table';
import HomePage from './components/home';
import Checkout from './components/cart/checkout'
import EditProfile from './components/user/edit-profile';
import UserTable from './components/user/user-table';
import OrderTable from './components/order/order-table';
import ProductPage from './components/product-comp/single-product-page';
import userServices from './services/userServices';
import { toast } from 'react-toastify';

function App() {

  const currentUser = userServices.getCurrentUser();

  if(currentUser == null) // NE ULOGOVANI KORISNIK
  {
    return (
    <>
     <div className="main-container">
        <Menu />
        <Routes>
          <Route path="" element={<Login/>} />
          <Route path="login" element={<Login/>} />
          <Route path="register" element={<Register/>} />
          <Route path="*" element={<NotFound />} />
        </Routes>
      </div>
    </>
    );
  }
  else if(currentUser.role=='Customer')
  {
    return (
    <>
     <div className="main-container">
        <Menu />
        <Routes>
          <Route path="" element={<HomePage />} />
          <Route path="checkout" element={<Checkout/>} />
          <Route path="orders" element={<OrderTable/>} />
          <Route path="/product/:id" element={<ProductPage />} />
          <Route path="*" element={<NotFound />} />
        </Routes>
      </div>
    </>
    );
  }
  else if(currentUser.role=='Saler')
  {
    return (
    <>
     <div className="main-container">
        <Menu />
        <Routes>
        <Route path="" element={<HomePage />} />
          <Route path="checkout" element={<Checkout/>} />
          <Route path="orders" element={<OrderTable/>} />
          <Route path="/product/:id" element={<ProductPage />} />
          <Route path="products" element={<ProductTable/>} />
          <Route path="*" element={<NotFound />} />
          <Route path="*" element={<NotFound />} />
        </Routes>
      </div>
    </>
    );
  }
  else if(currentUser.role=='Admin')
  {
    return (
    <>
     <div className="main-container">
        <Menu />
        <Routes>
          <Route path="" element={<HomePage />} />
          <Route path="orders" element={<OrderTable/>} />
          <Route path="login" element={<Login />} />
          <Route path="register" element={<Register />} />
          <Route path="products" element={<ProductTable/>} />
          <Route path="orders" element={<OrderTable/>} />
          <Route path="users" element={<UserTable/>} />
          <Route path="checkout" element={<Checkout/>} />
          <Route path="product/:id" element={<ProductPage />} />
          <Route path="*" element={<NotFound />} />
          
        </Routes>
      </div>
    </>
    );
  }
}
function NotFound() {
  const navigate = useNavigate();
  window.location.href = "https://localhost:3000/";
  toast.error('Wrong page.');
  return null;
}

export default App;
