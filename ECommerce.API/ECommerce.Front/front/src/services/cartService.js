import React from "react";
import baseUrl from '../components/endpoints';
import User from '../models/user';
import { toast } from 'react-toastify';
import axiosInstance from '../helpers/interceptor';
import { Product } from "../models/product";

function addToCart(product) {
  const cartItems = JSON.parse(localStorage.getItem('cartItems')) || [];
  const existingItem = cartItems.find((item) => item.id === product.id);

  if (existingItem && existingItem.count < existingItem.stock) {
    existingItem.count++;
    toast.success('Product added to cart!');
  } else if (!existingItem && product.stock >= 1) {
    product.count = 1;
    cartItems.push(product);
    toast.success('Product added to cart!');
  } else {
    toast.error('There are not enough products in stock');
  }

  localStorage.setItem('cartItems', JSON.stringify(cartItems));
}

function calculateShipping(){
  const cartItems = JSON.parse(localStorage.getItem('cartItems')) || [];
  let saleri = new Set(); // Koristimo Set da bismo sačuvali samo jedinstvene vrednosti salera
  cartItems.forEach(proizvod => {
    let saler = proizvod.customerId; // Pritupamo polju 'saler' za svaki proizvod
    saleri.add(saler); // Dodajemo salera u Set
  });
  return saleri.size * 250; // Vraćamo broj različitih salera (veličinu Set-a)
}


function removeFromCart(id) {
  const cartItems = JSON.parse(localStorage.getItem('cartItems')) || [];
  const itemIndex = cartItems.findIndex((item) => item.id === id);

  if (itemIndex !== -1) {
    cartItems.splice(itemIndex, 1);
    toast.success('Product removed from cart!')
  } else {
    toast.error('Product not found in cart')
  }

  localStorage.setItem('cartItems', JSON.stringify(cartItems));
}

function decreaseQuantity(id) {
  const cartItems = JSON.parse(localStorage.getItem('cartItems')) || [];
  const itemIndex = cartItems.findIndex((item) => item.id === id);

  if (cartItems[itemIndex] && cartItems[itemIndex].count >0) {
    cartItems[itemIndex].count--;
    if(cartItems[itemIndex].count == 0){
      cartItems.splice(itemIndex, 1);
      toast.warn('Product is remove from cart.')
    }
    toast.success('Item count increased!')
  } else if (!cartItems[itemIndex]) {
    toast.warn('Item not found in cart')
  } else {
    toast.error('Item count cannot exceed stock')
  }
  localStorage.setItem('cartItems', JSON.stringify(cartItems));
}

function truncateCart() {
  localStorage.removeItem("cartItems");
}

const updateCart = async () => {
  try {
    const cartItems = JSON.parse(localStorage.getItem('cartItems')) || [];
    var productIds = cartItems.map(function(product) {
      return product.id;
    });
    const response = await axiosInstance.post(`/product/updateCart`, productIds);
    if (response.status >= 200 && response.status < 300) {
      cartItems.forEach(element1 => {
        const elementFromDB = response.data.transferObject.find(element2 => element2.id === element1.id);
        if (elementFromDB && element1.price != elementFromDB.price) {
          toast.warn(element1.name + "is updated in the meantime.")
        }
        if(elementFromDB && elementFromDB.stock != element1.stock)
        {
          element1.stock = elementFromDB.stock;
          if(elementFromDB.stock < element1.count){
            element1.count = elementFromDB.stock;
            toast.error('The stock has changed, so we have reduced the number of '+element1.name+' to '+element1.count+' !');
          }
        }
        if(!elementFromDB)
        {
          const itemIndex = cartItems.findIndex((item) => item.id === element1.id);
          cartItems.splice(itemIndex, 1);
          toast.error(element1.name + " is deleted from cart, because is deleted from shop.")
        }
        if(element1.count == 0){
          const itemIndex = cartItems.findIndex((item) => item.id === element1.id);
          cartItems.splice(itemIndex, 1);
        }
      });
      localStorage.setItem('cartItems', JSON.stringify(cartItems));
      return true;
    } 
    else {
      toast.error("Error with updating cart.");
      return false;
    }
  } catch (error) {
    toast.error(error.message);
  }
};

export default {
  addToCart,
  truncateCart,
  updateCart,
  decreaseQuantity,
  removeFromCart,
  calculateShipping
};
