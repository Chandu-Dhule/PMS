import EditMachine from './EditMachine';
import DeleteMachine from './DeleteMachine'; // Assuming you have this component
import './styles/Settings.css';

function Settings() {
  return (
    <div className="settings-content">
      <h2 id="m1">Manage Machines</h2>
      <p>Use the forms below to edit or delete machine records.</p>

      <div className="settings-forms-container">
        <div className="settings-form-box">
          <EditMachine />
        </div>
        <div className="settings-form-box">
          <DeleteMachine />
        </div>
      </div>
    </div>
  );
}

export default Settings;
