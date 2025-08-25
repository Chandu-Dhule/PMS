import { Routes, Route, Navigate, useLocation } from 'react-router-dom';
import { useEffect, useState } from 'react';
import LoginPage from './LoginPage';
import ADash from './ADash';
import TDash from './TDash';
import MDash from './MDash';
import ProtectedRoute from './ProtectedRoute';

function App() {
    const [isLoggedIn, setIsLoggedIn] = useState(false);
    const location = useLocation();
    const isLoginPage = location.pathname === '/';

    const role = localStorage.getItem('userRole');
    const id = localStorage.getItem('userId');

    useEffect(() => {
        if (id && role) {
            setIsLoggedIn(true);
        }
    }, [id, role]);

    return (
        <>
            <Routes>
                <Route path="/" element={<LoginPage />} />

                {/* Admin Dashboard */}
                <Route
                    path="/admin/*"
                    element={
                        <ProtectedRoute allowedRoles={['Admin']}>
                            <ADash />
                        </ProtectedRoute>
                    }
                />

                {/* Technician Dashboard */}
                <Route
                    path="/technician/*"
                    element={
                        <ProtectedRoute allowedRoles={['Technician']}>
                            <TDash />
                        </ProtectedRoute>
                    }
                />

                {/* Manager Dashboard */}
                <Route
                    path="/manager/*"
                    element={
                        <ProtectedRoute allowedRoles={['Manager']}>
                            <MDash />
                        </ProtectedRoute>
                    }
                />

                {/* Catch-all redirect */}
                {!isLoggedIn && <Route path="*" element={<Navigate to="/" replace />} />}
            </Routes>
        </>
    );
}

export default App;
