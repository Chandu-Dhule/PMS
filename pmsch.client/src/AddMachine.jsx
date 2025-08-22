/*import React, { useState, useEffect } from 'react';

function AddMachine() {
  const [form, setForm] = useState({
    machineID: '',
    name: '',
    categoryID: '',
    typeID: '',
    installationDate: '',
    status: '',
    temperature: ''
  });

  const [categories, setCategories] = useState([]);
  const [types, setTypes] = useState([]);

  useEffect(() => {
    fetch('https://localhost:7090/api/MachineCategories')
      .then(res => res.json())
      .then(data => setCategories(data))
      .catch(err => console.error('Error fetching categories:', err));

    fetch('https://localhost:7090/api/MachineTypes')
      .then(res => res.json())
      .then(data => setTypes(data))
      .catch(err => console.error('Error fetching types:', err));
  }, []);

  const handleChange = e => {
    const { name, value } = e.target;
    setForm({ ...form, [name]: value });
  };

  const handleSubmit = async e => {
    e.preventDefault();
  
    const payload = {
      machineID: parseInt(form.machineID),
      name: form.name,
      categoryID: parseInt(form.categoryID),
      typeID: parseInt(form.typeID),
      installationDate: form.installationDate,
      status: form.status,
      temperature: parseFloat(form.temperature)
    };
  
    try {
      const response = await fetch('https://localhost:7090/api/Machines', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload)
      });
  
      if (!response.ok) {
        const errorText = await response.text();
        if (errorText.includes('Machine already exists')) {
          alert('⚠️ Machine with this ID already exists!');
          return;
        }
        throw new Error('Failed to add machine');
      }
  
      alert('✅ Machine added successfully!');
      setForm({
        machineID: '',
        name: '',
        categoryID: '',
        typeID: '',
        installationDate: '',
        status: '',
        temperature: ''
      });
    } catch (error) {
      console.error('Error adding machine:', error);
      alert('❌ Failed to add machine. Check console for details.');
    }
  };
  

  return (
    <div className="form-container-add">
      <h2>Add Machine</h2>
      <form onSubmit={handleSubmit}>
        <input name="machineID" placeholder="Machine ID" value={form.machineID} onChange={handleChange} required />
        <input name="name" placeholder="Machine Name" value={form.name} onChange={handleChange} required />

        <select name="categoryID" value={form.categoryID} onChange={handleChange} required>
          <option value="">Select Category</option>
          {categories.map(cat => (
            <option key={cat.categoryID} value={cat.categoryID}>{cat.categoryName}</option>
          ))}
        </select>

        <select name="typeID" value={form.typeID} onChange={handleChange} required>
          <option value="">Select Type</option>
          {types.map(type => (
            <option key={type.typeID} value={type.typeID}>{type.typeName}</option>
          ))}
        </select>

        <input name="installationDate" type="datetime-local" value={form.installationDate} onChange={handleChange} required />
        <input name="status" placeholder="Status" value={form.status} onChange={handleChange} required />
        <input name="temperature" type="number" step="0.1" placeholder="Temperature" value={form.temperature} onChange={handleChange} required />

        <button type="submit">Add</button>
      </form>
    </div>
  );
}

export default AddMachine;
*/

import React, { useState, useEffect } from 'react';

function AddMachine() {
  const [form, setForm] = useState({
    machineID: '',
    name: '',
    categoryID: '',
    typeID: '',
    installationDate: '',
    status: '',
    temperature: '',
    lifecycle: '',
    energyConsumption: '', // ✅ New field
    healthStatus: ''        // ✅ New field
  });

  const [categories, setCategories] = useState([]);
  const [types, setTypes] = useState([]);

  useEffect(() => {
    fetch('https://localhost:7090/api/MachineCategories')
      .then(res => res.json())
      .then(data => setCategories(data))
      .catch(err => console.error('Error fetching categories:', err));

    fetch('https://localhost:7090/api/MachineTypes')
      .then(res => res.json())
      .then(data => setTypes(data))
      .catch(err => console.error('Error fetching types:', err));
  }, []);

  const handleChange = e => {
    const { name, value } = e.target;
    setForm({ ...form, [name]: value });
  };

  const handleSubmit = async e => {
    e.preventDefault();

    const payload = {
      machineID: parseInt(form.machineID),
      name: form.name,
      categoryID: parseInt(form.categoryID),
      typeID: parseInt(form.typeID),
      installationDate: form.installationDate,
      status: form.status,
      temperature: parseFloat(form.temperature),
      lifecycle: form.lifecycle,
      energyConsumption: parseFloat(form.energyConsumption), // ✅ Added
      healthStatus: form.healthStatus                        // ✅ Added
    };

    try {
      const response = await fetch('https://localhost:7090/api/Machines', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload)
      });

      if (!response.ok) {
        const errorText = await response.text();
        if (errorText.includes('Machine already exists')) {
          alert('⚠️ Machine with this ID already exists!');
          return;
        }
        throw new Error('Failed to add machine');
      }

      alert('✅ Machine added successfully!');
      setForm({
        machineID: '',
        name: '',
        categoryID: '',
        typeID: '',
        installationDate: '',
        status: '',
        temperature: '',
        lifecycle: '',
        energyConsumption: '',
        healthStatus: ''
      });
    } catch (error) {
      console.error('Error adding machine:', error);
      alert('❌ Failed to add machine. Check console for details.');
    }
  };

  return (
    <div className="form-container-add">
      <h2>Add Machine</h2>
      <form onSubmit={handleSubmit}>
        <input name="machineID" placeholder="Machine ID" value={form.machineID} onChange={handleChange} required />
        <input name="name" placeholder="Machine Name" value={form.name} onChange={handleChange} required />

        <select name="categoryID" value={form.categoryID} onChange={handleChange} required>
          <option value="">Select Category</option>
          {categories.map(cat => (
            <option key={cat.categoryID} value={cat.categoryID}>{cat.categoryName}</option>
          ))}
        </select>

        <select name="typeID" value={form.typeID} onChange={handleChange} required>
          <option value="">Select Type</option>
          {types.map(type => (
            <option key={type.typeID} value={type.typeID}>{type.typeName}</option>
          ))}
        </select>

        <input name="installationDate" type="datetime-local" value={form.installationDate} onChange={handleChange} required />
        <input name="status" placeholder="Status" value={form.status} onChange={handleChange} required />
        <input name="temperature" type="number" step="0.1" placeholder="Temperature" value={form.temperature} onChange={handleChange} required />
        <input name="lifecycle" placeholder="Lifecycle" value={form.lifecycle} onChange={handleChange} required />
        
        {/* ✅ New Fields */}
        <input name="energyConsumption" type="number" step="0.1" placeholder="Energy Consumption (kWh)" value={form.energyConsumption} onChange={handleChange} required />
        <input name="healthStatus" placeholder="Health Status" value={form.healthStatus} onChange={handleChange} required />

        <button type="submit">Add</button>
      </form>
    </div>
  );
}

export default AddMachine;
