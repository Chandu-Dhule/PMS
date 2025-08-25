

// -----------------------------------------------------------------------
import React, { useState } from 'react';

function DeleteMachine() {
  const [form, setForm] = useState({
    category: '', // Assuming 'category' is the Machine ID
  });

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const machineId = form.category;

    try {
      const response = await fetch(`https://localhost:7090/api/Machines/${machineId}`, {
        method: 'DELETE',
        headers: {
          'Content-Type': 'application/json',
         
        },
      });


      

      if (response.status === 204) {
        alert('✅ Machine Deleted!');
      } else if (response.status === 404) {
        const errorText = await response.text();
        alert(`❌ ${errorText}`);
      }
      
 


    } catch (error) {
      alert(`❌ Error: ${error.message}`);
    }

    setForm({ category: '' });
  };

  return (
    <div className="form-container-delete">
      <h2>Delete Machine</h2>
      <form onSubmit={handleSubmit}>
        <input
          name="category"
          placeholder="Machine ID"
          value={form.category}
          onChange={handleChange}
          required
        />
        <button type="submit">Delete</button>
      </form>
    </div>
  );
}

export default DeleteMachine;


