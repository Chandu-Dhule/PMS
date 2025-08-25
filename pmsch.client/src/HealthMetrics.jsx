// import React from 'react';
import React, { useEffect, useState } from 'react';

function HealthMetrics() {
    const [mockMetrics, setHealth] = useState([]);
      const [loading, setLoading] = useState(true);
      const id = localStorage.getItem('userId');
      const role = localStorage.getItem('userRole');
      useEffect(() => {
        fetch(`https://localhost:7090/api/HealthMetrics/metrics/by-role?role=${role}&userId=${id}`)
          .then(response => response.json())
          .then(data => {
            console.log('fetcxhed data:', data);
            setHealth(data);
            setLoading(false);
          })
          .catch(error => {
            console.error('Error fetching machine data:', error);
            setLoading(false);
          });
      }, []);


  // const mockMetrics = [
  //   { machineId: 1, temp: 70, vibration: 0.3, pressure: 2.1, status: "Good" },
  //   { machineId: 1, temp: 70, vibration: 0.3, pressure: 2.1, status: "Good" },
  //   { machineId: 1, temp: 70, vibration: 0.3, pressure: 2.1, status: "Good" },
  //   { machineId: 1, temp: 70, vibration: 0.3, pressure: 2.1, status: "Good" },
  //   { machineId: 1, temp: 70, vibration: 0.3, pressure: 2.1, status: "Good" },
  //   { machineId: 2, temp: 85, vibration: 0.6, pressure: 3.0, status: "Alert" }
  // ];

  return (
    <div className="main1-content">
      <h2>Health Metrics</h2>

      {loading ? (
        <p>Loading...</p>
      ) : (
        mockMetrics.map((m, idx) => (
          <div key={idx} className="metric-card">
            <p><strong>Machine ID:</strong> {m.machineID}</p>
            <p>Temperature: {m.temperature} °C</p>
            <p>Energy Consumption: {m.energyConsumption}</p>
                <p>CheckDate: {new Date(m.checkDate).toLocaleDateString()}</p> 
            <p>Health Status: {m.healthStatus}</p>
          </div>
        ))
      )}


      {/* {mockMetrics.map((m, idx) => (
        <div key={idx} className="metric-card">
          <p><strong>Machine ID:</strong> {m.machineId}</p>
          <p>Temperature: {m.temp} °C</p>
          <p>Vibration: {m.vibration}</p>
          <p>Pressure: {m.pressure} Bar</p>
          <p>Status: {m.status}</p>
        </div>
      ))} */}
    </div>
  );
}

export default HealthMetrics;
