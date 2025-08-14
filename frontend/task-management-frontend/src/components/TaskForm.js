import { createContext, useState, useEffect } from 'react';
import axios from 'axios';

export const TaskContext = createContext();

const fetchTasks = async (filter, setTasks, setError) => {
  const url = filter === 'all' ? '/api/Tasks' : `/api/Tasks?status=${filter}`;
  try {
    const res = await axios.get(`https://localhost:44382${url}`);
    setTasks(res.data);
    setError(null);
  } catch (err) {
    console.error('Fetch tasks error:', err);
    setError('Failed to fetch tasks. Please check if the backend server is running.');
  }
};

export const TaskProvider = ({ children }) => {
  const [tasks, setTasks] = useState([]);
  const [filter, setFilter] = useState('all');
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchTasks(filter, setTasks, setError);
  }, [filter]); // Only filter is a dependency

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
      updatedTask.UpdatedAt = new Date();
      updatedTask.id = id;
      await axios.put(`https://localhost:44382/api/Tasks/${id}`, updatedTask);
      fetchTasks(filter, setTasks, setError);
      setError(null);
    } catch (err) {
      console.error('Update task error:', err);
      setError('Failed to update task.');
    }
  };

  const deleteTask = async (id) => {
    try {
      await axios.delete(`https://localhost:44382/api/Tasks/${id}`);
      fetchTasks(filter, setTasks, setError);
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
    <TaskContext.Provider value={{ tasks, addTask, updateTask, deleteTask, toggleComplete, setFilter, error }}>
      {children}
    </TaskContext.Provider>
  );
};
export default TaskProvider;