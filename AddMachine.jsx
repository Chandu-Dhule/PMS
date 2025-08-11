import React, { useState } from 'react';

function AddMachine() {
  const [machines, setMachines] = useState([]);
  const [form, setForm] = useState({
    name: '', category: '', type: '', installationDate: '', status: ''
  });

  const handleChange = e => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = e => {
    e.preventDefault();
    setMachines([...machines, form]);
    alert("✅ Machine Added!");
    setForm({ name: '', category: '', type: '', installationDate: '', status: '' });
  };

  return (
    <div className="form-container">
      <h2>Add Machine</h2>
      <form onSubmit={handleSubmit}>
        <input name="name" placeholder="Machine Name" value={form.name} onChange={handleChange} required />
        <input name="category" placeholder="Category ID" value={form.category} onChange={handleChange} required />
        <input name="type" placeholder="Type ID" value={form.type} onChange={handleChange} required />
        <input name="installationDate" type="date" value={form.installationDate} onChange={handleChange} required />
        <input name="status" placeholder="Status" value={form.status} onChange={handleChange} required />
        <button type="submit">Add</button>
      </form>
    </div>
  );
}

export default AddMachine;

