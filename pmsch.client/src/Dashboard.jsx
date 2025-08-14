import React from 'react';
import { Link } from 'react-router-dom';
import { FaUserCircle } from 'react-icons/fa';
import './styles/Dash.css';

function Dashboard() {
  return (
    <div className="dashboard-content">
      <div className="dashboard-top">
        <FaUserCircle className="profile-pic" />
        <h2 className="employee-name">Sanket</h2>
        <p className="employee-personal-info">ADMIN</p>
      </div>

      <div className="dashboard-links">
        <Link to="/home">Home</Link>
        <Link to="/technician-dashboard">Tech</Link>
        <Link to="/add-machine">Add Machine</Link>
        <Link to="/log-maintenance">Log Maintenance</Link>
        <Link to="/health-metrics">Health Metrics</Link>
        <Link to="/show-all-details">Show All Machine Details</Link>
        <Link to="/settings">Settings</Link>
      </div>
    </div>
  );
}

export default Dashboard;
