import React from 'react';
import { Style } from '../styles/pagination.css';

function Pagination({ count, currentPage,pageSize, onPageChange }) {
  const totalPages = Math.ceil(count / pageSize); // Pretpostavljamo da je broj stavki po stranici 10

  const handlePageChange = (page) => {
    if (page === currentPage) return; // Ako je kliknuta trenutna stranica, nema potrebe za promenom

    onPageChange(page);
  };

  const renderPageNumbers = () => {
    const pageNumbers = [];

    for (let i = 1; i <= totalPages; i++) {
      pageNumbers.push(
        <button class-name="pagination__button"
          key={i}
          className={i === currentPage ? 'active' : ''}
          onClick={() => handlePageChange(i)}
        >
          {i}
        </button>
      );
    }

    return pageNumbers;
  };

  return (
    <ul className="pagination">
      {renderPageNumbers()}
    </ul>
  );
}

export default Pagination;
