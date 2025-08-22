// import React from 'react';
// import { Bar, Pie, Doughnut } from 'react-chartjs-2';
// import {
//   Chart as ChartJS,
//   CategoryScale,
//   LinearScale,
//   BarElement,
//   ArcElement,
//   Tooltip,
//   Legend,
// } from 'chart.js';
// import './styles/AnalysisPage.css';
 
// ChartJS.register(CategoryScale, LinearScale, BarElement, ArcElement, Tooltip, Legend);
 
// // Dummy data for charts
// const machineHealthData = [
//   { name: 'Machine A', status: 'Good', temp: 80 },
//   { name: 'Machine B', status: 'Needs Maintenance', temp: 30 },
//   { name: 'Machine C', status: 'Good', temp: 60 },
// ];
 
// const maintenanceStatusData = [
//   { status: 'Good', count: 2 },
//   { status: 'Critical', count: 1 },
// ];
 
// const categoryDistributionData = [
//   { category: 'Lathe', count: 1 },
//   { category: 'Drill', count: 1 },
//   { category: 'CNC', count: 1 },
// ];
 
// const AnalysisPage = () => {
//   const barData = {
//     labels: machineHealthData.map(item => item.name),
//     datasets: [
//       {
//         label: 'Machine Temp',
//         data: machineHealthData.map(item => item.temp),
//         backgroundColor: '#3b82f6',
//         borderRadius: 4,
//       },
//     ],
//   };
 
//   const pieData = {
//     labels: maintenanceStatusData.map(item => item.status),
//     datasets: [
//       {
//         data: maintenanceStatusData.map(item => item.count),
//         backgroundColor: ['#90ee90', '#ff6b6b'],
//       },
//     ],
//   };
 
//   const doughnutData = {
//     labels: categoryDistributionData.map(item => item.category),
//     datasets: [
//       {
//         data: categoryDistributionData.map(item => item.count),
//         backgroundColor: ['#00b7eb', '#6495ed', '#007fff'],
//       },
//     ],
//   };
 
//   return (
//     <div className="analysis-page">
//       <h2>📈 Machine Analysis</h2>
//       <div className="chart-grid">
//         <div className="chart-card">
//           <h3>Machine Temperature</h3>
//           <Bar data={barData} />
//         </div>
//         <div className="chart-card">
//           <h3>Maintenance Status</h3>
//           <Pie data={pieData} />
//         </div>
//         <div className="chart-card">
//           <h3>Category Distribution</h3>
//           <Doughnut data={doughnutData} />
//         </div>
//       </div>
//     </div>
//   );
// };
 
// export default AnalysisPage;

// ----------------------------

/*import React from 'react';
import { Bar, Pie, Doughnut } from 'react-chartjs-2';
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  BarElement,
  ArcElement,
  Tooltip,
  Legend,
} from 'chart.js';
import './styles/AnalysisPage.css';

ChartJS.register(CategoryScale, LinearScale, BarElement, ArcElement, Tooltip, Legend);

const machineHealthData = [
  { name: 'Manufacturing A', status: 'Good', temp: 80 },
  { name: 'Packaging A', status: 'Needs Maintenance', temp: 30 },
  { name: 'Manufacturing B', status: 'Good', temp: 60 },
  { name: 'Production A', status: 'Good', temp: 60 },
  { name: 'Packaging B', status: 'Good', temp: 60 },
  // { name: 'Pr', status: 'Good', temp: 60 },
];

const maintenanceStatusData = [
  { status: 'Good', count: 2 },
  { status: 'Critical', count: 1 },
];

const categoryDistributionData = [
  { category: 'Lathe', count: 1 },
  { category: 'Drill', count: 1 },
  { category: 'CNC', count: 1 },
];

const AnalysisPage = () => {
  const barData = {
    labels: machineHealthData.map(item => item.name),
    datasets: [
      {
        label: 'Machine Temp',
        data: machineHealthData.map(item => item.temp),
        backgroundColor: '#3b82f6',
        borderRadius: 4,
      },
    ],
  };

  const barOptions = {
    indexAxis: 'y',
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: {
        position: 'top',
      },
    },
    scales: {
      x: {
        beginAtZero: true,
      },
    },
  };

  const pieData = {
    labels: maintenanceStatusData.map(item => item.status),
    datasets: [
      {
        data: maintenanceStatusData.map(item => item.count),
        backgroundColor: ['#90ee90', '#ff6b6b'],
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

  const chartHeight = '300px'; // You can adjust this as needed

  return (
    <div className="analysis-page">
      <h2>📈 Machine Analysis</h2>
      <div className="chart-grid">
        <div className="chart-card" style={{ height: chartHeight }}>
          <h3>Machine Temperature</h3>
          <Bar data={barData} options={barOptions} />
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
*/



import React, { useEffect, useState } from 'react';
import { Bar, Pie, Doughnut } from 'react-chartjs-2';
import { useNavigate } from 'react-router-dom';
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  BarElement,
  ArcElement,
  Tooltip,
  Legend,
} from 'chart.js';
import './styles/AnalysisPage.css';

ChartJS.register(CategoryScale, LinearScale, BarElement, ArcElement, Tooltip, Legend);

const AnalysisPage = () => {
  const [machineHealthData, setMachineHealthData] = useState([]);
  const [maintenanceStatusData, setMaintenanceStatusData] = useState([]);
  const [categoryDistributionData, setCategoryDistributionData] = useState([]);

  const navigate = useNavigate();
  const id = localStorage.getItem('userId') || 0;
  const role = localStorage.getItem('userRole') || 'Admin';

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await fetch(`https://localhost:7090/api/Analysis?Id=${id}&Role=${role}`);
        const data = await response.json();

        setMachineHealthData(data.machineHealthData || []);
        setMaintenanceStatusData(data.maintenanceStatusData || []);
        setCategoryDistributionData(data.categoryDistributionData || []);
      } catch (error) {
        console.error('Error fetching analysis data:', error);
      }
    };

    fetchData();
  }, [id, role]);

  // Group by machine name and keep the latest status
  const latestMachineData = Object.values(
    machineHealthData.reduce((acc, item) => {
      acc[item.name] = item;
      return acc;
    }, {})
  );

  const criticalMachines = latestMachineData.filter(machine => machine.status === 'Critical');

  const goToNotifications = () => {
    navigate('/notifications', {
      state: {
        userId: id,
        role,
        criticalMachines,
      },
    });
  };

  const barData = {
    labels: latestMachineData.map(item => item.name),
    datasets: [
      {
        label: 'Machine Temp (°C)',
        data: latestMachineData.map(item => item.temp),
        backgroundColor: latestMachineData.map(item =>
          item.status === 'Critical' ? '#ff6b6b' : '#90ee90'
        ),
        borderRadius: 4,
      },
    ],
  };

  const barOptions = {
    indexAxis: 'y',
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: {
        position: 'top',
      },
      tooltip: {
        callbacks: {
          label: function (context) {
            const machine = latestMachineData[context.dataIndex];
            return `Temp: ${machine.temp}°C | Status: ${machine.status}`;
          },
        },
      },
    },
    scales: {
      x: {
        beginAtZero: true,
      },
    },
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
          <h3>Machine Temperature</h3>
          <Bar data={barData} options={barOptions} />
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
