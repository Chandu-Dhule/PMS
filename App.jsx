import { Routes, Route } from 'react-router-dom';
import './styles/index.css';
import './styles/App.css';
import '@fortawesome/fontawesome-free/css/all.min.css';
import AddMachine from './AddMachine';
import LogMaintenance from './LogMaintenance';
import Dashboard from "./Dashboard";
import HealthMetrics from "./HealthMetrics";
import Settings from "./Settings";
import Home from './Home';

function App() {
  // Dummy data for now
  const totalMachines = 10;
  const maintenanceLogs = 25;
  const goodMachines = 7;
  const badMachines = 3;

  return (
    <div className="app-container">
      <aside className="sidebar">
        <Dashboard />
      </aside>

      <main className="main-content">
        <Routes>
          <Route
            path="/"
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
          <Route path="/log-maintenance" element={<LogMaintenance />} />
          <Route path="/health-metrics" element={<HealthMetrics />} />
          <Route path="/settings" element={<Settings />} />
        </Routes>
      </main>
    </div>
  );
}
export default App;