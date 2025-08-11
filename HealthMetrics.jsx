import React from 'react';

function HealthMetrics() {
  const mockMetrics = [
    { machineId: 1, temp: 70, vibration: 0.3, pressure: 2.1, status: "Good" },
    { machineId: 1, temp: 70, vibration: 0.3, pressure: 2.1, status: "Good" },
    { machineId: 1, temp: 70, vibration: 0.3, pressure: 2.1, status: "Good" },
    { machineId: 1, temp: 70, vibration: 0.3, pressure: 2.1, status: "Good" },
    { machineId: 1, temp: 70, vibration: 0.3, pressure: 2.1, status: "Good" },
    { machineId: 1, temp: 70, vibration: 0.3, pressure: 2.1, status: "Good" },
    { machineId: 1, temp: 70, vibration: 0.3, pressure: 2.1, status: "Good" },
    { machineId: 1, temp: 70, vibration: 0.3, pressure: 2.1, status: "Good" },
    { machineId: 1, temp: 70, vibration: 0.3, pressure: 2.1, status: "Good" },
    { machineId: 2, temp: 85, vibration: 0.6, pressure: 3.0, status: "Alert" }
  ];

  return (
    <div className="main1-content">
      <h2>Health Metrics</h2>
      {mockMetrics.map((m, idx) => (
        <div key={idx} className="metric-card">
          <p><strong>Machine ID:</strong> {m.machineId}</p>
          <p>Temperature: {m.temp} °C</p>
          <p>Vibration: {m.vibration}</p>
          <p>Pressure: {m.pressure} Bar</p>
          <p>Status: {m.status}</p>
        </div>
      ))}
    </div>
  );
}

export default HealthMetrics;
