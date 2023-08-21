import React, { useEffect, useState } from 'react';
import { useParams, Link, useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import productService from '../../services/productService';
import cartService from '../../services/cartService';
import '../../styles/single-product.css';

const ProductPage = () => {
  const { id } = useParams();
  const [product, setProduct] = useState(null);
  const navigate = useNavigate();

  const handleAddToCart = async (product) => {
    try {
      await cartService.addToCart(product);
      toast.success('Product added to cart.');
    } catch (error) {
      toast.error('Error adding product to cart.');
    }
  };

  useEffect(() => {
    const fetchProduct = async () => {
      try {
        const response = await productService.get(id);
        if (response.status === 200) {
          setProduct(response.transferObject); // Pretpostavka da je odgovor strukturiran kao { data: {} }
        } else {
          navigate('/');
          toast.error(response.message);
        }
      } catch (error) {
        console.error('Greška prilikom dobavljanja proizvoda:', error);
      }
    };

    fetchProduct();
  }, [id, navigate]);

  if (!product) {
    return <div>Loading...</div>;
  }

  return (
    <div className="container-product">
      <div className="card-product">
        <div className="row">
          <div className="col-md-4 kol2">
            <img src={product.images} alt={product.name} width="400px" />
          </div>
          <div className="col-md-6 kol1">
            <p className="title">{product.name}</p>
            <p className="product-desc">{product.description}</p>
            <p className="product-price">{product.price} RSD</p>
            <button className="addtocartproductpage" onClick={() => handleAddToCart(product)}>
              ADD TO CART
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ProductPage;
