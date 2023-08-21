import React, { useState, useEffect,useRef } from 'react';
import styles from "../../styles/slideshow.css";


const Slideshow = ({ images, interval }) => {
    const [currentSlide, setCurrentSlide] = useState(0);
    const timerRef = useRef();
  
    useEffect(() => {
      const goToNextSlide = () => {
        setCurrentSlide((prevSlide) => (prevSlide + 1) % images.length);
      };
  
      timerRef.current = setInterval(goToNextSlide, interval);
  
      return () => {
        clearInterval(timerRef.current);
      };
    }, [images.length, interval]);
  
    // const goToSlide = (slideIndex) => {
    //   setCurrentSlide(slideIndex);
    // };
  
    const goToNextSlide = () => {
        console.log("hahahhahahahhahaha")
      setCurrentSlide((prevSlide) => (prevSlide + 1) % images.length);
    };
  
    const goToPreviousSlide = () => {
      setCurrentSlide((prevSlide) => (prevSlide - 1 + images.length) % images.length);
    };
  
    // const handleManualSlide = (slideIndex) => {
    //   goToSlide(slideIndex);
    //   clearInterval(timerRef.current);
    //   timerRef.current = setInterval(goToNextSlide, interval);
    // };
  
    return (
      <div className="slideshow">
        <img src={images[currentSlide]} alt="Slideshow" />
        <button className="prev-button" onClick={goToPreviousSlide}>
          {'<'}
        </button>
        <button className="next-button" onClick={goToNextSlide}>
          {'>'}
        </button>
        {/* <div className="manual-controls">
          {images.map((_, index) => (
            <button
              key={index}
              className={currentSlide === index ? 'active' : ''}
              onClick={() => handleManualSlide(index)}
            >
              {index + 1}
            </button>
          ))}
        </div> */}
      </div>
    );
  };
  
  export default Slideshow;