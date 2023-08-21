import React from 'react';
import productService from "../services/productService";
import ProductDataIn from '../models/product';
import { toast } from 'react-toastify';
import Pagination from './pagination';
import '../styles/home-custom.css';
import ProductElement from '../components/product-comp/product-element'
class HomePage extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      products: [],
      showModal: false,
      selectedProduct: null,
      searchName: "",
      currentPage: 1,
      pageSize: 30,
      totalCount: 0,
      currentSlide: 0,
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
      };

      if (productData.searchName !== "") {
        productData.page = 1;
        this.setState({ currentPage: 1 });
      }

      const productsData = await productService.getAll(productData);
      const products = productsData.data.map(item => new ProductDataIn(
        item.category,
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
      console.log("Error ocurred:", error);
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

  handleSlideChange = (direction) => {
    const { currentSlide } = this.state;
    const slidesCount = 3; // Promenite broj ako dodate ili uklonite slajdove
    console.log(currentSlide)
    let newSlide = currentSlide + direction;
    if (newSlide < 0) {
      newSlide = slidesCount - 1;
    } else if (newSlide >= slidesCount) {
      newSlide = 0;
    }

    this.setState({ currentSlide: newSlide });
  };

  render() {
    const { totalCount, currentPage, pageSize, currentSlide } = this.state;

    return (
      <>
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.3/css/all.min.css" integrity="sha512-..." crossOrigin="anonymous" />

        <div id="slides-shop" className="cover-slides">
          <ul className="slides-container">
            <li className={`text-center ${currentSlide === 0 ? 'active1' : ''}`}>
              <img src="images/banner3.jpg" alt="" />
              <div className="container">
                <div className="row">
                  <div className="col-md-12">
                    <h1 className="m-b-20"><strong>Dobrodosli na nas <br /> SHOP ONLINE</strong></h1>
                    <p className="m-b-40">Kupujte po najpovoljnijim cenama na internetu <br /> i stedite vas novac.</p>
                    <p><a className="btn hvr-hover" href="#">KUPI ODMAH</a></p>
                  </div>
                </div>
              </div>
            </li>
            <li className={`text-center ${currentSlide === 1 ? 'active2' : ''}`}>
              <img src="images/banner2.avif" alt="" />
              <div className="container">
                <div className="row">
                  <div className="col-md-12">
                  <h1 className="m-b-20"><strong>Poruci i ocekuj paket <br /> u roku od 2 radna dana</strong></h1>
                  <p className="m-b-40">Vrsimo dostavu svim kurirskim sluzbama na teritoriji Srbije.</p>
                  <p><a className="btn hvr-hover" href="#">KUPI ODMAH</a></p>
                  </div>
                </div>
              </div>
            </li>
            <li className={`text-center ${currentSlide === 2 ? 'active3' : ''}`}>
              <img src="images/banner1.jpg" alt="" />
              <div className="container">
                <div className="row">
                  <div className="col-md-12">
                  <h1 className="m-b-20"><strong>Kupi i dobijas <br /> POKLON</strong></h1>
                  <p className="m-b-40">Kupovinom kod nas dobijate poklon iznenadjenja.</p>
                  <p><a className="btn hvr-hover" href="#">KUPI ODMAH</a></p>
                  </div>
                </div>
              </div>
            </li>
          </ul>
          <div className="slides-navigation">
            <a href="#" className="next" onClick={() => this.handleSlideChange(1)}><i className="fa fa-angle-right" aria-hidden="true"></i></a>
            <a href="#" className="prev" onClick={() => this.handleSlideChange(-1)}><i className="fa fa-angle-left" aria-hidden="true"></i></a>
          </div>
        </div>

        {/* PRODUCTS... */}
        <div clas="row">
          <div class="col-md-3">
            {this.state.products.map(product => (
                <ProductElement product={product} />
            ))}
          </div>
        </div>
      </>
    );
  }
}

export default HomePage;
