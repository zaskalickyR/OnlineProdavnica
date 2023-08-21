import { Link, useNavigate } from "react-router-dom";
import userServices from "../../services/userServices";
import React, { useState, useEffect } from "react";
import { toast } from 'react-toastify';
import "react-datepicker/dist/react-datepicker.css";
import { ToastContainer } from 'react-toastify';
import styles from "../../styles/editProfile.css";

function EditProfile({ isOpen, onClose, productId }) {
  const navigate = useNavigate();

  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    username: "",
    birthDate: "",
    roleId: "1",
    image: null,
    address: "",
    email: "",
    password: "",
    confirmPassword: ""
  });

  const [product, setProduct] = useState(null);
  const [formattedDate, setFormattedDate] = useState('');

  const handleDateChange = (e) => {
    const formattedDate = new Date(e.target.value).toISOString().split('T')[0];
    setFormattedDate(formattedDate);
  };

  const handleInputChange = (event) => {
    const { name, value } = event.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleImageChange = (event) => {
    const file = event.target.files[0];
    setFormData({ ...formData, image: file });
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
  
    try {
      const formDataToSend = new FormData();
      formDataToSend.append("firstName", formData.firstName);
      formDataToSend.append("lastName", formData.lastName);
      formDataToSend.append("username", formData.username);
      formDataToSend.append("birthDate", formattedDate); // koristimo ažuriranu vrednost formattedDate
      formDataToSend.append("roleId", formData.roleId);
      formDataToSend.append("image", formData.image);
      formDataToSend.append("address", formData.address);
      formDataToSend.append("email", formData.email);
      formDataToSend.append("id", formData.id);
      formDataToSend.append("password", formData.password);
  
      const ret = await userServices.createUser(formDataToSend);
  
      if (ret.status === 200) {
        toast.success("Successfully changed profile.");
        userServices.logOut();
        navigate('/');
      } else {
        toast.error(ret.message);
      }
      navigate("/");
  
    } catch (error) {
      // handle error
    }
  };
  

  useEffect(() => {
    const fetchData = async () => {
      if (isOpen && productId) {
        const product = await userServices.getUser(productId);
        setProduct(product);
        const formattedDate = new Date(product?.birthDate).toISOString().split('T')[0];
        setFormattedDate(formattedDate);
      }
    };

    fetchData();
  }, [isOpen, productId]);

  useEffect(() => {
    if (product) {
      setFormData({
        ...formData,
        firstName: product.firstName,
        lastName: product.lastName,
        address: product.address,
        birthDate: product.birthDate,
        email: product.email,
        image: product.image,
        id: product.id
      });
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
                  <form onSubmit={handleSubmit} encType="multipart/form-data">
                    <input type="text" name="id" value={formData.id} hidden />
                    <div className="row">
                      <div className="col-md-6">
                        <label className="input-label">
                          First Name
                          <div className="input-wrapper">
                            <input type="text" name="firstName" value={formData.firstName} onChange={handleInputChange} />
                          </div>
                        </label>
                        <label className="input-label">
                          Last Name
                          <div className="input-wrapper">
                            <input type="text" name="lastName" value={formData.lastName} onChange={handleInputChange} />
                          </div>
                        </label>
                        <label className="input-label">
                          Address
                          <div className="input-wrapper">
                            <input type="text" name="address" value={formData.address} onChange={handleInputChange} />
                          </div>
                        </label>
                        <label className="input-label">
                          Birth Date
                          <div className="input-wrapper">
                            <input
                              type="date"
                              name="birthDate"
                              value={formattedDate}
                              onChange={handleDateChange}
                            />
                          </div>
                        </label>
                      </div>
                      <div className="col-md-6">
                        <label className="input-label">
                          Profile Picture
                          <div className="input-wrapper">
                            <input type="file" accept="image/*" name="image" onChange={handleImageChange} />
                          </div>
                        </label>
                        <label className="input-label">
                          Email
                          <div className="input-wrapper">
                            <input type="email" name="email" value={formData.email} disabled />
                          </div>
                        </label>
                        <label className="input-label">
                          Password
                          <div className="input-wrapper">
                            <input
                              type="password"
                              name="password"
                              value={formData.password}
                              onChange={handleInputChange}
                            />
                          </div>
                        </label>
                        <label className="input-label">
                          Confirm Password
                          <div className="input-wrapper">
                            <input
                              type="password"
                              name="confirmPassword"
                              value={formData.confirmPassword}
                              onChange={handleInputChange}
                            />
                          </div>
                        </label>
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

export default EditProfile;
