import './App.css';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import TaskList from './components/TaskList';
import TaskForm from './components/TaskForm';
import Login from './components/Login';
import { TaskProvider } from './context/TaskContext';
import { AuthProvider } from './context/AuthContext';
import ProtectedRoute from './components/ProtectedRoute';
import 'bootstrap/dist/css/bootstrap.min.css';

function App() {
  return (
    <AuthProvider>
      <TaskProvider>
        <Router>
          <Routes>
            <Route path="/login" element={<Login />} />
            <Route path="/tasks" element={<ProtectedRoute><TaskList /></ProtectedRoute>} />
            <Route path="/add" element={<ProtectedRoute><TaskForm /></ProtectedRoute>} />
            <Route path="/edit/:id" element={<ProtectedRoute><TaskForm /></ProtectedRoute>} />
            <Route path="/" element={<ProtectedRoute><TaskList /></ProtectedRoute>} />
          </Routes>
        </Router>
      </TaskProvider>
    </AuthProvider>
  );
}

export default App;