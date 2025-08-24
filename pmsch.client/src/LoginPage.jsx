/*
import React, { useState } from 'react';
import './styles/LoginPage.css';
import { Navigate, useNavigate } from 'react-router-dom';

function Login() {
const [form, setForm] = useState({ username: '', password: '' });
const [error, setError] = useState('');
const [success, setSuccess] = useState('');
const nav = useNavigate();


const handleChange = e => {
  const { name, value } = e.target;
  setForm({ ...form, [name]: value });
};

const handleSubmit = async e => {
  e.preventDefault();
  setError('');
  setSuccess('');

  try {
    const response = await fetch('https://localhost:7090/api/Login', {
      method: 'GET',
      headers: { 'Content-Type': 'application/json' }
    });

    if (!response.ok) {
      throw new Error('Failed to fetch login data');
    }

    const users = await response.json();

    const matchedUser = users.find(
      user => user.user === form.username && user.pass === form.password
    );

    if (matchedUser) {
      setSuccess(`✅ Login successful! Welcome ${matchedUser.role}`);
      localStorage.setItem('userRole', matchedUser.role); 
      localStorage.setItem('userUser', matchedUser.user); 
      nav('/home');

    } else {
      setError('❌ Invalid username or password');
    }
  } catch (err) {
    console.error('Login error:', err);
    setError('❌ Login failed. Please try again.');
  }
};

  return (
    <div className="form-container-add">
      <h2>Login</h2>
<form onSubmit={handleSubmit}>
  <input
    name="username"
    placeholder="Username"
    value={form.username}
    onChange={handleChange}
    required
  />
  <input
    name="password"
    type="password"
    placeholder="Password"
    value={form.password}
    onChange={handleChange}
    required
  />
  <button type="submit">Login</button>
</form>

 <button onClick={() => nav('/create-account')}>Create New Account</button>

    {error && <p style={{ color: 'red' }}>{error}</p>}
    {success && <p style={{ color: 'green' }}>{success}</p>}
    </div>
  );
}

export default Login;
*/






// ----------------------------------------------
// import React from 'react';
// import './styles/LoginPage.css';






// old code 





import React, { useState } from 'react';
import './styles/LoginPage.css';
import { useNavigate } from 'react-router-dom';

const Login = () => {
  const [form, setForm] = useState({ username: '', password: '' });
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [showTerms, setShowTerms] = useState(false);
  const [acceptedTerms, setAcceptedTerms] = useState(false);
  const nav = useNavigate();
  

  const handleChange = e => {
    const { name, value } = e.target;
    setForm({ ...form, [name]: value });
  };

  const handleSubmit = async e => {
    e.preventDefault();
    setError('');
    setSuccess('');
  
    if (!acceptedTerms) {
      setError('❌ Please accept the Terms & Conditions to proceed.');
      return;
    }
  
    try {
      const response = await fetch('https://localhost:7090/api/Login/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          user: form.username,
          pass: form.password,
          role: 'Admin', // or dynamically set if needed
          id:'0'

        })
      });
  
      if (!response.ok) {
        const errorText = await response.text();
        throw new Error(errorText || 'Login failed');
      }
  
      const matchedUser = await response.json();
      setSuccess(`✅ Login successful! Welcome ${matchedUser.role}`);
      localStorage.setItem('userRole', matchedUser.role);
      localStorage.setItem('userUser', matchedUser.user);
      localStorage.setItem('userId', matchedUser.id);
      // localStorage.setItem('userId', matchedUser.id)

      
      // if (matchedUser.role === 'Admin') {
      //   nav('/home');
      // } else if (matchedUser.role === 'Technician') {
      //   nav('/home-technician');
      // } else if (matchedUser.role === 'Manager') {
      //   nav('/home-manager');
      // } else {
      //   setError('❌ Unknown role. Please contact support.');
      // }
      
      if (matchedUser) {
        setSuccess(`✅ Login successful! Welcome ${matchedUser.role}`);
localStorage.setItem('userRole', matchedUser.role);
localStorage.setItem('userUser', matchedUser.user);

if (matchedUser.role === 'Admin') {
  
  // alert(matchedUser.user)
  // alert(matchedUser.Id)
  console.log(matchedUser);
  nav('/admin/home');
} else if (matchedUser.role === 'Technician') {
  // alert(matchedUser.role)
  nav('/technician/home-technician');
} else if (matchedUser.role === 'Manager') {
  nav('/manager/home-manager');
} else {
  

  setError('❌ Unknown role. Please contact support.');
}

      }
       else {
        setError('❌ Invalid username or password');
      }
    } catch (err) {
      console.error('Login error:', err);
      setError('❌ Login failed. Please try again.');
    }
  };
  

  return (
    <div className="login-wrapper">
      <div className="login-left">
        <h2>Machine Minder</h2>
        <p>
          This Predictive Maintenance Scheduler helps monitor industrial machines, track health metrics, and schedule maintenance proactively to reduce downtime and improve efficiency.
        </p>
      </div>
      <div className="login-right">
        <h2>Login</h2>
        <p>Welcome! Login to monitor your machines smartly.</p>
        <form className="login-page-form" onSubmit={handleSubmit}>
          <label htmlFor="username">User Name</label>
          <input
            type="text"
            id="username"
            name="username"
            placeholder="Username"
            value={form.username}
            onChange={handleChange}
            required
          />
          <label htmlFor="password">Password</label>
          <input
            name="password"
            id="password"
            type="password"
            placeholder="Password"
            value={form.password}
            onChange={handleChange}
            required
          />

          <div className="terms-container">
            <input
              type="checkbox"
              id="terms"
              checked={acceptedTerms}
              onChange={() => setAcceptedTerms(!acceptedTerms)}
            />
            <label htmlFor="terms">
              I accept the{' '}
              <span className="terms-link" onClick={() => setShowTerms(true)}>
                Terms & Conditions
              </span>
            </label>
          </div>

          <button type="submit">Login</button>
          {error && <p style={{ color: 'red' }}>{error}</p>}
          {success && <p style={{ color: 'green' }}>{success}</p>}
        </form>

        {showTerms && (
          <div className="terms-popup">
            <div className="terms-content">
              <h3>Terms & Conditions</h3>
              <ul>
                <li>1. Users must keep login credentials confidential.</li>
                <li>2. Unauthorized access or misuse is strictly prohibited.</li>
                <li>3. All activities are monitored for security and compliance.</li>
                <li>4. Data entered must be accurate and truthful.</li>
                <li>5. Users are responsible for actions taken under their account.</li>
                <li>6. iolation may result in suspension or termination of access.</li>
              </ul>
              <button onClick={() => setShowTerms(false)}>Close</button>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default Login;
