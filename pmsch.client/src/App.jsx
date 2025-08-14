App.jsx
import { Routes, Route, useLocation } from 'react-router-dom';
import './styles/index.css';
import './styles/App.css';
import './styles/LoginPage.css';
import '@fortawesome/fontawesome-free/css/all.min.css';
 import TechnicianDashboard from './TechnicianDashboard';

import AddMachine from './AddMachine';
import LogMaintenance from './LogMaintenance';
import Dashboard from "./Dashboard";
import HealthMetrics from "./HealthMetrics";
import Settings from "./Settings";
import Home from './Home';
import ShowAllDetails from './ShowAllDetails';
import AppInfo from './AppInfo';
import LoginPage from './LoginPage';
 
function App() {
  const location = useLocation();
 
  // Dummy data
  const totalMachines = 10;
  const maintenanceLogs = 25;
  const goodMachines = 7;
  const badMachines = 3;
 
  const isLoginPage = location.pathname === '/';
 
  return (
    <div className="app-container">
      {!isLoginPage && (
        <aside className="sidebar">
          <Dashboard />
        </aside>
      )}
 
      <main className="main-content">
        <Routes>
          <Route path="/" element={<LoginPage />} />
          <Route
            path="/home"
            element={
              <Home
                totalMachines={totalMachines}
                maintenanceLogs={maintenanceLogs}
                goodMachines={goodMachines}
                badMachines={badMachines}
              />
            }
          />
          <Route path="/add-machine" element={<AddMachine />} />
          <Route path="/technician-dashboard" element={<TechnicianDashboard />} />
          <Route path="/log-maintenance" element={<LogMaintenance />} />
          <Route path="/health-metrics" element={<HealthMetrics />} />
          <Route path="/settings" element={<Settings />} />
          <Route path="/show-all-details" element={<ShowAllDetails />} />
          <Route path="/app-info" element={<AppInfo />} />
        </Routes>
      </main>
    </div>
  );
}
 
export default App;
 
 