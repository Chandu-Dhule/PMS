import React, { useState, useEffect } from 'react';
import './styles/AssignTechnician.css';

function AssignTechnician({ role, userId }) {
    const [form, setForm] = useState({
        technicianID: '',
        machineID: ''
    });

    const [machines, setMachines] = useState([]);
    const id = localStorage.getItem('userId');
    const Role = localStorage.getItem('userRole');

    useEffect(() => {
        async function fetchMachines() {
            try {
                const response = await fetch(`https://localhost:7090/api/Machines/machines/by-role?role=${Role}&userId=${id}`);
                const data = await response.json();
                setMachines(data);
            } catch (error) {
                console.error('Error fetching machines:', error);
                alert('❌ Failed to load machines.');
            }
        }

        fetchMachines();
    }, [role, userId]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setForm({ ...form, [name]: value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        const payload = {
            userId: parseInt(form.technicianID),
            machineId: parseInt(form.machineID)
        };

        try {
            const response = await fetch('https://localhost:7090/api/TechnicianMachineAssignment/assign', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload)
            });

            const result = await response.text();

            if (!response.ok) {
                alert(`❌ ${result}`);
            } else {
                alert(`✅ ${result}`);
                setForm({
                    technicianID: '',
                    machineID: ''
                });
            }
        } catch (error) {
            console.error('Error assigning technician:', error);
            alert('❌ Something went wrong. Please check the console for details.');
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
                <select name="machineID" value={form.machineID} onChange={handleChange} required>
                    <option value="">Select Machine</option>
                    {machines.map(m => (
                        <option key={m.machineID} value={m.machineID}>
                            {m.machineName || `Machine ${m.machineID}`}
                        </option>
                    ))}
                </select>
                <button type="submit">Assign</button>
            </form>
        </div>
    );
}

export default AssignTechnician;
