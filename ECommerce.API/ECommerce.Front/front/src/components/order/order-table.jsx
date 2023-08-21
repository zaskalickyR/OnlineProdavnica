import React, { useState, useEffect } from 'react';
import orderService from "../../services/orderService";
import orderDataIn from '../../models/order';
import { toast } from 'react-toastify';
import Pagination from '../pagination';
import '../../index.css'; // Ispravljena izjava za uvoz CSS-a
import { FaCheckCircle, FaTimesCircle, FaEdit, FaTrash, FaRegListAlt } from 'react-icons/fa';
import OrderItemTable from './order-item-table';
import OrderStatus from './order-status';
import CountdownTimerShipping from './countdown';

function OrderTable() {
  const [orders, setOrders] = useState([]);
  const [showModal, setShowModal] = useState(false);
  const [selectedOrder, setSelectedOrder] = useState(null);
  const [searchName, setSearchName] = useState("");
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(5);
  const [totalCount, setTotalCount] = useState(0);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isCheckboxChecked, setCheckboxChecked] = useState(true);
  const [expandedOrderId, setExpandedOrderId] = useState(null); // New state variable

  const openModal = (orderId) => {
    if (orderId == null) {
      setShowModal(true);
      setSelectedOrder(null);
      setIsModalOpen(true);
    } else {
      const selectedOrderTemp = orders.find(order => order.id === orderId);
      setShowModal(true);
      setSelectedOrder(selectedOrderTemp);
      setIsModalOpen(true);
    }
    setExpandedOrderId(orderId); // Update the expanded order ID
  };

  const closeModal = () => {
    setShowModal(false);
    setSelectedOrder(null);
    setIsModalOpen(false);
    reloadTable(1);
  };

  const deleteUser = (orderId) => {
    orderService.deleteUser(orderId);
    reloadTable(1);
  };

  const reloadTable = async (page) => {
    try {
      const orderData = {
        page: page,
        searchName: searchName,
        pageSize: pageSize,
        filterByUserRole: !isCheckboxChecked
      };

      if (orderData.searchName !== "") {
        orderData.page = 1;
        setCurrentPage(1);
      }

      const ordersData = await orderService.getAll(orderData);
      setOrders(ordersData.transferObject.data);
      setTotalCount(ordersData.transferObject.count);
    } catch (error) {
      console.log("Došlo je do greške:", error);
    }
  };

  const handleApprove = (orderId) => {
    orderService.approveOrRejectUser(orderId, true);
    reloadTable(1);
  };
  
  const handleReject = (orderId) => {
    orderService.approveOrRejectUser(orderId, false);
    reloadTable(1);
  };

  useEffect(() => {
    reloadTable(1);
  }, []);

  const handleSearchSubmit = (event) => {
    event.preventDefault();
    reloadTable(1);
  };

  const handleSearchChange = (event) => {
    setSearchName(event.target.value);
  };
  
  const handlePageChange = (page) => {
    setCurrentPage(page);
    reloadTable(page);
  };
  
  const handleCheckboxChange = () => {
    setCheckboxChecked(!isCheckboxChecked);
    reloadTable(1);
  };

  return (
    <>
      <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.3/css/all.min.css" integrity="sha512-..." crossOrigin="anonymous" />
      <div className="width90">
        <div className="row">
          <div className="col-md-6"><h1 className="title">ORDERS</h1></div>
          <div className="col-md-6 row">
            <div className="icons row">
              {/* <div className="addBox" onClick={() => openModal()}>
                <i className="fas fa-plus"></i>
              </div> */}
              <div className="checkBoxTest" onClick={handleCheckboxChange}>
                <input type='checkbox' className='cb' checked={isCheckboxChecked} />
                <span className='cbtext'>NEW ORDERS</span>
              </div>
              <div className="searchBox">
                <form name="search" onSubmit={handleSearchSubmit}>
                  <input type="text" className="input" name="searchName" value={searchName} onChange={handleSearchChange} />
                  <i className="fas fa-search" onClick={handleSearchSubmit}></i>
                </form>
              </div>
            </div>
          </div>
        </div>
        <table>
          <thead>
            <tr>
              <th>Id</th>
              <th>Customer</th>
              <th>Shipping Data</th>
              <th>Comment</th>
              <th>Total</th>
              <th>Shipping</th>
              <th>Date</th>
              <th>Status</th>
              <th>No orders</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            {orders.map(order => (
              <React.Fragment key={order.id}>
                <tr>
                  <td>{order?.id}</td>
                  <td>{order?.customer} ({order?.customerId})</td>
                  <td>{order?.name} <br/> {order?.address} <br/> {order?.phone}</td>
                  <td>{order?.comment}</td>
                  <td>{order?.total} RSD</td>
                  <td>{order?.shipping} RSD</td>
                  <td>
                    <b>Order Date</b><br/>{order?.orderDate} <br/> <b>Shipping Date</b> <br/> {order?.shippingTime}
                    <CountdownTimerShipping order={order} />
                  </td>
                  <td>
                    <OrderStatus order={{id: order?.id, orderDate: order?.orderDate, shippingTime: order?.shippingTime, status: order?.status}} />
                  </td>
                  <td>{order?.orderItems.length}</td>
                  <td>
                    <button className='editButton' onClick={() => openModal(order?.id)}>
                      <FaRegListAlt />
                    </button>
                    {/* <button className='removeButton' onClick={() => deleteUser(order?.id)}>
                      <FaTrash />
                    </button> */}
                  </td>
                </tr>
                {expandedOrderId === order.id && (
                  <tr>
                    <td colSpan="9">
                      {isModalOpen && (
                        <OrderItemTable
                          isOpen={isModalOpen}
                          onClose={closeModal}
                          orderItems={order.orderItems}
                        />
                      )}
                    </td>
                  </tr>
                )}
              </React.Fragment>
            ))}
          </tbody>
        </table>
        <Pagination count={totalCount} currentPage={currentPage} pageSize={pageSize} onPageChange={handlePageChange} />        
      </div>
    </>
  );
}

export default OrderTable;
