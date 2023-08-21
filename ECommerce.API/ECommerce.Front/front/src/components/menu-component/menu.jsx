import { Link, useNavigate } from "react-router-dom";
import styles from "../../styles/menu.css";
import MenuItem from "./menuItem";
import userServices from "../../services/userServices";
import { toast } from "react-toastify";
import { FaShoppingCart } from 'react-icons/fa';
import cartService from '../../services/cartService'
import userService from '../../services/userServices'
import CartSidebar from '../../components/cart/cart-sidebar'
import React, { useState } from "react";
import { FaUser, FaSignOutAlt } from 'react-icons/fa';
import EditProfile from "../user/edit-profile";

function useLogout() {
  const [isCartModalOpen, setIsCartModalOpen] = useState(false);

  const openCartModal = () => {
    setIsCartModalOpen(true);
  };

  const closeCartModal = () => {
    setIsCartModalOpen(false);
  };

  const logout = () => {
    userService.logOut();
  }

  return { logout, openCartModal, closeCartModal, isCartModalOpen };
}


function ShowUserIcon() {
  const navigate = useNavigate();
  const user = userServices.getCurrentUser();
  const [isDropdownOpen, setIsDropdownOpen] = useState(false);
  const [isOpen, setIsOpen] = useState(false); // Dodano lokalno stanje isOpen
  const { logout, openCartModal, closeCartModal } = useLogout();

  const handleDropdownToggle = () => {
    setIsDropdownOpen(!isDropdownOpen);
  };

  const handleOpenModal = () => {
    setIsOpen(true);
  };
  const orders = () => {
    navigate('/orders')
  };
  const handleCloseModal = () => {
    setIsOpen(false);
  };

  const openModal = (productId) => {
    handleOpenModal();
  };

  const closeModal = () => {
    handleCloseModal();
  };

  return (
    <div className="userIcon">
      <div className="user-dropdown">
        <img
          src={user?.image}
          alt="Slika korisnika"
          className="user-avatar"
          onClick={handleDropdownToggle}
        />
        {isDropdownOpen && (
          <ul className="dropdown-menu">
            <li onClick={() => { openModal(user?.id); handleDropdownToggle(); }}>Profil</li>
            <li onClick={() => { orders(); handleDropdownToggle(); }}>Orders</li>
            <li onClick={() => { logout(); handleDropdownToggle(); }}>
              <FaSignOutAlt />
              Odjava
            </li>
          </ul>
        
        )}
      </div>
      <EditProfile isOpen={isOpen} onClose={handleCloseModal} productId={user?.id} />
    </div>
  );
}


function Menu() {
  const { logout, openCartModal, closeCartModal, isCartModalOpen } = useLogout();
  const user = userServices.getCurrentUser();
  const navigate = useNavigate();

  return (
    <>
      {user != null ? (
        <div className="menu-wrapper">
          <div className="menu-header">
            <img className="login-photo" src={require("../../images/logo.png")} onClick={() => { navigate('/'); }}/>
          </div>
          <div className="menu-right">
            <MenuItem item="Home" path="/" role={["Customer","Saler","Admin"]}/>
            <MenuItem item="Users" path="/users" role={["Admin"]}/>
            <MenuItem item="Products" path="/products" role={["Saler","Admin"]}/>
            <MenuItem item="Orders" path="/orders" role={["Customer","Saler","Admin"]}/>
          </div>
          <div className="menu-left">
            
            <ShowUserIcon/>
            <div className="cart-icon" onClick={openCartModal}>
              <FaShoppingCart/>
            </div>
          </div>
          {isCartModalOpen && <CartSidebar onClose={closeCartModal} />}
        </div>
      ) : null}
    </>
  );
}

export default Menu;
