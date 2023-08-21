import React, { useEffect } from "react";
import styles from "../styles/login.css";
import { Link, useNavigate } from "react-router-dom";
import { useForm } from 'react-hook-form';
import userServices from "../services/userServices"
import { toast } from "react-toastify";
import FacebookLoginButton from "../helpers/FacebookLoginButton";

function Login() {
  const navigate = useNavigate();

  const { register, handleSubmit, formState: { errors } } = useForm();

  const onSubmit = async (loginData) => {
    try {
      await userServices.login(loginData);
      navigate('/');
    } catch (error) {
      toast.error(error.message);
    }
  };

  useEffect(() => {
    if (userServices.isLoggedIn()) {
      navigate('/');
    }
  }, []);

  const handleLoginSuccess = (accessToken) => {
    // Obrada uspešne prijave
    toast.success('Facebook login successfuly!')
    console.log('Facebook login successfuly. Access token:', accessToken);
  };

  const handleLoginFailure = (error) => {
    // Obrada neuspešne prijave
    toast.error('Facebook login failed.')
    console.log('Facebook login error:', error);
  };

  return (
    <>
      <div className='login'>
        <img src={require("../images/logoWhite.png")} class="logo" />
        <div className="frame">
          <h1>LOGIN</h1>
          <form onSubmit={handleSubmit(onSubmit)}>
            <label className='input-label'>
              Email
              <div className='input-wrapper'>
                <input type="text" {...register("email", { required: true })} />
              </div>
            </label>
            {errors.email && <span>This field is required</span>}
            <label className='input-label'>
              Password
              <div className='input-wrapper'>
                <input type="password" {...register("password", { required: true })} />
              </div>
            </label>
            {errors.password && <span>This field is required</span>}
            
            <button type="submit">LOGIN</button><br/>
            <FacebookLoginButton 
              onLoginSuccess={handleLoginSuccess}
              onLoginFailure={handleLoginFailure}
            />
          </form>
        </div>
        <div className="login-link-wrapper">
          <Link to="/register">Register</Link>
        </div>
      </div>
    </>
  );
}

export default Login;
