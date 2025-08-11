import { useNavigate } from 'react-router-dom';
import './styles/Settings.css';

function Settings() {
  const navigate = useNavigate();

  return (
    <div className="settings-content">
      <h2>Settings</h2>
      <p>Future customizations and configuration options will live here.</p>

      <div className="settings-actions">
        <div className="settings-box info" onClick={() => navigate('/app-info')}>
          <i className="fas fa-info-circle"></i>
          <span>App Info</span>
        </div>
        <div className="settings-box edit" onClick={() => navigate('/edit-machine')}>
          <i className="fas fa-edit"></i>
          <span>Edit Machine</span>
        </div>
        <div className="settings-box delete" onClick={() => navigate('/delete-machine')}>
          <i className="fas fa-trash-alt"></i>
          <span>Delete Machine</span>
        </div>
      </div>
    </div>
  );
}

export default Settings;

