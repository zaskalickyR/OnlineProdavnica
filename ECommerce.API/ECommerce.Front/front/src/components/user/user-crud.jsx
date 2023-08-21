import { Link, useNavigate } from "react-router-dom";
import { useForm, Controller } from 'react-hook-form';
import userServices from "../../services/userServices";
import React, { useState, useEffect } from "react";
import { toast } from 'react-toastify';
import "react-datepicker/dist/react-datepicker.css";
import { ToastContainer } from 'react-toastify';
import styles from "../../styles/editProfile.css";

function CreateEditUser({ isOpen, onClose, productId }) {
  const navigate = useNavigate();

  const { register, handleSubmit, formState: { errors }, control, watch, setValue } = useForm();
  const [birthDate, setBirthDate] = useState(new Date());
  const [product, setProduct] = useState(null);

  const password = watch("password"); // Prati vrijednost unosa šifre
  const onSubmit = async (registerData) => {
    try {
      const formData = new FormData();
      formData.append("firstName", registerData.firstName);
      formData.append("lastName", registerData.lastName);
      formData.append("username", registerData.username);
      formData.append("birthDate", registerData.birthDate);
      formData.append("roleId", registerData.roleId);
      formData.append("image", registerData.image[0]);
      formData.append("address", registerData.address);
      formData.append("email", registerData.email);
      formData.append("id", registerData.id);
      formData.append("password", registerData.password);
  
      const ret = await userServices.createUser(formData);
      
      if (ret.status === 200) {
        toast.success(ret.message);
        userServices.logOut();
        window.location.href = "/";
        window.location.reload(true);
      
      } else {
        toast.error(ret.message);
      }
      navigate("/");

    } catch (error) {
    }
  };

  useEffect(() => {
    const fetchData = async () => {
      if (isOpen && productId) {
        const product = await userServices.getUser(productId);
        setProduct(product);
      }
    };
  
    fetchData();
  }, [isOpen, productId]);
  
  useEffect(() => {
    if (product) {
      setValue("firstName", product.firstName);
      setValue("lastName", product.lastName);
      setValue("address", product.address);
      setValue("birthDate", product.birthDate);
      setValue("email", product.email);
      setValue("image", product.image);
      setValue("id", product.id);
    }
  }, [product]);
  
  

  return (
    <>
      {isOpen && (
        <div className="editProduct">
          <div className="edit-profile-modal">
            <button className="close-button" onClick={onClose}>x</button>
            <div className="modal-content">
              <div className="register-container">
                <div className="register-form-wrapper">
                  <img src={product?.image} className="imageProfileEdit" alt="Profile Picture" />
                  <form onSubmit={handleSubmit(onSubmit)} encType="multipart/form-data">
                    <input type="text" {...register("id", { required: false })} hidden />
                    <div className="row">
                      <div className="col-md-6">
                        <label className="input-label">
                          First Name
                          <div className="input-wrapper">
                            <input type="text" {...register("firstName", { required: false })} />
                          </div>
                        </label>
                        <label className="input-label">
                          Last Name
                          <div className="input-wrapper">
                            <input type="text" {...register("lastName", { required: false })} />
                          </div>
                        </label>
                        <label className="input-label">
                          Address
                          <div className="input-wrapper">
                            <input type="text" {...register("address", { required: false })} />
                          </div>
                        </label>
                        <label className="input-label">
                          Birth Date
                          <div className="input-wrapper">
                            <input type="date" {...register("birthDate", { required: false })} />
                          </div>
                        </label>
                      </div>
                      <div className="col-md-6">
                        <label className="input-label">
                          Profile Picture
                          <div className="input-wrapper">
                            <input type="file" accept="image/*" {...register("image", { required: false })} />
                          </div>
                        </label>
                        <label className="input-label">
                          Email
                          <div className="input-wrapper">
                            <input type="email" {...register("email", { required: false })} disabled/>
                          </div>
                        </label>
                        <label className="input-label">
                          Password
                          <div className="input-wrapper">
                            <input
                              type="password"
                              {...register("password", { required: false, minLength: 8, maxLength: 20 })}
                            />
                          </div>
                        </label>
                        {errors.password?.type === "minLength" && <span>Password must be at least 8 characters long</span>}
                        {errors.password?.type === "maxLength" && <span>Password must be at most 20 characters long</span>}
                        <label className="input-label">
                          Confirm Password
                          <div className="input-wrapper">
                            <input
                              type="password"
                              {...register("confirmPassword", {
                                required: false,
                                validate: (value) => value === password, // Provjerava jednakost sa unosom šifre
                              })}
                            />
                          </div>
                        </label>
                        {errors.confirmPassword?.type === "validate" && <span>Passwords do not match</span>}
                      </div>
                    </div>
                    <button type="submit">EDIT</button>
                  </form>
                </div>
              </div>
            </div>
          </div>
        </div>
      )}
    </>
  );
}

export default CreateEditUser;
