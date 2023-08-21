import React from 'react';
function OrderItemTable({ isOpen, onClose, orderItems }) {
  return (
    <div className={`modal ${isOpen ? 'open' : ''}`}>
      <div className="modal-content">
        <span className="close" onClick={onClose}>&times;</span>
        <h2>Order Items</h2>
        <table>
          <thead>
            <tr>
              <th>Image</th>
              <th>Product</th>
              <th>Price</th>
              <th>Quantity</th>
              <th>Saler</th>
            </tr>
          </thead>
          <tbody>
            {orderItems.map(item => (
              <tr key={item.orderId}>
                <td><img src={item.image} width="70px"/></td>
                <td>{item.productName}</td>
                <td>{item.productPrice}</td>
                <td>{item.quantity}</td>
                <td>{item.saler}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}

export default OrderItemTable;
