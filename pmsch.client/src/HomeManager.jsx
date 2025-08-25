import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './styles/ManagerDashboard.css';
import ManagerDashboard from './ManagerDashboard';

const HomeManager = () => {
    const managerId = 'manager001'; // Simulated logged-in manager
    const navigate = useNavigate();
      const hideSidebar = location.pathname === '/log-out'|| location.pathname === '/login-page'

    // Dummy machine data
    const [machines] = useState([
        { id: 1, name: 'Machine A', category_ID: "ABC", type: 'Lathe', date: "xx-xx-xxxx", status: 'Good', assignedTo: 'tech123' },
        { id: 2, name: 'Machine B', category_ID: "ABC", type: 'Drill', date: "xx-xx-xxxx", status: 'Needs Maintenance', assignedTo: 'tech456' },
        { id: 3, name: 'Machine C', category_ID: "ABC", type: 'CNC', date: "xx-xx-xxxx", status: 'Good', assignedTo: 'tech123' },
    ]);

    const goToHealthMetrics = () => {
        navigate('/health-metrics', {
            state: { managerId, machines },
        });
    };

    const goToShowAllDetails = () => {
        navigate('/show-all-details', {
            state: { managerId, machines },
        });
    };

    const goToAnalysis = () => {
        navigate('/analysis', {
            state: { managerId, machines },
        });
    };

    return (





        <div className="manager-dashboard">

            {/* {!hideSidebar && (
                <aside className="sidebar">
                    <ManagerDashboard />
                </aside>
            )} */}
            {/* <h2>🧑‍💼 Manager Dashboard</h2> */}

            <section>
                <h3>🛠️ All Machines Overview</h3>
                {machines.length === 0 ? (
                    <p>No machines available.</p>
                ) : (
                    <table className="machine-table">
                        <thead>
                            <tr>
                                <th>Name</th>
                                <th>Type</th>
                                <th>CategoryID</th>
                                <th>Date</th>
                                <th>Status</th>
                                <th>Assigned To</th>
                            </tr>
                        </thead>
                        <tbody>
                            {machines.map(machine => (
                                <tr key={machine.id}>
                                    <td>{machine.name}</td>
                                    <td>{machine.type}</td>
                                    <td>{machine.category_ID}</td>
                                    <td>{machine.date}</td>
                                    <td>{machine.status}</td>
                                    <td>{machine.assignedTo}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                )}
            </section>
        </div>
    );
};

export default HomeManager;