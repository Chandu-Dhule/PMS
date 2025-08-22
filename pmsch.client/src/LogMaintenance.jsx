import React, { useState } from 'react';

function LogMaintenance() {
  const [logs, setLogs] = useState([]);
  const [logData, setLogData] = useState({
    logId: '',               // ✅ New field
    machineId: '',
    date: '',
    operator: '',
    description: '',
    temperature: '',
    // lifeCycle: '',
    energyConsumption: '',
    healthStatus: '',        // Optional: if you want to track health status
    nextDueDate: ''          // ✅ New field
  });

  const handleChange = e =>
    setLogData({ ...logData, [e.target.name]: e.target.value });

  const handleSubmit = e => {
    e.preventDefault();
    setLogs([...logs, logData]);
    alert('✅ Log added!');
    setLogData({
      logId: '',
      machineId: '',
      date: '',
      operator: '',
      description: '',
      temperature: '',
      // lifeCycle: '',
      energyConsumption: '',
      healthStatus: '',
      nextDueDate: ''
    });
  };

  return (
    <div className="form-container-log">
      <h2>Log Maintenance</h2>
      <form onSubmit={handleSubmit}>
        <input
          name="logId"
          placeholder="Log ID"
          value={logData.logId}
          onChange={handleChange}
          required
        />
        <input
          name="machineId"
          placeholder="Machine ID"
          value={logData.machineId}
          onChange={handleChange}
          required
        />
        <input
          name="date"
          type="date"
          value={logData.date}
          onChange={handleChange}
          required
        />
        <input
          name="operator"
          placeholder="Operator Name"
          value={logData.operator}
          onChange={handleChange}
          required
        />
        <textarea
          name="description"
          placeholder="Description"
          value={logData.description}
          onChange={handleChange}
          required
        />
        <input
          name="temperature"
          type="number"
          step="0.1"
          placeholder="Temperature (°C)"
          value={logData.temperature}
          onChange={handleChange}
          required
        />
       
        <input
          name="energyConsumption"
          type="number"
          step="0.1"
          placeholder="Energy Consumption (kWh)"
          value={logData.energyConsumption}
          onChange={handleChange}
          required
        />
        <input
          name="healthStatus"
          placeholder="Health Status"
          value={logData.healthStatus}
          onChange={handleChange}
        />
        <label htmlFor="nextDueDate">Next Due_Date : </label>
        <input
          name="nextDueDate"
          type="date"
          value={logData.nextDueDate}
          onChange={handleChange}
          required
        />

        <button type="submit">Log</button>
      </form>
    </div>
  );
}

export default LogMaintenance;

