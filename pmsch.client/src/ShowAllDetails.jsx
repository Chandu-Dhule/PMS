// import React from 'react';
// import './styles/ShowAllDetails.css';

// function ShowAllDetails() {
//   const machineData = [
//     { machineId: 101, typeId: 'T01', categoryId: 'C01', operator: 'Alice', lastMaintenance: '2023-08-15', healthStatus: 'Good' },
//     { machineId: 102, typeId: 'T02', categoryId: 'C02', operator: 'Bob', lastMaintenance: '2023-07-10', healthStatus: 'Alert' },
//     { machineId: 103, typeId: 'T03', categoryId: 'C01', operator: 'Charlie', lastMaintenance: '2023-09-01', healthStatus: 'Critical' },
//     { machineId: 104, typeId: 'T01', categoryId: 'C03', operator: 'Diana', lastMaintenance: '2023-06-20', healthStatus: 'Good' },
//   ];

//   return (
//     <div className="details-wrapper">
//       <h2>All Machine Details</h2>
//       <table className="details-table">
//         <thead>
//           <tr>
//             <th>Machine ID</th>
//             <th>Type ID</th>
//             <th>Category ID</th>
//             <th>Operator Name</th>
//             <th>Last Maintenance</th>
//             <th>Health Status</th>
//           </tr>
//         </thead>
//         <tbody>
//           {machineData.map((m, idx) => (
//             <tr key={idx}>
//               <td>{m.machineId}</td>
//               <td>{m.typeId}</td>
//               <td>{m.categoryId}</td>
//               <td>{m.operator}</td>
//               <td>{m.lastMaintenance}</td>
//               <td className={`status-${m.healthStatus.toLowerCase()}`}>{m.healthStatus}</td>
//             </tr>
//           ))}
//         </tbody>
//       </table>
//     </div>
//   );
// }

// export default ShowAllDetails;



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
        <table className="details-table">
          <thead>
            <tr>
            <th>Name</th>
              <th>Machine ID</th>
              {/* <th>Name</th> */}
              <th>Category ID</th>
              
              <th>Type ID</th>
              
              <th>Installation Date</th>
              <th>Status</th>
              <th>Temperature</th>
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
                <td>{m.lifeCycle}</td>
                
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}

export default ShowAllDetails;
