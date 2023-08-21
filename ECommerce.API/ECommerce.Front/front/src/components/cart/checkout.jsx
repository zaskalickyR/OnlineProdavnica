import React, { useState, useEffect } from 'react';
import { Style } from '../../styles/checkout.css';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import orderService from '../../services/orderService'
import cartService from '../../services/cartService';
import { toast } from 'react-toastify';
import { Link, useNavigate } from "react-router-dom";
import userServices from '../../services/userServices';

function Checkout() {
  const currentUser = userServices.getCurrentUser();
  const [firstName, setFirstName] = useState(currentUser.name || '');
  const [lastName, setLastName] = useState(currentUser.lastName || '');
  const [address, setAddress] = useState(currentUser.address || '');
  const [email, setEmail] = useState(currentUser.email || '');
  const [phoneNumber, setPhoneNumber] = useState('');
  const [paymentMethod, setPaymentMethod] = useState('');
  const [deliveryMethod, setDeliveryMethod] = useState('');
  const [comment, setComment] = useState('');
  const [errors, setErrors] = useState({});
  const [cartItems, setCartItems] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    const updateCart = async () => {
      await cartService.updateCart();
      const updatedCartItems = JSON.parse(localStorage.getItem('cartItems')) || [];
      setCartItems(updatedCartItems);
    };
  
    updateCart();
  }, []);
  

  const calculateTotal = () => {
    let total = 0;
    cartItems.forEach((item) => {
      total += item.price * item.count;
    });
    return total;
  };
 
  const handleFirstNameChange = (e) => {
    setFirstName(e.target.value);
  };

  const handleEmailChange = (e) => {
    setEmail(e.target.value);
  };

  const handleLastNameChange = (e) => {
    setLastName(e.target.value);
  };

  const handleAddressChange = (e) => {
    setAddress(e.target.value);
  };

  const handlePhoneNumberChange = (e) => {
    setPhoneNumber(e.target.value);
  };

  const handlePaymentMethodChange = (e) => {
    setPaymentMethod(e.target.value);
  };

  const handleCommentChange = (e) => {
    setComment(e.target.value);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const validationErrors = validateForm();
    if (Object.keys(validationErrors).length > 0) {
      setErrors(validationErrors);
      return;
    }

    const order = {
      firstName,
      lastName,
      address,
      phoneNumber,
      paymentMethod,
      deliveryMethod,
      cartItems,
      comment,
    };

    try {
      const ret = await orderService.makeOrder(order);
      if (ret.status === 200) {
        toast.success(ret.message);
        navigate('/');
      } else if (ret.status === 400) {
        toast.warn(ret.message);
        cartService.updateCart();
        const updatedCartItems = JSON.parse(localStorage.getItem('cartItems')) || [];
        setCartItems(updatedCartItems);
      } else {
        toast.warn(ret.message);
      }
    } catch (error) {
      toast.error('Error occurred: ' + error);
    }

    // Reset form values
    setFirstName('');
    setLastName('');
    setAddress('');
    setPhoneNumber('');
    setPaymentMethod('');
    setDeliveryMethod('');
    setErrors({});
    cartService.truncateCart();
  };

  const validateForm = () => {
    const errors = {};

    if (firstName.trim() === '') {
      errors.firstName = 'First Name is required';
    }

    if (lastName.trim() === '') {
      errors.lastName = 'Last Name is required';
    }

    if (address.trim() === '') {
      errors.address = 'Address is required';
    }

    if (phoneNumber.trim() === '') {
      errors.phoneNumber = 'Phone Number is required';
    }

    return errors;
  };

  return (
    <div className="checkout">
      <div className="container-fluid pt-5">
        <div className="row">
          <div className="col-md-6 leftSide">
            <div className="mb-4">
              <h4 className="shippingDetailsTitle">SHIPPING DETAILS</h4>
              <div className="row">
                <div className="col-md-6 form-group">
                  <label>First Name</label>
                  <input
                    className="form-control"
                    type="text"
                    placeholder=""
                    onChange={handleFirstNameChange}
                    value={firstName}
                  />
                  {errors.firstName && (
                    <p className="error">{errors.firstName}</p>
                  )}
                </div>
                <div className="col-md-6 form-group">
                  <label>Last Name</label>
                  <input
                    className="form-control"
                    type="text"
                    placeholder=""
                    onChange={handleLastNameChange}
                    value={lastName}
                  />
                  {errors.lastName && <p className="error">{errors.lastName}</p>}
                </div>
                <div className="col-md-6 form-group">
                  <label>Mobile No</label>
                  <input
                    pattern="[0-9]*"
                    className="form-control"
                    type="text"
                    placeholder=""
                    onChange={handlePhoneNumberChange}
                    value={phoneNumber}
                  />
                  {errors.phoneNumber && (
                    <p className="error">{errors.phoneNumber}</p>
                  )}
                </div>
                <div className="col-md-6 form-group">
                  <label>Address</label>
                  <input
                    className="form-control"
                    type="text"
                    placeholder=""
                    onChange={handleAddressChange}
                    value={address}
                  />
                  {errors.address && <p className="error">{errors.address}</p>}
                </div>
              </div>
              <div className="row">
                <label>Comment</label>
                <textarea name="comment" onChange={handleCommentChange}></textarea>
                <button className="submitOrderButton" onClick={handleSubmit}>
                  Submit
                </button>
              </div>
            </div>
          </div>
          <div className="col-md-6 rightSide">
            <div className="card border-secondary mb-5">
              <div className="card-header bg-secondary border-0">
                <h4 className="orderDetailsTitle">ORDER DETAILS</h4>
              </div>
              <div className="card-body">
                {cartItems.map((item) => (
                  <div className="d-flex justify-content-between">
                    <p>{item?.name}</p>
                    <p>
                      <span>{item?.count} x</span> <b>{item?.price} RSD</b>
                    </p>
                  </div>
                ))}
              </div>

              <div className="card-footer border-secondary bg-transparent">
                <div className="row">
                  <div className='col-md-4'>
                  <h7 className="titleCheckout">PRODUCTS</h7>
                  <h3 className="valueCheckout">{calculateTotal()} RSD</h3>
                  </div>
                  <div className='col-md-4'>
                  <h7 className="titleCheckout">SHIPPING</h7>
                  <h3 className="valueCheckout">{cartService.calculateShipping()} RSD</h3>
                  </div>
                  <div className='col-md-4'>
                  <h7 className="titleCheckout">TOTAL</h7>
                  <h3 className="valueCheckout">{calculateTotal()+cartService.calculateShipping()} RSD</h3>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default Checkout;
