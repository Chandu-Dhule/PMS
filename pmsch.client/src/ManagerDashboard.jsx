import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { FaUserCircle, FaInfoCircle } from 'react-icons/fa';
import './styles/Dash.css';

function ManagerDashboard() {
    const user = localStorage.getItem('userUser');
    const role = localStorage.getItem('userRole');
    const navigate = useNavigate();

    const handleLogout = () => {
        localStorage.clear(); // Clear all session data
        navigate('/'); // Redirect to login page
    };

    return (
        <div className="dashboard-content">
            <div className="dashboard-top">
                <FaUserCircle className="profile-pic" />
                <h2 className="employee-name">{user}</h2>
                <p className="employee-personal-info">({role})</p>
            </div>

            <div className="dashboard-links">
                <Link to="/manager/home-manager">Home</Link>
                <Link to="/manager/assign-technician">Assign Technician</Link>
                <Link to="/manager/health-metrics">Health Metrics</Link>
                <Link to="/manager/show-all-details">Show All Machine Details</Link>
                <Link to="/manager/show-log-maintain">Show Log Maintenance</Link>

                <div className="logout-container">
                    <button onClick={handleLogout} className="logout-button">Log Out</button>
                    <Link to="/manager/app-info" className="info-icon-small" title="App Info">
                        <FaInfoCircle />
                    </Link>
                </div>
            </div>
        </div>
    );
}

export default ManagerDashboard;
