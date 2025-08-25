import React, { useEffect, useState } from 'react';
import './styles/Notification.css';

const Notification = () => {
  const [criticalMachines, setCriticalMachines] = useState([]);
  const [loading, setLoading] = useState(true);
  const id = localStorage.getItem('userId');
  const role = localStorage.getItem('userRole');
  useEffect(() => {
      fetch(`https://localhost:7090/api/HealthMetrics/critical-metrics?Role=${role}&userId=${id}`) // Adjust this endpoint to match your backend route
      .then(response => {
        if (!response.ok) {
          throw new Error('Failed to fetch critical machines');
        }
        return response.json();
      })
      .then(data => {
        const filtered = data.filter(machine => machine.healthStatus === 'Critical');
        setCriticalMachines(filtered);
        setLoading(false);
      })
      .catch(error => {
        console.error('Error fetching data:', error);
        setLoading(false);
      });
  }, []);

  return (
    <div className="notification-page">
          <h2>🔔 Critical Machine Alerts</h2>
      {loading ? (
        <p>Loading...</p>
      ) : criticalMachines.length === 0 ? (
        <p>No critical alerts at the moment.</p>
      ) : (
        <div className="notification-cards">
          {criticalMachines.map(machine => (
            <div className="notification-card" key={machine.metricID}>
              <h3><strong>Machine ID : </strong>{machine.machineID}</h3>
              <p><strong>Temperature : </strong> {machine.temperature}°F</p>
              <p><strong>Energy      : </strong> {machine.energyConsumption} kWh</p>
              <p><strong>Status      : </strong> {machine.healthStatus}</p>
              <p><strong>Date        :</strong> {new Date(machine.checkDate).toLocaleDateString()}</p>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default Notification;
