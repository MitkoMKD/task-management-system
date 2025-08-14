import { createContext, useState, useEffect, useCallback } from 'react';
import axios from 'axios';
import { useAuth } from './AuthContext';

export const TaskContext = createContext();

export const TaskProvider = ({ children }) => {
  const [tasks, setTasks] = useState([]);
  const [filter, setFilter] = useState('all');
  const [error, setError] = useState(null);
  const { credentials, isAuthenticated } = useAuth();

  const axiosInstance = axios.create({
    baseURL: 'https://localhost:44382',
  });

  const addAuthHeader = (config) => {
    if (isAuthenticated && credentials.username && credentials.password) {
      const base64Credentials = btoa(`${credentials.username}:${credentials.password}`);
      config.headers.Authorization = `Basic ${base64Credentials}`;
    }
    return config;
  };

  axiosInstance.interceptors.request.use(addAuthHeader, (error) => Promise.reject(error));

  const fetchTasks = useCallback(async () => {
    if (!isAuthenticated) {
      setError('Please log in to view tasks.');
      return;
    }
    const url = filter === 'all' ? '/api/Tasks' : `/api/Tasks?status=${filter}`;
    try {
      const res = await axiosInstance.get(url);
      setTasks(res.data);
      setError(null);
    } catch (err) {
      console.error('Fetch tasks error:', err);
      setError('Failed to fetch tasks. Check authentication or server.');
    }
  }, [filter, isAuthenticated, axiosInstance]); // Added axiosInstance to dependency array

  useEffect(() => {
    fetchTasks();
  }, [fetchTasks]);

  const addTask = async (task) => {
    if (!isAuthenticated) {
      setError('Please log in to add tasks.');
      return;
    }
    try {
      const res = await axiosInstance.post('/api/Tasks', task);
      setTasks([...tasks, res.data]);
      setError(null);
    } catch (err) {
      console.error('Add task error:', err);
      setError('Failed to add task.');
    }
  };

  const updateTask = async (id, updatedTask) => {
    if (!isAuthenticated) {
      setError('Please log in to update tasks.');
      return;
    }
    try {
      await axiosInstance.put(`/api/Tasks/${id}`, updatedTask);
      fetchTasks();
      setError(null);
    } catch (err) {
      console.error('Update task error:', err);
      setError('Failed to update task.');
    }
  };

  const deleteTask = async (id) => {
    if (!isAuthenticated) {
      setError('Please log in to delete tasks.');
      return;
    }
    try {
      await axiosInstance.delete(`/api/Tasks/${id}`);
      fetchTasks();
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