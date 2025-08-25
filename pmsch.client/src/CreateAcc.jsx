import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './styles/CreateAcc.css';

const CreateAcc = () => {
    const [form, setForm] = useState({ id: '', user: '', pass: '', role: 'Admin' });
    const [message, setMessage] = useState('');
    const navigate = useNavigate();

    const handleChange = e => {
        const { name, value } = e.target;
        setForm(prev => ({ ...prev, [name]: value }));
    };

    const handleSubmit = async e => {
        e.preventDefault();
        setMessage('');

        try {
            const response = await fetch('https://localhost:7090/api/Login/register', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(form)
            });

            if (response.status === 200) {
                setMessage('✅ Account created successfully!');
                // navigate('/login'); // Uncomment if you want to redirect
            } else if (response.status === 409) {
                setMessage('❌ User already exists.');
            } else {
                setMessage('❌ Something went wrong. Please try again.');
            }
        } catch (err) {
            console.error('Account creation error:', err);
            setMessage('❌ Error occurred. Please check your connection.');
        }
    };

    return (
        <div className="createacc-wrapper">
            <div className="createacc-right">
                <h2>Add User</h2>
                <p>Add new user to access the application.</p>
                <form className="createacc-form" onSubmit={handleSubmit}>
                    <label htmlFor="id">User ID</label>
                    <input
                        type="text"
                        id="id"
                        name="id"
                        placeholder="UserID"
                        value={form.id}
                        onChange={handleChange}
                        required
                    />
                    <label htmlFor="username">User Name</label>
                    <input
                        type="text"
                        id="username"
                        name="user"
                        placeholder="Username"
                        value={form.user}
                        onChange={handleChange}
                        required
                    />
                    <label htmlFor="password">Password</label>
                    <input
                        name="pass"
                        id="password"
                        type="password"
                        placeholder="Password"
                        value={form.pass}
                        onChange={handleChange}
                        required
                    />
                    <label htmlFor="role">Role</label>
                    <select name="role" value={form.role} onChange={handleChange}>
                        {/*<option value="Admin">Admin</option>*/}
                        <option value="Manager">Manager</option>
                        <option value="Technician">Technician</option>
                    </select>
                    <button type="submit">Create Account</button>
                    {message && <p>{message}</p>}
                </form>
            </div>
        </div>
    );
};

export default CreateAcc;