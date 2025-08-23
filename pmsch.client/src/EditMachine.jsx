import React, { useState, useEffect } from 'react';

function EditMachine() {

    const [form, setForm] = useState({

        machineID: '',

        name: '',

        categoryID: '',

        typeID: '',

        installationDate: '',

        status: '',

        temperature: '',

        energyConsumption: '', // ✅ Correct field

        healthStatus: ''        // ✅ Correct field

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

    const handleChange = (e) => {

        setForm({ ...form, [e.target.name]: e.target.value });

    };

    const handleSubmit = async (e) => {

        e.preventDefault();

        const { machineID, name, categoryID, typeID, installationDate, status, temperature, energyConsumption, healthStatus } = form;

        if (!machineID) {

            alert("❗ Please enter a Machine ID.");

            return;

        }

        try {

            const getResponse = await fetch(`https://localhost:7090/api/Machines/${machineID}`);

            if (!getResponse.ok) {

                alert("❌ Machine does not exist.");

                return;

            }

            const updatedMachine = {

                name,

                categoryID: parseInt(categoryID),

                typeID: parseInt(typeID),

                installationDate,

                status,

                temperature: parseFloat(temperature),

                energyConsumption: parseFloat(energyConsumption),

                healthStatus

            };

            const putResponse = await fetch(`https://localhost:7090/api/Machines/${machineID}`, {

                method: 'PUT',

                headers: {

                    'Content-Type': 'application/json'

                },

                body: JSON.stringify(updatedMachine)

            });

            if (putResponse.ok) {

                alert("✅ Machine updated successfully!");

            } else {

                alert("❌ Failed to update machine.");

            }

        } catch (error) {

            console.error("Error:", error);

            alert("❌ An error occurred while updating the machine.");

        }

    };

    return (
        <div className="form-container-edit">
            <h2 className="edith2">Edit Machine</h2>
            <form onSubmit={handleSubmit}>
                <input

                    name="machineID"

                    placeholder="Machine ID"

                    value={form.machineID}

                    onChange={handleChange}

                    required

                />
                <input

                    name="name"

                    placeholder="Machine Name"

                    value={form.name}

                    onChange={handleChange}

                    required

                />

                <select name="categoryID" value={form.categoryID} onChange={handleChange} required>
                    <option value="">Select Category</option>

                    {categories.map(cat => (
                        <option key={cat.categoryID} value={cat.categoryID}>

                            {cat.categoryName}
                        </option>

                    ))}
                </select>

                <select name="typeID" value={form.typeID} onChange={handleChange} required>
                    <option value="">Select Type</option>

                    {types.map(type => (
                        <option key={type.typeID} value={type.typeID}>

                            {type.typeName}
                        </option>

                    ))}
                </select>

                <input

                    name="installationDate"

                    type="date"

                    value={form.installationDate}

                    onChange={handleChange}

                    required

                />
                <input

                    name="status"

                    placeholder="Status"

                    value={form.status}

                    onChange={handleChange}

                    required

                />
                <input

                    name="temperature"

                    type="number"

                    step="0.1"

                    placeholder="Temperature"

                    value={form.temperature}

                    onChange={handleChange}

                    required

                />
                <input

                    name="energyConsumption"

                    type="number"

                    step="0.1"

                    placeholder="Energy Consumption"

                    value={form.energyConsumption}

                    onChange={handleChange}

                    required

                />
                <input

                    name="healthStatus"

                    placeholder="Health Status"

                    value={form.healthStatus}

                    onChange={handleChange}

                    required

                />
                <button type="submit">Edit</button>
            </form>
        </div>

    );

}

export default EditMachine;
