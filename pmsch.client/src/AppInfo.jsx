import React from 'react';
import './styles/AppInfo.css';

function AppInfo() {
  return (
    <div className="info-wrapper">
      <h2>Application Information</h2>

      <p><strong>About the Application:</strong> Machine Minder is a Predictive Maintenance Scheduler designed to monitor industrial machines, track health metrics, and schedule maintenance proactively. It helps reduce downtime, optimize performance, and extend machine lifespan.</p>

      <p><strong>Key Features:</strong></p>
      <ul>
        <li>Real-time machine health monitoring</li>
        <li>Automated maintenance scheduling</li>
        <li>Role-based dashboards for Admin, Manager, and Technician</li>
        <li>Data visualization and analysis tools</li>
        <li>User management and access control</li>
      </ul>

      <p><strong>Version:</strong> v1.0.0 – Initial Release</p>

      <p><strong>Technology Stack:</strong></p>
      <ul>
        <li>Frontend: React.js</li>
        <li>Backend: ASP.NET Core Web API</li>
        <li>Database: SQL Server</li>
        <li>Styling: CSS3 with responsive design</li>
      </ul>

      <p><strong>Contact Us:</strong><br />
        Email: support@maintenancescheduler.com<br />
        Phone: +91 98765 XXXXX
      </p>

      <p><strong>Development Team:</strong><br />
        Chandrakant Dhule<br />
        Utkarsh Gupta<br />
        Sanket Ghurghure<br />
        Vaibhav Dhote
      </p>

      <p><strong>Future Enhancements:</strong></p>
      <ul>
        <li>Integration with IoT sensors for live data</li>
        <li>Machine learning models for predictive analytics</li>
        <li>Cloud-based deployment and scalability</li>
        <li>Mobile app support for technicians</li>
      </ul>

      <p><strong>Disclaimer:</strong> This project is a prototype built for academic and demonstration purposes. It may not reflect production-grade security or scalability features.</p>
    </div>
  );
}

export default AppInfo;
