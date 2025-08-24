import React, { useEffect, useState } from 'react';
import { Pie, Doughnut } from 'react-chartjs-2';
import { useNavigate } from 'react-router-dom';
import {
    Chart as ChartJS,
    CategoryScale,
    LinearScale,
    ArcElement,
    Tooltip,
    Legend,
} from 'chart.js';
import './styles/AnalysisPage.css';

ChartJS.register(CategoryScale, LinearScale, ArcElement, Tooltip, Legend);

const AnalysisPage = () => {
    const [maintenanceStatusData, setMaintenanceStatusData] = useState([]);
    const [categoryDistributionData, setCategoryDistributionData] = useState([]);
    const [machineData, setMachineData] = useState([]);
    const [machineTypes, setMachineTypes] = useState([]);
    const [loading, setLoading] = useState(true);

    const navigate = useNavigate();
    const id = localStorage.getItem('userId') || 0;
    const role = localStorage.getItem('userRole') || 'Admin';

    useEffect(() => {
        const fetchAnalysisData = async () => {
            try {
                const response = await fetch(`https://localhost:7090/api/Analysis?Id=${id}&Role=${role}`);
                const data = await response.json();
                setMaintenanceStatusData(data.maintenanceStatusData || []);
                setCategoryDistributionData(data.categoryDistributionData || []);
            } catch (error) {
                console.error('Error fetching analysis data:', error);
            }
        };

        const fetchMachineData = async () => {
            try {
                const response = await fetch(`https://localhost:7090/api/Machines/machines/by-role?role=${role}&userId=${id}`);
                const data = await response.json();
                setMachineData(data || []);
            } catch (error) {
                console.error('Error fetching machine data:', error);
            }
        };

        const fetchMachineTypes = async () => {
            try {
                const response = await fetch('https://localhost:7090/api/MachineTypes');
                const data = await response.json();
                setMachineTypes(data || []);
            } catch (error) {
                console.error('Error fetching types:', error);
            }
        };

        const fetchAll = async () => {
            await Promise.all([fetchAnalysisData(), fetchMachineData(), fetchMachineTypes()]);
            setLoading(false);
        };

        fetchAll();
    }, [id, role]);

    const goToNotifications = () => {
        navigate('/notifications', {
            state: { userId: id, role },
        });
    };

    // Map typeID to typeName
    const typeIdToName = machineTypes.reduce((acc, type) => {
        acc[type.typeID] = type.typeName;
        return acc;
    }, {});

    // Count machines by typeName
    const typeNameCounts = machineData.reduce((acc, machine) => {
        const typeName = typeIdToName[machine.typeID] || `Type ${machine.typeID}`;
        acc[typeName] = (acc[typeName] || 0) + 1;
        return acc;
    }, {});

    const doughnutTypeData = {
        labels: Object.keys(typeNameCounts),
        datasets: [
            {
                data: Object.values(typeNameCounts),
                backgroundColor: ['#00b7eb', '#6495ed', '#007fff', '#ffa07a', '#98fb98'],
            },
        ],
    };

    const pieData = {
        labels: maintenanceStatusData.map(item => item.status),
        datasets: [
            {
                data: maintenanceStatusData.map(item => item.count),
                backgroundColor: ['#ff6b6b', '#90ee90'],
            },
        ],
    };

    const doughnutData = {
        labels: categoryDistributionData.map(item => item.category),
        datasets: [
            {
                data: categoryDistributionData.map(item => item.count),
                backgroundColor: ['#00b7eb', '#6495ed', '#007fff'],
            },
        ],
    };

    const chartHeight = '300px';

    return (
        <div className="analysis-page">
            <div className="dashboard-header">
                <div className="header-title">
                    <h2>📈 Machine Analysis</h2>
                    <button
                        className="notification-bell"
                        onClick={goToNotifications}
                        title="View Critical Notifications"
                    >
                        <i className="fas fa-bell"></i>
                    </button>
                </div>
            </div>

            <div className="chart-grid">
                <div className="chart-card" style={{ height: chartHeight }}>
                    <h3>Machine Types</h3>
                    {loading ? <p>Loading...</p> : <Doughnut data={doughnutTypeData} options={{ maintainAspectRatio: false }} />}
                </div>
                <div className="chart-card" style={{ height: chartHeight }}>
                    <h3>Maintenance Status</h3>
                    <Pie data={pieData} options={{ maintainAspectRatio: false }} />
                </div>
                <div className="chart-card" style={{ height: chartHeight }}>
                    <h3>Category Distribution</h3>
                    <Doughnut data={doughnutData} options={{ maintainAspectRatio: false }} />
                </div>
            </div>
        </div>
    );
};

export default AnalysisPage;
