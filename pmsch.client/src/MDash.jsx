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
import TechnicianDashboard from './TechnicianDashboard';
import ManagerDashboard from './ManagerDashboard';
import AnalysisPage from './AnalysisPage';
import CreateAcc from './CreateAcc';
import LoginPage from './LoginPage';
import AdminDashboard from './AdminDashboard';
import HomeManager from './HomeManager';
import AssignTechnician from './AssignTechnician';
import Notification from './Notification';
import ShowLogMaintain from './ShowLogMaintain';





function MDash() {
  const totalMachines = 10;
  const maintenanceLogs = 25;
  const goodMachines = 7;
  const badMachines = 3;
  // const hideSidebar = location.pathname === '/create-account'
  const hideSidebar = location.pathname === '/log-out' || location.pathname === '/login-page'



  return (

    <div className="app-container">
      {!hideSidebar && (
        <aside className="sidebar">
          <ManagerDashboard />
        </aside>
      )}



      <main className="main-content">
        <Routes>
          {/* <Route
            path="/home"
            element={
              <Home
                totalMachines={totalMachines}
                maintenanceLogs={maintenanceLogs}
                goodMachines={goodMachines}
                badMachines={badMachines}
              />
            }
          /> */}
          <Route path="/home-manager" element={<AnalysisPage />} />
          {/* <Route path="/home-manager" element={<HomeManager />} /> */}
          <Route path="/add-machine" element={<AddMachine />} />
          <Route path="/log-maintenance" element={<LogMaintenance />} />
          <Route path="/health-metrics" element={<HealthMetrics />} />
          <Route path="/settings" element={<Settings />} />
          <Route path="/show-all-details" element={<ShowAllDetails />} />
          <Route path="/app-info" element={<AppInfo />} />
          <Route path="/edit-machine" element={<EditMachine />} />
          <Route path="/delete-machine" element={<DeleteMachine />} />
          <Route path="/technician-dashboard" element={<TechnicianDashboard />} />
          <Route path="/manager-dashboard" element={<ManagerDashboard />} />
          <Route path="/analysis-page" element={<AnalysisPage />} />
          <Route path="/create-account" element={<CreateAcc />} />
          <Route path="/login-page" element={<LoginPage />} />
          <Route path="/" element={<LoginPage />} />
          <Route path="/show-log-maintain" element={<ShowLogMaintain />} />
          <Route path="/assign-technician" element={<AssignTechnician />} />
          <Route path="/notifications" element={<Notification />} />
          

        </Routes>
      </main>
    </div>
  );
}

export default MDash;
