import React from 'react';
import './styles/AppInfo.css';

function AppInfo() {
  return (
    <div className="info-wrapper">
      <h2>Application Information</h2>

      <div className="info-section">
        <h3>About the Application</h3>
        <p>This Predictive Maintenance Scheduler helps monitor industrial machines, track health metrics, and schedule maintenance proactively to reduce downtime and improve efficiency.</p>
      </div>

      <div className="info-section">
        <h3>Version</h3>
        <p>v1.0.0 – Initial Release</p>
      </div>

      <div className="info-section">
        <h3>Contact Us</h3>
        <p>Email: support@maintenancescheduler.com</p>
        <p>Phone: +91 98765 43210</p>
      </div>

      <div className="info-section">
        <h3>Development Team</h3>
        <ul>
          <li>Chandrakant Dhule</li>
          <li>Priya Sharma</li>
          <li>Ravi Mehta</li>
          <li>Neha Kulkarni</li>
        </ul>
      </div>

      <div className="info-section">
        <h3>Additional Notes</h3>
        <p>This project is a prototype built for demonstration and learning purposes. Future versions may include real-time sensor integration, machine learning models, and cloud-based analytics.</p>
      </div>
    </div>
  );
}

export default AppInfo;
