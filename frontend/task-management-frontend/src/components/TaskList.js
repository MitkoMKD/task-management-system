import { useContext } from 'react';
import { TaskContext } from '../context/TaskContext';
import { Link } from 'react-router-dom';
import { Button, Card, Form, Table } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';


const TaskList = () => {
  const { tasks, deleteTask, toggleComplete, setFilter, error, filter } = useContext(TaskContext);

  return (
    <div className="container mt-5">
      <h1 className="mb-4 text-center text-primary">Tasks</h1>
      {error && <div className="alert alert-danger mb-4">{error}</div>}
      <div className="d-flex justify-content-between align-items-center mb-4">
        <Form.Select
          value={filter}
          onChange={(e) => setFilter(e.target.value)}
          className="w-auto"
          aria-label="Filter tasks"
        >
          <option value="all">All</option>
          <option value="completed">Completed</option>
          <option value="incomplete">Incomplete</option>
        </Form.Select>
        <Link to="/add" className="btn btn-primary">
          Add Task
        </Link>
      </div>
      <Table striped bordered hover responsive>
        <thead>
          <tr>
            <th className="w-25">Title</th>
            <th className="w-25">Description</th>
            <th className="w-25">Status</th>
            <th className="w-25">Actions</th>
          </tr>
        </thead>
        <tbody>
          {tasks.length === 0 && !error ? (
            <tr>
              <td colSpan="4" className="text-center p-4">
                <Card.Text>No tasks available.</Card.Text>
              </td>
            </tr>
          ) : (
            tasks.map((task) => (
              <tr
                key={task.id}
                style={{ textDecoration: task.isCompleted ? 'line-through' : 'none' }}
              >
                <td className="align-middle">{task.title}</td>
                <td className="align-middle">{task.description || '-'}</td>
                <td className="align-middle">
                  {task.isCompleted ? 'Completed' : 'Incomplete'}
                </td>
                <td className="align-middle">
                  <Button
                    variant={task.isCompleted ? 'success' : 'warning'}
                    size="sm"
                    className="me-2"
                    onClick={() => toggleComplete(task.id, task.isCompleted)}
                  >
                    Toggle {task.isCompleted ? 'Completed' : 'Incomplete'}
                  </Button>
                  <Link
                    to={`/edit/${task.id}`}
                    className="btn btn-info btn-sm me-2"
                  >
                    Edit
                  </Link>
                  <Button
                    variant="danger"
                    size="sm"
                    onClick={() => deleteTask(task.id)}
                  >
                    Delete
                  </Button>
                </td>
              </tr>
            ))
          )}
        </tbody>
      </Table>
    </div>
  );
};

export default TaskList;