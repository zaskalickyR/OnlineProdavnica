import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';
import { BrowserRouter } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { ClipLoader } from 'react-spinners';

ReactDOM.render(
<>
    <BrowserRouter>
      <App />
    </BrowserRouter>
    <ToastContainer
      position="top-right"
      autoClose={3000}
      hideProgressBar={false}
      closeOnClick
    />
    <ClipLoader
      color={'white'}
      loading={true}
      size={150}
      css={`
        display: block;
        margin: 0 auto;
        background-color: purple;
      `}
    /></>
  ,
  document.getElementById('root')
);

reportWebVitals();
