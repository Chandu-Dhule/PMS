/*import { Routes, Route } from 'react-router-dom';
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
import TechnicianDashboard from './TechnicianDashboard';
import ManagerDashboard from './ManagerDashboard';
import AnalysisPage from './AnalysisPage';
import CreateAcc from './CreateAcc';
import LoginPage from './LoginPage';



function AdminDashboard() {
  const totalMachines = 10;
  const maintenanceLogs = 25;
  const goodMachines = 7;
  const badMachines = 3;
  // const hideSidebar = location.pathname === '/create-account'
  const hideSidebar = location.pathname === '/create-account'|| location.pathname === '/login-page'


  return (
    // <div className="app-container">
    //   <aside className="sidebar">
    //     <Dashboard />
    //   </aside>
    <div className="app-container">
      {!hideSidebar && (
        <aside className="sidebar">
          <Dashboard />
        </aside>
      )}

      <main className="main-content">
        <Routes>
          <Route
            path="/home"
            element={
              <Home
                totalMachines={totalMachines}
                maintenanceLogs={maintenanceLogs}
                goodMachines={goodMachines}
                badMachines={badMachines}
              />
            }
          />
          <Route path="/add-machine" element={<AddMachine />} />
          <Route path="/log-maintenance" element={<LogMaintenance />} />
          <Route path="/health-metrics" element={<HealthMetrics />} />
          <Route path="/settings" element={<Settings />} />
          <Route path="/show-all-details" element={<ShowAllDetails />} />
          <Route path="/app-info" element={<AppInfo />} />
          <Route path="/edit-machine" element={<EditMachine />} />
          <Route path="/delete-machine" element={<DeleteMachine />} />
          <Route path="/technician-dashboard" element={<TechnicianDashboard />} />
          <Route path="/manager-dashboard" element={<ManagerDashboard />} />
          <Route path="/analysis-page" element={<AnalysisPage />} />
          <Route path="/create-account" element={<CreateAcc />} />
          <Route path="/login-page" element={<LoginPage />} />
        </Routes>
      </main>
    </div>
  );
}

export default AdminDashboard;
*/
import React from 'react';
import { Link } from 'react-router-dom';
import { FaUserCircle, FaInfoCircle } from 'react-icons/fa';
import './styles/Dash.css';

function AdminDashboard() {
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
        <Link to="/home">Home</Link>
        <Link to="/add-machine">Add Machine</Link>
        <Link to="/health-metrics">Health Metrics</Link>
        <Link to="/show-all-details">Show All Machine Details</Link>
        {/* <Link to="/analysis-page">Analysis</Link> */}
        <Link to="/show-log-maintain">LogMaintenace</Link>
        <Link to="/settings">Manage Machines</Link>
        <Link to="/create-account">Add User</Link>

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

export default AdminDashboard;
