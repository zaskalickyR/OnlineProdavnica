import { Link, useNavigate } from "react-router-dom";
import { useForm, Controller } from 'react-hook-form';
import userServices from "../services/userServices";
import React, { useState } from "react";
import { toast } from 'react-toastify';
import "react-datepicker/dist/react-datepicker.css";
import { ToastContainer } from 'react-toastify';
import styles from "../styles/register.css";

function Register() {
  const navigate = useNavigate();

  const { register, handleSubmit, formState: { errors }, control, watch } = useForm();
  const [birthDate, setBirthDate] = useState(new Date());

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
      formData.append("password", registerData.password);
  
      const ret = await userServices.createUser(formData);
      
      if(ret.status == 200){
        toast.success(ret.message);
        window.location.reload(true);
        window.location.href = "/";
      }
      else{
        toast.success(ret.message)
      }
      navigate("/");
    } catch (error) {
    }
  };
  

  return (
    <>
      <div className="register-container">
        <img src={require("../images/logoWhite.png")} class="logo" />
        <div className="register-form-wrapper">
          <h1 className="header">REGISTER</h1>
          <div className="row">
            <div className="col-md-6">
              <form onSubmit={handleSubmit(onSubmit)} enctype="multipart/form-data">
                <label className='input-label'>
                  First Name <span>*</span>
                  <div className='input-wrapper'>
                    <input type="text" {...register("firstName", { required: true })} />
                  </div>
                </label>
                {errors.firstName && <span>This field is required</span>}
                
                <label className='input-label'>
                  UserName <span>*</span>
                  <div className='input-wrapper'>
                    <input type="text" {...register("username", { required: true })} />
                  </div>
                </label>
                {errors.userName && <span>This field is required</span>}
                <label className='input-label'>
                  Birth Date <span>*</span>
                  <div className='input-wrapper'>
                    <input type="date" {...register("birthDate", { required: true })} />
                  </div>
                </label>
                {errors.birthDate && <span>This field is required</span>}
                <label className='input-label'>
                  Role <span>*</span>
                  <div className='input-wrapper'>
                    <select {...register("roleId", { required: true })}>
                      <option value='1' selected>Customer</option>
                      <option value='2'>Seller</option>
                    </select>
                  </div>
                </label>
                {errors.roleId && <span>This field is required</span>}
                <label className='input-label'>
                  Profile Picture <span>*</span>
                  <div className='input-wrapper'>
                    <input type="file" accept="image/*" {...register("image", { required: true })} />
                  </div>
                </label>
                {errors.image && <span>This field is required</span>}
              </form>
            </div>
            <div className="col-md-6">
              <form onSubmit={handleSubmit(onSubmit)} enctype="multipart/form-data">
              <label className='input-label'>
                  Last Name <span>*</span>
                  <div className='input-wrapper'>
                    <input type="text" {...register("lastName", { required: true })} />
                  </div>
                </label>
                {errors.lastName && <span>This field is required</span>}
                <label className='input-label'>
                  Address <span>*</span>
                  <div className='input-wrapper'>
                    <input type="text" {...register("address", { required: true })} />
                  </div>
                </label>
                {errors.address && <span>This field is required</span>}
                <label className='input-label'>
                  Email <span>*</span>
                  <div className='input-wrapper'>
                    <input type="email" {...register("email", { required: true })} />
                  </div>
                </label>
                {errors.email && <span>This field is required</span>}
                
                <label className='input-label'>
                  Password <span>*</span>
                  <div className='input-wrapper'>
                    <input type="password" {...register("password", { required: true, minLength: 8, maxLength: 20 })} />
                  </div>
                </label>
                {errors.password?.type === 'required' && <span>This field is required</span>}
                {errors.password?.type === 'minLength' && <span>Password must be at least 8 characters long</span>}
                {errors.password?.type === 'maxLength' && <span>Password must be at most 20 characters long</span>}
                <label className='input-label'>
                  Confirm Password <span>*</span>
                  <div className='input-wrapper'>
                    <input
                      type="password"
                      {...register("confirmPassword", {
                        required: true,
                        validate: (value) => value === password, // Provjerava jednakost sa unosom šifre
                      })}
                    />
                  </div>
                </label>
                {errors.confirmPassword?.type === 'required' && <span>This field is required</span>}
                {errors.confirmPassword?.type === 'validate' && <span>Passwords do not match</span>}
                <button type="submit">REGISTER</button>
              </form>
            </div>
          </div>
        </div>
        <div className="login-link-wrapper">
          <Link to="/">Log in</Link>
        </div>
      </div>
    </>
  );
}

export default Register;
