/*import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './styles/ManagerDashboard.css';
 
const ManagerDashboard = () => {
  const managerId = 'manager001'; // Simulated logged-in manager
  const navigate = useNavigate();
 
  // Dummy machine data
  const [machines] = useState([
    { id: 1, name: 'Machine A', category_ID: "ABC", type: 'Lathe', date: "xx-xx-xxxx", status: 'Good', assignedTo: 'tech123' },
    { id: 2, name: 'Machine B', category_ID: "ABC", type: 'Drill', date: "xx-xx-xxxx", status: 'Needs Maintenance', assignedTo: 'tech456' },
    { id: 3, name: 'Machine C', category_ID: "ABC", type: 'CNC', date: "xx-xx-xxxx", status: 'Good', assignedTo: 'tech123' },
  ]);
 
  const goToHealthMetrics = () => {
    navigate('/health-metrics', {
      state: { managerId, machines },
    });
  };
 
  const goToShowAllDetails = () => {
    navigate('/show-all-details', {
      state: { managerId, machines },
    });
  };
 
  const goToAnalysis = () => {
    navigate('/analysis', {
      state: { managerId, machines },
    });
  };
 
  return (
    <div className="manager-dashboard">
      <h2>🧑‍💼 Manager Dashboard</h2>
 
      <section>
        <h3>🛠️ All Machines Overview</h3>
        {machines.length === 0 ? (
          <p>No machines available.</p>
        ) : (
          <table className="machine-table">
            <thead>
              <tr>
                <th>Name</th>
                <th>Type</th>
                <th>CategoryID</th>
                <th>Date</th>
                <th>Status</th>
                <th>Assigned To</th>
              </tr>
            </thead>
            <tbody>
              {machines.map(machine => (
                <tr key={machine.id}>
                  <td>{machine.name}</td>
                  <td>{machine.type}</td>
                  <td>{machine.category_ID}</td>
                  <td>{machine.date}</td>
                  <td>{machine.status}</td>
                  <td>{machine.assignedTo}</td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </section>
 
      <div className="dashboard-actions">
        <button onClick={goToHealthMetrics}>📊 Health Metrics</button>
        <button onClick={goToShowAllDetails}>📋 Show All Details</button>
        <button onClick={() => navigate('/analysis-page')}>📈 Analysis</button>
      </div>
    </div>
  );
};
 
export default ManagerDashboard;
*/

import React from 'react';
import { Link } from 'react-router-dom';
import { FaUserCircle, FaInfoCircle } from 'react-icons/fa';
import './styles/Dash.css';

function ManagerDashboard() {
  const user = localStorage.getItem('userUser');
  const role = localStorage.getItem('userRole');
  return (
    <div className="dashboard-content">
      <div className="dashboard-top">
        <FaUserCircle className="profile-pic" />
        <h2 className="employee-name">{user}</h2>
        <p className="employee-personal-info">({role})</p>
      </div>

      <div className="dashboard-links">
        <Link to="/home-manager">Home</Link>
        <Link to="/assign-technician">Assign Technician</Link>
        {/* <Link to="/home">Home</Link> */}
        {/* <Link to="/technician-dashboard">Technician</Link> */}
        {/* <Link to="/manager-dashboard">Manager</Link> */}
        {/* <Link to="/add-machine">Add Machine</Link> */}
        {/* <Link to="/log-maintenance">Log Maintenance</Link> */}
        <Link to="/health-metrics">Health Metrics</Link>
              <Link to="/show-all-details">Show All Machine Details</Link>
              <Link to="/show-log-maintain">Show Log Maintenaince</Link>
        {/* <Link to="/analysis-page">Analysis</Link> */}
        {/* <Link to="/">LogOut</Link> */}
        {/* <Link to="/settings">Show All Machine Details</Link>

        {/* <Link to="/settings">Settings</Link> */}
        <div className="logout-container">
                  <Link to="/">LogOut</Link>
                  <Link to="/app-info" className="info-icon-small" title="App Info">
                    <FaInfoCircle />
                  </Link>
                </div>
      </div>
    </div>
  );
}

export default ManagerDashboard;
 