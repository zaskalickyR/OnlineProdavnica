import React from 'react';
import productService from "../../services/productService";
import CreateEditProduct from './create-edit-product';
import ProductDataIn from '../../models/product';
import { toast } from 'react-toastify';
import Pagination from '../pagination';
import { FaCheckCircle, FaTimesCircle,FaEdit , FaTrash } from 'react-icons/fa';
import { Style } from '../../index.css';
class ProductTable extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      products: [],
      showModal: false,
      selectedProduct: null,
      searchName: "",
      currentPage: 1,
      pageSize: 5,
      totalCount: 0,
    };
  }

  openModal = (productId) => {
    if (productId == null) {
      this.setState({ showModal: true, selectedProduct: null });
    } else {
      const selectedProductTemp = this.state.products.find(product => product.id === productId);
      this.setState({ showModal: true, selectedProduct: selectedProductTemp });
    }
  }

  deleteProduct  = (productId) => {
    productService.deleteProduct(productId);
    this.reloadTable(1);
  };

  closeModal = () => {
    this.setState({ showModal: false, selectedProduct: null, productId: null });
    this.reloadTable(1);
  }

  async reloadTable(page) {
    try {
      const productData = {
        page: page,
        searchName: this.state.searchName,
        pageSize: this.state.pageSize,
        filterByUserRole: true
      };

      if (productData.searchName !== "") {
        productData.page = 1;
        this.setState({ currentPage: 1 });
      }

      const productsData = await productService.getAll(productData);
      const products = productsData.data.map(item => new ProductDataIn(
        item.categoryName,
        item.categoryId,
        item.customerId,
        item.description,
        item.id,
        item.images,
        item.name,
        item.lastUpdateTime,
        item.isDeleted,
        item.price,
        item.stock
      ));
      this.setState({ products: products, totalCount: productsData.count });
    } catch (error) {
      console.log("Došlo je do greške:", error);
    }
  }

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

  render() {
    const { totalCount, currentPage, pageSize } = this.state;

    return (
      <>
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.3/css/all.min.css" integrity="sha512-..." crossOrigin="anonymous" />
        <div className="width90">
          <div className="row">
            <div className="col-md-6"><h1 className="title">PROIZVODI</h1></div>
            <div className="col-md-6 row">
              <div className="icons row">
                <div className="addBox" onClick={() => this.openModal()}>
                  <i className="fas fa-plus"></i>
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
                <th>Image</th>
                <th>Name</th>
                <th>Price</th>
                <th>Stock</th>
                <th>Category</th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              {this.state.products.map(product => (
                <tr key={product.id}>
                  <td><img className='userImage' src={product.images}/></td>
                  <td>{product.name}</td>
                  <td>{product.price}</td>
                  <td>{product.stock}</td>
                  <td>{product.category}</td>
                  <td>
                    <button className='editButton' onClick={() => this.openModal(product.id)}>
                      <FaEdit />
                    </button>
                    <button className='removeButton' onClick={() => this.deleteProduct(product.id)}>
                      <FaTrash />
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
          <Pagination count={totalCount} currentPage={currentPage} pageSize={pageSize} onPageChange={this.handlePageChange} />
        </div>
        <CreateEditProduct isOpen={this.state.showModal} onClose={this.closeModal} product={this.state.selectedProduct} />
      </>
    );
  }
}

export default ProductTable;
