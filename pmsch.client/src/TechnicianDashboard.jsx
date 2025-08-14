import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './styles/TechnicianDashboard.css';

const TechnicianDashboard = () => {
  const technicianId = 'tech123'; // Simulated logged-in technician
  const navigate = useNavigate();

  // Dummy machine data
  const [machines] = useState([
    { id: 1, name: 'Machine A', categoty_ID: "ABC", type: 'Lathe', date: "xx-xx-xxxx", status: 'Good',  assignedTo: 'tech123' },
    { id: 2, name: 'Machine B', categoty_ID: "ABC",type: 'Drill', date: "xx-xx-xxxx",status: 'Needs Maintenance', assignedTo: 'tech456' },
    { id: 3, name: 'Machine C', categoty_ID: "ABC",type: 'CNC',date: "xx-xx-xxxx", status: 'Good', assignedTo: 'tech123' },
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


  return (
    <div className="technician-dashboard">
      <h2>👨‍🔧Technician Dashboard</h2>

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
        {/* <button onClick={goToSettings}>⚙️ Settings</button> */}
      </div>
    </div>
  );
};

export default TechnicianDashboard;
