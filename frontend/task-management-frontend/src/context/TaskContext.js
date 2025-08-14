import { createContext, useState, useEffect, useCallback } from 'react';
import axios from 'axios';

export const TaskContext = createContext();

export const TaskProvider = ({ children }) => {
  const [tasks, setTasks] = useState([]);
  const [filter, setFilter] = useState('all');
  const [error, setError] = useState(null);

  const fetchTasks = useCallback(async () => {
    const url = filter === 'all' ? '/api/Tasks' : `/api/tasks?status=${filter}`;
    try {
      const res = await axios.get(`https://localhost:44382${url}`);
      setTasks(res.data);
      setError(null);
    } catch (err) {
      console.error('Fetch tasks error:', err);
      setError('Failed to fetch tasks. Please check if the backend server is running.');
    }
  }, [filter]);

  useEffect(() => {
    fetchTasks();
  }, [fetchTasks]);

  const addTask = async (task) => {
    try {
      const res = await axios.post('https://localhost:44382/api/Tasks', task);
      setTasks([...tasks, res.data]);
      setError(null);
    } catch (err) {
      console.error('Add task error:', err);
      setError('Failed to add task.');
    }
  };

  const updateTask = async (id, updatedTask) => {
    try {
      await axios.put(`https://localhost:44382/api/Tasks/${id}`, updatedTask);
      fetchTasks(); // Refetch to ensure consistency with backend
      setError(null);
    } catch (err) {
      console.error('Update task error:', err);
      setError('Failed to update task.');
    }
  };

  const deleteTask = async (id) => {
    try {
      await axios.delete(`https://localhost:44382/api/Tasks/${id}`);
      fetchTasks(); // Refetch to ensure consistency with backend
      setError(null);
    } catch (err) {
      console.error('Delete task error:', err);
      setError('Failed to delete task.');
    }
  };

  const toggleComplete = async (id, isCompleted) => {
    try {
      const updatedTask = {
        isCompleted: !isCompleted,
        id: id,
        title: tasks.find(task => task.id === id).title,
        description: tasks.find(task => task.id === id).description
      }
      await updateTask(id, updatedTask);
      setError(null);
    } catch (err) {
      console.error('Toggle complete error:', err);
      setError('Failed to toggle task completion.');
    }
  };

  return (
    <TaskContext.Provider value={{ tasks, setTasks, addTask, updateTask, deleteTask, toggleComplete, setFilter, error, filter }}>
      {children}
    </TaskContext.Provider>
  );
};
export default TaskProvider;