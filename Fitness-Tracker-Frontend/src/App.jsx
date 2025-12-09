import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Login from './pages/Login';
import Register from './pages/Register';
import Dashboard from './pages/Dashboard';
import FitnessGoals from './pages/FitnessGoals';
import Workout from './pages/Workout';
import Statistics from './pages/Statistics';
import Settings from './pages/Settings';
import ProtectedRoute from './components/ProtectedRoute';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route path="/dashboard" element={
          <ProtectedRoute>
            <Dashboard />
          </ProtectedRoute>
        } />
        <Route path="/goals" element={
          <ProtectedRoute>
            <FitnessGoals />
          </ProtectedRoute>
        } />
        <Route path="/workout" element={
          <ProtectedRoute>
            <Workout />
          </ProtectedRoute>
        } />
        <Route path="/statistics" element={
          <ProtectedRoute>
            <Statistics />
          </ProtectedRoute>
        } />
        <Route path="/settings" element={
          <ProtectedRoute>
            <Settings />
          </ProtectedRoute>
        } />
      </Routes>
    </BrowserRouter>
  );
}

export default App;