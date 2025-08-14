import { useContext, useEffect, useState } from 'react';
import { TaskContext } from '../context/TaskContext';
import { useParams, useNavigate } from 'react-router-dom';
import { Button, Form, InputGroup } from 'react-bootstrap';

const TaskForm = () => {
  const { addTask, updateTask, tasks } = useContext(TaskContext);
  const { id } = useParams();
  const navigate = useNavigate();
  const [task, setTask] = useState({ title: '', description: '', isCompleted: false });
  const [validated, setValidated] = useState(false);

  // Load existing task if editing
  useEffect(() => {
    if (id) {
      const existing = tasks.find((t) => t.id === parseInt(id));
      if (existing) setTask(existing);
    }
  }, [id, tasks]);

  const handleSubmit = (e) => {
    e.preventDefault();
    const form = e.currentTarget;
    if (form.checkValidity() === false) {
      e.stopPropagation();
      setValidated(true);
      return;
    }
    if (id) updateTask(id, task);
    else addTask(task);
    navigate('/');
  };

  return (
    <Form noValidate validated={validated} onSubmit={handleSubmit} className="p-4 border rounded shadow-sm bg-light">
      {/* Title Input */}
      <Form.Group controlId="formTaskTitle" className="mb-3">
        <Form.Label>Title</Form.Label>
        <InputGroup hasValidation>
          <Form.Control
            type="text"
            value={task.title}
            onChange={(e) => setTask({ ...task, title: e.target.value })}
            placeholder="Enter task title"
            required
            isInvalid={!task.title.trim()}
          />
          <Form.Control.Feedback type="invalid">
            Please provide a title.
          </Form.Control.Feedback>
        </InputGroup>
      </Form.Group>

      {/* Description Textarea */}
      <Form.Group controlId="formTaskDescription" className="mb-3">
        <Form.Label>Description</Form.Label>
        <Form.Control
          as="textarea"
          rows={3}
          value={task.description}
          onChange={(e) => setTask({ ...task, description: e.target.value })}
          placeholder="Enter task description"
        />
      </Form.Group>

      {/* Buttons */}
      <div className="d-flex gap-2">
        <Button
          type="submit"
          variant={id ? 'warning' : 'primary'}
          className="w-100"
        >
          {id ? 'Update' : 'Add'} Task
        </Button>
        <Button
          variant="secondary"
          onClick={() => navigate('/')}
          className="w-100"
        >
          Cancel
        </Button>
      </div>
    </Form>
  );
};

export default TaskForm;