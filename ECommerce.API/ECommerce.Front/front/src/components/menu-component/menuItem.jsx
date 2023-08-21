import { Link, useNavigate } from "react-router-dom";
import styles from "../../styles/menu.css";
import userServices from "../../services/userServices";

function MenuItem({ item, path, role }) {
  const navigate = useNavigate();
  const currentUser = userServices.getCurrentUser();
  console.log(currentUser,'currentUser')
  const handleClick = (e) => {
    e.preventDefault();
    navigate("../" + path);
  };

  if (currentUser == null || !role.includes(currentUser.role)) {
    return (
      <>
      </>
    );
  }

  return (
    <>
      <div className="item-wrapper" onClick={handleClick}>
        <span className="item">{item}</span>
      </div>
    </>
  );
}

export default MenuItem;
