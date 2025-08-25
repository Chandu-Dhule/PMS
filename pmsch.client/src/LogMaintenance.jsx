import React, { useState, useEffect } from 'react';

function LogMaintenance() {
    const [logs, setLogs] = useState([]);
    const [machines, setMachines] = useState([]);
    const [logData, setLogData] = useState({
        logId: '',
        machineId: '',
        date: '',
        operator: '',
        description: '',
        status:'',
        temperature: '',
        energyConsumption: '',
        healthStatus: '',
        nextDueDate: ''
    });

    const userId = localStorage.getItem('userId');

    useEffect(() => {
        async function fetchMachines() {
            try {
                const response = await fetch(`https://localhost:7090/api/Machines/machines/by-role?role=Technician&userId=${userId}`);
                const data = await response.json();
                setMachines(data);
            } catch (error) {
                console.error('Error fetching machines:', error);
                alert('❌ Failed to load machines.');
            }
        }

        fetchMachines();
    }, [userId]);

    const handleChange = (e) => {
        setLogData({ ...logData, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        const temperature = parseFloat(logData.temperature);
        const energyConsumption = parseFloat(logData.energyConsumption);
        const healthStatus = (temperature > 80 || energyConsumption > 100) ? 'Critical' : 'Good';
        const status = (temperature > 80 || energyConsumption > 100) ? 'Need Maintenance' : 'Active';

        const payload = {
            logID: '0',
            machineID: parseInt(logData.machineId),
            maintenanceDate: logData.date,
            description: logData.description,
            operatorName: logData.operator,
            status,
            temperature,
            energyConsumption,
            healthStatus,
            nextDueDate: logData.nextDueDate
        };

        try {
            const response = await fetch('https://localhost:7090/api/MaintenanceLogs', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(payload)
            });

            const result = await response.text();

            if (!response.ok) {
                alert(`❌ ${result}`);
            } else {
                alert(`✅ Log added successfully!`);
                setLogs([...logs, logData]);
                setLogData({
                    machineId: '',
                    date: '',
                    operator: '',
                    description: '',
                    temperature: '',
                    energyConsumption: '',
                    healthStatus: '',
                    nextDueDate: ''
                });
            }
        } catch (error) {
            console.error('Error adding maintenance log:', error);
            alert('❌ Something went wrong. Please check the console for details.');
        }
    };


    return (
        <div className="form-container-log">
            <h2>Log Maintenance</h2>
            <form onSubmit={handleSubmit}>
                {/*<input*/}
                {/*    name="logId"*/}
                {/*    placeholder="Log ID"*/}
                {/*    value={logData.logId}*/}
                {/*    onChange={handleChange}*/}
                {/*    required*/}
                {/*/>*/}
                <select
                    name="machineId"
                    value={logData.machineId}
                    onChange={handleChange}
                    required
                >
                    <option value="">Select Machine</option>
                    {machines.map(m => (
                        <option key={m.machineID} value={m.machineID}>
                            {m.machineName || `Machine ${m.machineID}`}
                        </option>
                    ))}
                </select>
                <input
                    name="date"
                    type="date"
                    value={logData.date}
                    onChange={handleChange}
                    required
                />
                <input
                    name="operator"
                    placeholder="Operator Name"
                    value={logData.operator}
                    onChange={handleChange}
                    required
                />
                <textarea
                    name="description"
                    placeholder="Description"
                    value={logData.description}
                    onChange={handleChange}
                    required
                />
                <input
                    name="temperature"
                    type="number"
                    step="0.1"
                    placeholder="Temperature (°C)"
                    value={logData.temperature}
                    onChange={handleChange}
                    required
                />
                <input
                    name="energyConsumption"
                    type="number"
                    step="0.1"
                    placeholder="Energy Consumption (kWh)"
                    value={logData.energyConsumption}
                    onChange={handleChange}
                    required
                />
                {/*<input*/}
                {/*    name="healthStatus"*/}
                {/*    placeholder="Health Status"*/}
                {/*    value={logData.healthStatus}*/}
                {/*    onChange={handleChange}*/}
                {/*/>*/}
                <label htmlFor="nextDueDate">Next Due Date:</label>
                <input
                    name="nextDueDate"
                    type="date"
                    value={logData.nextDueDate}
                    onChange={handleChange}
                    required
                />
                <button type="submit">Log</button>
            </form>
        </div>
    );
}

export default LogMaintenance;
