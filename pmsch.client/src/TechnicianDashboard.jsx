/*import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './styles/TechnicianDashboard.css';

import { Routes, Route } from 'react-router-dom';
import './styles/index.css';
import './styles/App.css';
import './styles/LoginPage.css';
import './styles/Deletemachine.css';
import './styles/Editmachine.css';
import './styles/Addmachine.css';
import './styles/HealthMetrics.css';
import './styles/LogMaintenance.css';
import './styles/TechnicianDashboard.css';
import './styles/ManagerDashboard.css';
import './styles/AnalysisPage.css';

import '@fortawesome/fontawesome-free/css/all.min.css';

import AddMachine from './AddMachine';
import EditMachine from './EditMachine';
import LogMaintenance from './LogMaintenance';
import Dashboard from './Dashboard';
import HealthMetrics from './HealthMetrics';
import Settings from './Settings';
import Home from './Home';
import ShowAllDetails from './ShowAllDetails';
import AppInfo from './AppInfo';
import DeleteMachine from './DeleteMachine';

import ManagerDashboard from './ManagerDashboard';
import AnalysisPage from './AnalysisPage';
import CreateAcc from './CreateAcc';
 
const TechnicianDashboard = () => {
  const technicianId = 'tech123'; // Simulated logged-in technician
  const navigate = useNavigate();
 
  // Dummy machine data
  const [machines] = useState([
    { id: 1, name: 'Machine A', categoty_ID: "ABC", type: 'Lathe', date: "xx-xx-xxxx", status: 'Good', assignedTo: 'tech123' },
    { id: 2, name: 'Machine B', categoty_ID: "ABC", type: 'Drill', date: "xx-xx-xxxx", status: 'Needs Maintenance', assignedTo: 'tech456' },
    { id: 3, name: 'Machine C', categoty_ID: "ABC", type: 'CNC', date: "xx-xx-xxxx", status: 'Good', assignedTo: 'tech123' },
  ]);
 
  const assignedMachines = machines.filter(machine => machine.assignedTo === technicianId);
 
  // Navigation handlers
  const goToLogMaintenance = () => {
    navigate('/log-maintenance', {
      state: { technicianId, assignedMachines },
    });
  };
 
  const goToHealthMetrics = () => {
    navigate('/health-metrics', {
      state: { technicianId, assignedMachines },
    });
  };
 
  const goToShowAllDetails = () => {
    navigate('/show-all-details', {
      state: { technicianId, assignedMachines },
    });
  };
 
  const goToAnalysis = () => {
    navigate('/analysis', {
      state: { technicianId, assignedMachines },
    });
  };
 
  return (
    <div className="technician-dashboard">
      <h2>👨‍🔧 Technician Dashboard</h2>
 
      <section>
        <h3>🛠️ Machines Assigned to You</h3>
        {assignedMachines.length === 0 ? (
          <p>No machines assigned.</p>
        ) : (
          <table className="machine-table">
            <thead>
              <tr>
                <th>Name</th>
                <th>Type</th>
                <th>CategoryID</th>
                <th>Date</th>
                <th>Status</th>
              </tr>
            </thead>
            <tbody>
              {assignedMachines.map(machine => (
                <tr key={machine.id}>
                  <td>{machine.name}</td>
                  <td>{machine.type}</td>
                  <td>{machine.categoty_ID}</td>
                  <td>{machine.date}</td>
                  <td>{machine.status}</td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </section>
 
      <div className="dashboard-actions">
        <button onClick={goToLogMaintenance}>📝 Log Maintenance</button>
        <button onClick={goToHealthMetrics}>📊 Health Metrics</button>
        <button onClick={goToShowAllDetails}>📋 Show All Details</button>
        <button onClick={() => navigate('/analysis-page')}>📈 Analysis</button>
      </div>
    </div>
  );
};
 
export default TechnicianDashboard;
*/

import React from 'react';
import { Link } from 'react-router-dom';
import { FaUserCircle, FaInfoCircle } from 'react-icons/fa';
import './styles/Dash.css';

function TechnicianDashboard() {
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
              <Link to="/home-technician">Home</Link>
        {/* <Link to="/technician-dashboard">Technician</Link> */}
        {/* <Link to="/manager-dashboard">Manager</Link> */}
        {/* <Link to="/add-machine">Add Machine</Link> */}
        <Link to="/log-maintenance">Log Maintenance</Link>
        <Link to="/health-metrics">Health Metrics</Link>
        <Link to="/show-all-details">Show All Machine Details</Link>
        <Link to="/show-log-maintain">Show Log Maintenaince</Link>
        {/* <Link to="/">LogOut</Link> */}
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

export default TechnicianDashboard;
 