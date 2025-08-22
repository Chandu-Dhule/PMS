import React, { useState } from 'react';
import './styles/AssignTechnician.css';

function AssignTechnician() {
  const [form, setForm] = useState({
    technicianID: '',
    machineID: '',
    type: ''
  });

  const handleChange = e => {
    const { name, value } = e.target;
    setForm({ ...form, [name]: value });
  };

  const handleSubmit = async e => {
    e.preventDefault();

    const payload = {
      technicianID: form.technicianID,
      machineID: form.machineID,
      type: form.type
    };

    try {
      const response = await fetch('https://localhost:7090/api/AssignTechnician', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload)
      });

      if (!response.ok) throw new Error('Failed to assign technician');

      alert('✅ Technician assigned successfully!');
      setForm({
        technicianID: '',
        machineID: '',
        type: ''
      });
    } catch (error) {
      console.error('Error assigning technician:', error);
      alert('❌ Failed to assign technician. Check console for details.');
    }
  };

  return (
    <div className="form-container-add">
      <h2>Assign Technician</h2>
      <form onSubmit={handleSubmit}>
        <input
          name="technicianID"
          placeholder="Technician ID"
          value={form.technicianID}
          onChange={handleChange}
          required
        />
        <input
          name="machineID"
          placeholder="Machine ID"
          value={form.machineID}
          onChange={handleChange}
          required
        />
        {/* <input
          name="type"
          placeholder="Type"
          value={form.type}
          onChange={handleChange}
          required
        /> */}
        <button type="submit">Assign</button>
      </form>
    </div>
  );
}

export default AssignTechnician;
