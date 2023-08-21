function MyInput({ text, type, name }) {
    return (
      <>
        <div className="input-form">
          <span className="input-text">{text}</span>
          <input className="input-my" type={type} name={name}></input>
        </div>
      </>
    );
  }
  
  export default MyInput;
  