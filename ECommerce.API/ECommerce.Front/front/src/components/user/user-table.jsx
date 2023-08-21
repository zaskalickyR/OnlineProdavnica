import React from 'react';
import productService from "../../services/productService";
import ProductDataIn from '../../models/product';
import { toast } from 'react-toastify';
import Pagination from '../pagination';
import { Style } from '../../index.css';
import { FaCheckCircle, FaTimesCircle,FaEdit , FaTrash } from 'react-icons/fa';
import userServices from '../../services/userServices';
import CreateEditUser from './user-crud';
class UserTable extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      users: [],
      showModal: false,
      selectedProduct: null,
      searchName: "",
      currentPage: 1,
      pageSize: 7,
      totalCount: 0,
      isCheckboxChecked: false,
    };
  }

  openModal = (productId) => {

    if (productId == null) {
      this.setState({ showModal: true, selectedProduct: null });
    } else {
      const selectedProductTemp = this.state.users.find(product => product.id === productId);
      this.setState({ showModal: true, selectedProduct: selectedProductTemp });
    }
  }

  closeModal = () => {
    this.setState({ showModal: false, selectedProduct: null, productId: null });
    this.reloadTable(1);
  }


  deleteUser = (productId) => {
    userServices.deleteUser(productId);
    this.reloadTable(1);
  };

  async reloadTable(page) {
    try {
      const productData = {
        page: page,
        searchName: this.state.searchName,
        pageSize: this.state.pageSize,
        pageSize: this.state.pageSize,
        filterByUserRole: !this.state.isCheckboxChecked
      };

      if (productData.searchName !== "") {
        productData.page = 1;
        this.setState({ currentPage: 1 });
      }

      const productsData = await userServices.getAll(productData);
      const users = productsData.data;
      this.setState({ users: users, totalCount: productsData.count });
    } catch (error) {
      console.log("Došlo je do greške:", error);
    }
  }

  handleApprove = (productId) => {
    userServices.approveOrRejectUser(productId,true);
    this.reloadTable(1);
  };
  
  handleReject = (productId) => {
    userServices.approveOrRejectUser(productId,false);
    this.reloadTable(1);
  };

  componentDidMount() {
    this.reloadTable(1);
  }

  handleSearchSubmit = (event) => {
    event.preventDefault();
    this.reloadTable(1);
  }

  handleSearchChange = (event) => {
    this.setState({ searchName: event.target.value });
  }

  handlePageChange = (page) => {
    this.setState({ currentPage: page });
    this.reloadTable(page);
  };
  handleCheckboxChange = () => {
    this.setState(prevState => ({
      isCheckboxChecked: !prevState.isCheckboxChecked,
    }));
    this.reloadTable(1)
  };
  
  render() {
    const { totalCount, currentPage, pageSize } = this.state;

    return (
      <>
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.3/css/all.min.css" integrity="sha512-..." crossOrigin="anonymous" />
        <div className="width90">
          <div className="row">
            <div className="col-md-6"><h1 className="title">USERS</h1></div>
            <div className="col-md-6 row">
              <div className="icons row">
                {/* <div className="addBox" onClick={() => this.openModal()}>
                  <i className="fas fa-plus"></i>
                </div> */}
                <div className="checkBoxTest" onClick={() => this.handleCheckboxChange()}>
                  <input type='checkbox' className='cb' checked={this.state.isCheckboxChecked}/>
                  <span className='cbtext'>SALER</span>
                </div>
                <div className="searchBox">
                  <form name="search" onSubmit={this.handleSearchSubmit}>
                    <input type="text" className="input" name="searchName" value={this.state.searchName} onChange={this.handleSearchChange} />
                    <i className="fas fa-search" onClick={this.handleSearchSubmit}></i>
                  </form>
                </div>
              </div>
            </div>
          </div>
          <table>
            <thead>
              <tr>
                <th>Id</th>
                <th>First Name</th>
                <th>Last Name</th>
                <th>Email</th>
                <th>Username</th>
                <th>Image</th>
                <th>Role</th>
                <th>Birth date</th>
                <th>Status</th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              {this.state.users.map(product => (
                <tr key={product.id}>
                  <td>{product.id}</td>
                  <td>{product.firstName}</td>
                  <td>{product.lastName}</td>
                  <td>{product.email}</td>
                  <td>{product.userName}</td>
                  <td><img className='userImage' src={product.image}/></td>
                  <td>{product.role}</td>
                  <td>{product.birthDate}</td>
                  <td>
                    {product.status}
                    {product.status === 'Pending' && (
                      <div>
                        <button className='approveButton' onClick={() => this.handleApprove(product.id)}>
                          <FaCheckCircle />
                        </button>
                        <button className='rejectButton' onClick={() => this.handleReject(product.id)}>
                          <FaTimesCircle />
                        </button>
                      </div>
                    )}
                  </td>
                  <td>
                    {/* <button className='editButton' onClick={() => this.openModal(product.id)}>
                      <FaEdit />
                    </button> */}
                    <button className='removeButton' onClick={() => this.deleteUser(product.id)}>
                      <FaTrash />
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
          <Pagination count={totalCount} currentPage={currentPage} pageSize={pageSize} onPageChange={this.handlePageChange} />
          <CreateEditUser isOpen={this.state.showModal} onClose={this.closeModal} product={this.state.selectedProduct} />
        </div>
      </>
    );
  }
}

export default UserTable;
