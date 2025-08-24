import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { FaUserCircle, FaInfoCircle } from 'react-icons/fa';
import './styles/Dash.css';

function AdminDashboard() {
    const user = localStorage.getItem('userUser');
    const role = localStorage.getItem('userRole');
    const navigate = useNavigate();

    const handleLogout = () => {
        localStorage.clear(); // or remove specific keys like localStorage.removeItem('userId');
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
                <Link to="/admin/home">Home</Link>
                <Link to="/admin/add-machine">Add Machine</Link>
                <Link to="/admin/health-metrics">Health Metrics</Link>
                <Link to="/admin/show-all-details">Show All Machine Details</Link>
                <Link to="/admin/show-log-maintain">Log Maintenance</Link>
                <Link to="/admin/settings">Manage Machines</Link>
                <Link to="/admin/create-account">Add User</Link>

                <div className="logout-container">
                    <button onClick={handleLogout} className="logout-button">Log Out</button>
                    <Link to="/admin/app-info" className="info-icon-small" title="App Info">
                        <FaInfoCircle />
                    </Link>
                </div>
            </div>
        </div>
    );
}

export default AdminDashboard;
