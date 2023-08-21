import React, { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { Style } from '../../styles/cart.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTimes } from '@fortawesome/free-solid-svg-icons';
import cartService from '../../services/cartService';

function CartSidebar({ onClose }) {
  const [cartItems, setCartItems] = useState([]);
  const [cartQuantity, setCartQuantity] = useState(0);
  const [totalAmount, setTotalAmount] = useState(0);

  useEffect(() => {
    const fetchCartItems = async () => {
      await updateCart();
    };

    fetchCartItems();
  }, []);

  useEffect(() => {
    const updatedCartQuantity = cartItems.reduce(
      (total, item) => total + item.count,
      0
    );
    const updatedTotalAmount = cartItems.reduce(
      (total, item) => total + item.price * item.count,
      0
    );

    setCartQuantity(updatedCartQuantity);
    setTotalAmount(updatedTotalAmount);
  }, [cartItems]);

  const handleCartClose = () => {
    onClose();
  };

  const inc = (product) => {
    cartService.addToCart(product);
    updateCart();
  };

  const dec = (productId) => {
    cartService.decreaseQuantity(productId);
    updateCart();
  };

  const rem = (productId) => {
    cartService.removeFromCart(productId);
    updateCart();
  };

  const navigate = useNavigate();

  const handleCheckout = () => {
    navigate('/checkout');
    handleCartClose();
  };

  const handleIncreaseQuantity = (itemId) => {
    cartService.increaseQuantity(itemId);
    const updatedCartItems = cartItems.map((item) => {
      if (item.id === itemId) {
        const updatedItem = { ...item };
        updatedItem.count += 1;
        return updatedItem;
      }
      return item;
    });
    setCartItems(updatedCartItems);
  };

  const handleRemoveFromCart = (itemId) => {
    cartService.removeFromCart(itemId);
    const updatedCartItems = cartItems.filter((item) => item.id !== itemId);
    setCartItems(updatedCartItems);
  };

  const updateCart = async () => {
    await cartService.updateCart();
    const cartItemsFromLocalStorage = localStorage.getItem('cartItems');
    if (cartItemsFromLocalStorage) {
      try {
        const parsedCartItems = JSON.parse(cartItemsFromLocalStorage);
        if (Array.isArray(parsedCartItems)) {
          setCartItems(parsedCartItems);
        }
      } catch (error) {
        console.error('Error parsing cart items from localStorage:', error);
      }
    }
  };
  return (
    <div className="sidebar">
      <div className="row">
        <div className="col-md-6">
          <h2 className="sidebar-title">CART ({cartQuantity})</h2>
        </div>
        <div className="col-md-6 text-right">
          <span className="close-button" onClick={handleCartClose}>
            CLOSE
          </span>
        </div>
      </div>
      {cartItems.length === 0 ? (
        <p className="empty-cart-message">Vaša korpa je prazna.</p>
      ) : (
        <ul className="cart-items-list">
          {cartItems.map((item) => (
            <li className="cart-item row" key={item?.id}>
              <div className="col-md-2 col1">
                <img className="cart-item-image" src={item?.images} alt="Product" />
              </div>
              <div className="col-md-6 col2">
                <div className="item-details">
                  <h3 className="item-name">{item?.name}</h3>
                  <div className="quantity-controls">
                    <button className="quantity-button decrease" onClick={() => dec(item?.id)}>
                      -
                    </button>
                    <span className="item-quantity">{item?.count}</span>
                    <button className="quantity-button increase" onClick={() => inc(item)}>
                      +
                    </button>
                  </div>
                  <p className="item-price">{item?.price} RSD x {item?.count}</p>
                </div>
              </div>
              <div className="col-md-2 text-right">
                <button className="remove-button" onClick={() => rem(item?.id)}>
                  <FontAwesomeIcon icon={faTimes} />
                </button>
              </div>
            </li>
          ))}
        </ul>
      )}
      {cartItems.length > 0 && (
        <div className="checkout-section">
          <p className="total-amount">TOTAL: {totalAmount} RSD</p>
          <button className="checkout-button" onClick={handleCheckout}>
            CHECKOUT
          </button>
        </div>
      )}
    </div>
  );
}

export default CartSidebar;
