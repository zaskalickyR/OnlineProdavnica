import React from 'react';
import FacebookLogin from 'react-facebook-login';
import styles from '../styles/facebookLogin.css'
import userServices from '../services/userServices';
import { Link, useNavigate } from "react-router-dom";

const FacebookLoginButton = ({ onLoginSuccess, onLoginFailure }) => {
  const navigate = useNavigate();

  const responseFacebook = async (response) => {
    if (response.accessToken) {
      await userServices.facebookLogin(response.accessToken);
      onLoginSuccess(response.accessToken);
    } else {
      navigate('/');
      onLoginFailure(response);
    }
  };

  const handleFacebookLogin = async () => {
    await responseFacebook();
  };

  return (
    <FacebookLogin
      appId="308064178375467"
      autoLoad={false}
      fields="name,email,picture"
      callback={responseFacebook} // Bez await ovde
      cssClass="facebook-login-button"
      render={(renderProps) => (
        <button onClick={handleFacebookLogin}>FACEBOOK LOGIN</button>
      )}
    />
  );
};

export default FacebookLoginButton;
