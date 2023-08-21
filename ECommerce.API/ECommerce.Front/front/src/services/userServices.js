import axios from 'axios';
import { decodeToken, isExpired } from "react-jwt";
import { toast } from 'react-toastify';
import axiosInstance from '../helpers/interceptor';
import { Link, useNavigate } from "react-router-dom";

function isLoggedIn() {
  let token = localStorage.getItem('token');
  if (!token || isExpired(token)) {
    return false;
  }
}

function logOut() {
  window.location.href = "/";
  window.location.reload(true);
  localStorage.removeItem("token");
  toast.warn('Success logout!');
}

function getCurrentUser() {
  let token = localStorage.getItem('token');
  if (!token) {
    return null;
  }
  return decodeToken(token);
}

const login = async (loginData) => {
  try {
    const response = await axiosInstance.post(`/user/login`, loginData);
    if ((response.status == 200 || response.status === "OK") && response.data.status == 200) {
      localStorage.setItem('token', JSON.stringify(response.data.transferObject));
      window.location.href = "/";
      toast.success(response.data.message);
    } else {
      toast.warn(response.data.message);
    }
  } catch (error) {
    toast.error(error);
  }
};

const facebookLogin = async (accessToken) => {
  try {
    const response = await axiosInstance.post(`/user/facebookLogin`, { facebookLoginToken: accessToken });
    if ((response.status == 200 || response.status === "OK") && response.data.status == 200) {
      localStorage.setItem('token', JSON.stringify(response.data.transferObject));
      window.location.href = "/";
      toast.success(response.data.message);
    } else {
      toast.warn(response.data.message);
    }
  } catch (error) {
    toast.error(error);
  }
};

const getUser = async (userId) => {
  try {
    const response = await axiosInstance.get(`/user/`+userId);
    if ((response.status == 200 || response.status === "OK") && response.data.status == 200) {
      return response.data.transferObject
    } else {
      toast.warn(response.data.message);
    }
  } catch (error) {
    toast.error(error);
  }
};

const getAll = async (getData) => {
  try {
    const response = await axiosInstance.post(`/user/getall`, getData);
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

const createUser = async (user) => {
  try {
    const response = await axiosInstance.post(`/user/register`, user);
    if ((response.status == 200 || response.status === "OK") && response.data.result.status == 200) {
      toast.success(response.data.result.message);
      return response.data.result
    } else {
      toast.warn(response.data.result.message);
    }
  } catch (error) {
    toast.success(error);
  }
};

const approveOrRejectUser = async (userId, action) => {
  try {
    const response = await axiosInstance.get(`/user/approveOrRejectUser/${userId}/${action}`);
    if ((response.status == 200 || response.status === "OK") && response.data.result.status == 200) {
      toast.success(response.data.result.message);
      return response.data.result
    } else {
      toast.warn(response.data.result.message);
    }
  } catch (error) {
    toast.success(error);
  }
};

const deleteUser = async (userId) => {
  try {
    const response = await axiosInstance.get(`/user/delete/${userId}`);
    if ((response.status == 200 || response.status === "OK") && response.data.result.status == 200) {
      toast.success(response.data.result.message);
    } else {
      toast.warn(response.data.result.message);
    }
  } catch (error) {
    toast.success(error);
  }
};

const userServices = {
  login,
  createUser,
  logOut,
  getCurrentUser,
  isLoggedIn,
  getUser,
  getAll,
  facebookLogin,
  deleteUser,
  approveOrRejectUser
};

export default userServices;
