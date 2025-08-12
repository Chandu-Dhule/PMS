// File: LoginPage.jsx
import React, { useState } from 'react';
import './LoginPage.css'; // Optional: for styling

const LoginPage = () => {
  const [role, setRole] = useState('');
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');

  const handleLogin = (e) => {
    e.preventDefault();
    // Replace with actual backend API call
    console.log(`Role: ${role}, Username: ${username}, Password: ${password}`);
    alert(`Logging in as ${role}`);
  };

  return (
    <div className="login-container">
      <h2>Login</h2>
      <form onSubmit={handleLogin}>
        <select value={role} onChange={(e) => setRole(e.target.value)} required>
          <option value="">Select Role</option>
          <option value="admin">Admin</option>
          <option value="technician">Technician</option>
          <option value="manager">Manager</option>
        </select>
        <input
          type="text"
          placeholder="Username"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          required
        />
        <input
          type="password"
          placeholder="Password"
          value={password}
                   required
        />
        <button type="submit">Login</button>
      </form>
    </div>
  );
};

export default LoginPage;
