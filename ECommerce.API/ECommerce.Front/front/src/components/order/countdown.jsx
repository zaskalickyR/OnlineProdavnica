import React, { useState, useEffect } from 'react';

function CountdownTimerShipping({ order }) {
  const [timeRemaining, setTimeRemaining] = useState(0);

  useEffect(() => {
    if (order && order.shippingTime) {
      const shippingDate = new Date(order.shippingTime);
      const now = new Date();
      const difference = shippingDate - now;

      if (difference > 0) {
        setTimeRemaining(difference);
        const interval = setInterval(() => {
          const updatedDifference = shippingDate - new Date();
          if (updatedDifference <= 0) {
            clearInterval(interval);
            setTimeRemaining(0);
          } else {
            setTimeRemaining(updatedDifference);
          }
        }, 1000);

        return () => clearInterval(interval);
      }
    }
  }, [order]);

  const days = Math.floor(timeRemaining / (1000 * 60 * 60 * 24));
  const hours = Math.floor((timeRemaining % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
  const minutes = Math.floor((timeRemaining % (1000 * 60 * 60)) / (1000 * 60));
  const seconds = Math.floor((timeRemaining % (1000 * 60)) / 1000);

  return (
      timeRemaining > 0 ? (
        <div>
          <b>Time until shipping</b>
          <p>{days}d {hours}h {minutes}m {seconds}s</p>
        </div>
      ) : (
        <p>Shipping has already occurred.</p>
      )
  );
}

export default CountdownTimerShipping;
