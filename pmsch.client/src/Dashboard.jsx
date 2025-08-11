import React from 'react';
import { Link } from 'react-router-dom';

function Dashboard() {
  return (
    <div className="dashboard-content">
      <h1>MENU</h1>
      <div className="stats-box">
        <div><Link to=" ">Home</Link></div> 
        <div><Link to="/add-machine">Add Machine</Link></div>
        <div><Link to="/log-maintenance">Log Maintenance</Link></div>
        <div><Link to="/health-metrics">Health Metrics</Link></div>
        <div><Link to=" ">Show All Machine Details</Link></div>
        <div><Link to="/settings">Settings</Link></div>
      </div>
    </div>
  );
}

export default Dashboard;
