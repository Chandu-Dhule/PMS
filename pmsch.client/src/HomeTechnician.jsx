/*import React, { useState } from 'react';
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
*/



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

function HomeTechnician() {
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
                                {/*<th>Assigned To</th>*/}
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
                                    {/*<td>{m.assignedTo}</td>*/}
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

export default HomeTechnician;
