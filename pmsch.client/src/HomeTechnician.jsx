import React, { useState } from 'react';
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
 

const HomeTechnician = () => {
  const technicianId = 'tech123';
  const navigate = useNavigate();

  const [machines] = useState([
    { id: 1, name: 'Machine A', categoty_ID: "ABC", type: 'Lathe', date: "xx-xx-xxxx", status: 'Good', assignedTo: 'tech123' },
    { id: 2, name: 'Machine B', categoty_ID: "ABC", type: 'Drill', date: "xx-xx-xxxx", status: 'Needs Maintenance', assignedTo: 'tech456' },
    { id: 3, name: 'Machine C', categoty_ID: "ABC", type: 'CNC', date: "xx-xx-xxxx", status: 'Critical', assignedTo: 'tech123' },
  ]);

  const assignedMachines = machines.filter(machine => machine.assignedTo === technicianId);

  const goToNotifications = () => {
    const criticalMachines = assignedMachines.filter(machine => machine.status === 'Critical');
    navigate('/notifications', {
      state: { technicianId, criticalMachines },
    });
  };

  return (
    <div className="technician-dashboard">
    <div className="dashboard-header">
  <div className="header-title">
    <div>
      <h3>🛠️ Machines Assigned to You</h3>
    </div>
    <div>
    <button
      className="notification-bell"
      onClick={() => navigate('/notifications', {
        state: {
          technicianId,
          criticalMachines: assignedMachines.filter(machine => machine.status === 'Critical')
        }
      })}
      title="View Critical Notifications"
    >
      <i className="fas fa-bell"></i>
    </button>
    </div>
  </div>
</div>


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
    </div>
  );
};

export default HomeTechnician;
