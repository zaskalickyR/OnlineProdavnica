import React, { useState } from 'react';
import { FaCheckCircle, FaTimesCircle } from 'react-icons/fa';
import { Style } from './../../styles/order.css';
import orderService from '../../services/orderService';

const OrderStatus = ({ order }) => {
  const getCurrentTime = () => {
    return new Date().getTime(); // Trenutno vrijeme u milisekundama
  };

  const getOrderTime = () => {
    return new Date(order?.orderDate).getTime(); // Vrijeme narudžbe u milisekundama
  };

  const getShippingTime = () => {
    return new Date(order?.shippingTime).getTime(); // Vrijeme isporuke u milisekundama
  };

  const getStatus = () => {
    const currentTime = getCurrentTime();
    const orderTime = getOrderTime();
    const shippingTime = getShippingTime();
    if (!order || !order.status) {
      return '';
    } else if (order.status === 'Rejected') {
      return 'Rejected';
    } else if (currentTime - orderTime < 3600000 && order.status === 'Pending') {
      return 'Pending';
    } else if (currentTime > orderTime && currentTime < shippingTime && order.status === 'Pending') {
      return 'Approved';
    } else if (currentTime > shippingTime) {
      return 'Shipped';
    }
    return '';
  };

  const handleReject = async (orderId) => {
    if (!order || !order.id) {
      return;
    }
    const temp = await orderService.cancelOrder(order.id);
    if (temp.status === 200) {
      setStatus('Rejected');
    }
  };

  const [status, setStatus] = useState(getStatus());

  return (
    <div>
      <div className={`statusOrder ${status.toLowerCase()}`}>
        {status}
        
      </div>
      <div>
        {status === 'Approved' && (
          <div>
            <button className='rejectButton' onClick={() => handleReject(order?.id)}>
              <FaTimesCircle /> <span className='cancelorderbutton'>CANCEL ORDER</span>
            </button>
          </div>
        )}
      </div>
    </div>
  );
};

export default OrderStatus;
