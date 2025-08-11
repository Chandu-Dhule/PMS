import React, { useState } from 'react';

function LogMaintenance() {
  const [logs, setLogs] = useState([]);
  const [logData, setLogData] = useState({
    machineId: '', date: '', operator: '', description: ''
  });

  const handleChange = e => setLogData({ ...logData, [e.target.name]: e.target.value });

  const handleSubmit = e => {
    e.preventDefault();
    setLogs([...logs, logData]);
    alert("Log added!");
  };

  return (
    <div className="form-container">
      <h2>Log Maintenance</h2>
      <form onSubmit={handleSubmit}>
        <input name="machineId" placeholder="Machine ID" onChange={handleChange} />
        <input name="date" type="date" onChange={handleChange} />
        <input name="operator" placeholder="Operator Name" onChange={handleChange} />
        <textarea name="description" placeholder="Description" onChange={handleChange} />
        <button type="submit">Log</button>
      </form>
    </div>
  );
}

export default LogMaintenance;
