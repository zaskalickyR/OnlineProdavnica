import React from "react";
import axios from 'axios';
import baseUrl from '../components/endpoints';
import ProductDataIn from '../models/product';
import { decodeToken, isExpired } from "react-jwt";
import { toast } from 'react-toastify';
import axiosInstance from '../helpers/interceptor';

const makeOrder = async (productData) => {
  try {
    const response = await axiosInstance.post('/Order/save', productData);
    console.log(response,'sss')
    if (response.status === 200 || response.status === 'OK') {
      if (response.data.message !== '') {
        toast.success(response.data.result.message);
        return response.data.result
      }
    } else if (response.status === 402) {
      toast.error(response.data.message);
    } else {
      toast.error('An error occurred.');
    }
  } catch (error) {
    toast.error(error.message);
  }
};

const getAll = async (getData) => {
  try {
    const response = await axiosInstance.post(`/order/getall`, getData);
    if (response.status >= 200 && response.status < 300) {
      if(response.data.message != "")
        toast.success(response.data.message);
      return response.data;
    } else if (response.status === 402) {
      toast.error(response.data.message);
    } else {
      toast.error("Error occurred.");
    }
  } catch (error) {
    toast.error(error.message);
  }
};

const cancelOrder = async (id) => {
  try {
    const response = await axiosInstance.get(`/order/cancelOrder/`+id);
    if (response.status >= 200 && response.status < 300) {
      toast.success(response.data.result.message);
      return response.data.result
    } else if (response.status === 402) {
      toast.error(response.data.message);
    } else {
      toast.error("Error occurred.");
    }
  } catch (error) {
    toast.error(error.message);
  }
};

export default {
  makeOrder,
  getAll,
  cancelOrder
};
