import React, { useEffect, useState } from 'react';
import './styles/ShowAllDetails.css';

function ShowAllDetails() {
  const [machineData, setMachineData] = useState([]);
  const [loading, setLoading] = useState(true);
  const id = localStorage.getItem('userId');
  const role = localStorage.getItem('userRole');
  // alert(id)
  // alert(role)
  // const id = localStorage.getItem('userId');
  // const role = localStorage.getItem('userRole');
  // alert(id)
  // alert(role)

  useEffect(() => {
    // fetch('https://localhost:7090/api/Machines')
    fetch(`https://localhost:7090/api/Machines/machines/by-role?role=${role}&userId=${id}`)

      .then(response => response.json())
      .then(data => {
        console.log('fetcxhed data:', data);
        setMachineData(data);
        setLoading(false);
      })
      .catch(error => {
        console.error('Error fetching machine data:', error);
        setLoading(false);
      });
  }, []);
  
  return (
    <div className="details-wrapper">
      <h2>All Machine Details</h2>
      {loading ? (
        <p>Loading...</p>
          ) : (
                  <div className="table-scroll-container">
                  <table className="details-table">
        
          <thead>
                              <tr>
                                  <th id="name1">Name</th>
              <th>Machine ID</th>
              {/* <th>Name</th> */}
              <th>Category ID</th>
              
              <th>Type ID</th>
              
              <th>Installation Date</th>
              <th>Status</th>
             <th>Temperature</th>
              <th>Energy Consumptions</th>
              <th>Assigned To</th>
              <th>LifeCycle(Month)</th>
            </tr>
                          </thead>
       
          <tbody>
            {machineData.map((m, idx) => (
              <tr key={idx}>
                <td>{m.name}</td>
                <td>{m.machineID}</td>
                {/* <td>{m.name}</td> */}
                <td>{m.categoryID}</td>
                
                <td>{m.typeID}</td>
                
                <td>{new Date(m.installationDate).toLocaleDateString()}</td>
                <td className={`status-${m.status.toLowerCase()}`}>{m.status}</td>
                    <td>{m.temperature}</td>
                    <td>{m.energyConsumption}</td>
                    <td>{m.assignedTo}</td>
                <td>{m.lifeCycle}</td>
                
              </tr>
            ))}
          </tbody>
                      </table>
      </div>
      )}
    </div>
  );
}

export default ShowAllDetails;
