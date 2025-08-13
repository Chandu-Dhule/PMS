// LOGINPAGE.JSX
import React, { useState } from 'react';
import './styles/LoginPage.css';
import { Navigate, useNavigate } from 'react-router-dom';
 
const LoginPage = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const nav = useNavigate();
 
 
  const handleLogin = (e) => {
    // e.preventDefault();
    // console.log(`Username: ${username}, Password: ${password}`);
    // alert(`Logging in as ${username}`);
 
    if(username === 'admin' && password === 'admin'){
      nav('/home');
    }
    else{
      alert('Invalid Credentials');
    }
 
  };
 
  return (
    <div className="login-container">
      <h2>Login</h2>
      <form onSubmit={handleLogin}>
        <div>
        <input
          type="text"
          placeholder="Username"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          required
          className="login-input"
        />
        </div>
        <div>
        <input
          type="password"
          placeholder="Password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
          className="login-input"
        />
        </div>
        <button type="submit" className="login-button" onClick={handleLogin}>Login</button>
      </form>
    </div>
  );
};
 
export default LoginPage;
 
 
 