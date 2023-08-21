import React from "react";
import axios from 'axios';
import ProductDataIn from '../models/product';
import { decodeToken, isExpired } from "react-jwt";
import { toast } from 'react-toastify';
import axiosInstance from '../helpers/interceptor';

const createProduct = async (productData) => {
  try {
    const response = await axiosInstance.post('/product/save', productData);
    if (response.status === 200 || response.status === 'OK') {
      if (response.data.message !== '') {
        toast.success(response.data.message);
      }
    } else if (response.status === 402) {
      toast.error(response.data.message);
    } else {
      toast.error('An error occurred.');
    }
    return response.data.result
  } catch (error) {
    toast.error(error.message);
  }
};

const getAll = async (getData) => {
  try {
    const response = await axiosInstance.post(`/product/getAll`, getData);
    if (response.status >= 200 && response.status < 300) {
      if(response.data.message != "")
        toast.success(response.data.message);

      return response.data.transferObject;
    } else if (response.status === 402) {
      toast.error(response.data.message);
    } else {
      toast.error("Error occurred.");
    }
  } catch (error) {
    toast.error(error.message);
  }
};

const deleteProduct = async (productId) => {
  try {
    const response = await axiosInstance.get(`/product/delete/${productId}`);
    if ((response.status == 200 || response.status === "OK") && response.data.result.status == 200) {
      toast.success(response.data.result.message);
    } else {
      toast.warn(response.data.result.message);
    }
  } catch (error) {
    toast.success(error);
  }
};

const get = async (productId) => {
  try {
    const response = await axiosInstance.get(`/product/get/${productId}`);
    console.log(response)

    if ((response.status == 200 || response.status === "OK") && response.data.status == 200) {
      toast.success(response.data.message);
      return response.data
    } else {
      toast.warn(response.data.message);
    }
  } catch (error) {
    toast.success(error);
  }
};

const productService = {
  createProduct,
  getAll,
  get,
  deleteProduct
};


export default productService;