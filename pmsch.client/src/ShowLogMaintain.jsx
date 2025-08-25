import React, { useEffect, useState } from 'react';
import './styles/ShowAllDetails.css';

function ShowLogMaintain() {
  const [logData, setLogData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const id = localStorage.getItem('userId');
  const role = localStorage.getItem('userRole');

  useEffect(() => {
    fetch(`https://localhost:7090/api/MaintenanceLogs/logs/by-role?role=${role}&userId=${id}`)
      .then(response => {
        if (!response.ok) {
          throw new Error(`HTTP error! status: ${response.status}`);
        }
        return response.json();
      })
      .then(data => {
        console.log('Fetched log data:', data);
        setLogData(data);
        setLoading(false);
      })
      .catch(error => {
        console.error('Error fetching log maintenance data:', error);
        setError('Failed to load log data.');
        setLoading(false);
      });
  }, []);

  return (
    <div className="details-wrapper">
      <h2>Log Maintenance Details</h2>

      {error && <p style={{ color: 'red' }}>{error}</p>}

      {loading ? (
        <p>Loading...</p>
          ) : (
          <div className="table-scroll-container">
        <table className="details-table">
          <thead>
            <tr>
              <th>Log ID</th>
              <th>Machine ID</th>
              <th>Maintenance Date</th>
              <th>Description</th>
              <th>Operator Name</th>
              <th>Next Due Date</th>
            </tr>
          </thead>
          <tbody>
            {logData.map((log, idx) => (
              <tr key={idx}>
                <td>{log.logID}</td>
                <td>{log.machineID}</td>
                <td>{new Date(log.maintenanceDate).toLocaleDateString()}</td>
                <td>{log.description}</td>
                <td>{log.operatorName}</td>
                <td>{new Date(log.nextDueDate).toLocaleDateString()}</td>
              </tr>
            ))}
          </tbody>
                      </table>
        </div>
      )}
    </div>
  );
}

export default ShowLogMaintain;
