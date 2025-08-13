// dASHBOARD.CSS
import React from 'react';
import { Link } from 'react-router-dom';
import { FaUserCircle, FaPhone, FaEnvelope } from 'react-icons/fa';
import './styles/Dash.css';
 
function Dashboard() {
  return (
    <div className="dashboard-content">
      <div className="stats-box">
        <FaUserCircle className="profile-pic" />
        <h2 className="employee-name">ABCXYZ</h2>
        <p className="employee-personal-info">ADMIN</p>
        <div><Link to="/home">Home</Link></div>
        {/* <div><Link to="/login">Login</Link></div> */}
        <div><Link to="/add-machine">Add Machine</Link></div>
        <div><Link to="/log-maintenance">Log Maintenance</Link></div>
        <div><Link to="/health-metrics">Health Metrics</Link></div>
        <div><Link to="/show-all-details">Show All Machine Details</Link></div>
        <div><Link to="/settings">Settings</Link></div>
      </div>
    </div>
  );
}
 
export default Dashboard;
 
 