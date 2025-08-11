import React from 'react';
import './styles/Home.css';

const Home = ({ totalMachines, maintenanceLogs, goodMachines, badMachines }) => {
  return (
    <div className="home-wrapper">
      <h1 className="home-heading">Predictive Maintenance Scheduler</h1>
      <div className="dashboard-card">
        <div className="dashboard-item item-machines">
          <h3>Machines</h3>
          <p>{totalMachines}</p>
        </div>
        <div className="dashboard-item item-logs">
          <h3>Maintenance Logs</h3>
          <p>{maintenanceLogs}</p>
        </div>
        <div className="dashboard-item item-good">
          <h3>Good Machines</h3>
          <p>{goodMachines}</p>
        </div>
        <div className="dashboard-item item-bad">
          <h3>Bad Machines</h3>
          <p>{badMachines}</p>
        </div>
      </div>
    </div>
  );
};

export default Home;

