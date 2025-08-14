  import './App.css';
  import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
  import TaskList from './components/TaskList';
  import TaskForm from './components/TaskForm';
  import Login from './components/Login';
  import { TaskProvider } from './context/TaskContext';
import { AuthProvider } from './context/AuthContext';

  function App() {
    return (
      <AuthProvider>
      <TaskProvider>
        <Router>
          <Routes>
            <Route path="/login" element={<Login />} />
            <Route path="/tasks" element={<TaskList />} />
            <Route path="/add" element={<TaskForm />} />
            <Route path="/edit/:id" element={<TaskForm />} />
            <Route path="/" element={<TaskList />} />
          </Routes>
        </Router>
      </TaskProvider>
    </AuthProvider>      
    );
  }

  export default App;
