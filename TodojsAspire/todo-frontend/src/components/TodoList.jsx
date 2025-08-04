import { useState, useEffect, use } from 'react';
import './TodoList.css';
import TodoItem from './TodoItem';

function TodoList() {
    const [tasks, setTasks] = useState([]);
    const [newTaskText, setNewTaskText]=useState('');
    const [todos, setTodo] = useState([]);

    const getTodo = async ()=> {
        fetch("/api/Todo")
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            return response.text();
        })
        .then(text => {
            if (text) {
                setTodo(JSON.parse(text));
            }
        })
        .catch(error => console.error('Error fetching data:', error));        
    }

    useEffect(() => {
        getTodo();
    }, []);

    function handleInputChange(event){
        setNewTaskText(event.target.value);
    }

    async function addTask(event) {
        event.preventDefault();
        if(newTaskText.trim()) {
            const result = await fetch("/api/Todo", {
                method: "POST",
                headers: {"Content-Type": "application/json"},
                body: JSON.stringify({title: newTaskText, isCompleted:false})
            })
            if(result.ok) {
                getTodo();                
            }
            setNewTaskText('');
        }
    }

    async function deleteTask(taskId) {
        const result = await fetch("/api/Todo/" + taskId, {
            method: "DELETE"
        })
        if(result.ok) {
           await getTodo();
        }
    }

    async function moveTaskUp(index) {
     console.log("Move task up:", index);
     const todo = todos[index];
     const result = await fetch(`/api/Todo/move-up/${todo.id}`,{
        method: "POST"
     });
     if(result.ok) {
        await getTodo();
     }
     else {
        console.error("Failed to move task up", result.statusText);
     }
    }

    async function moveTaskDown(index) {
        console.log("Move task up:", index);
     const todo = todos[index];
     const result = await fetch(`/api/Todo/move-down/${todo.id}`,{
        method: "POST"
     });
     if(result.ok) {
        await getTodo();
     }
     else {
        console.error("Failed to move task down", result.statusText);
     }
    }

    return (
        <article
        className="todo-list"
        aria-label="task list manager">
            <header>
                <h1>TODO</h1>
                <form className="todo-input"
                      onSubmit={addTask}
                      aria-controls='todo-list'>
                        <input type="text" required autoFocus placeholder='Enter a task'
                        value={newTaskText} aria-label="Task text"
                        onChange={handleInputChange}/>
                        <button className='add-button' aria-label="Add task">Add</button>
                      </form>
            </header>
            <ol id="todo-list" aria-live="polite" aria-label="task list">
                {todos.map((task, index) =>
                <TodoItem
                    key={task.id}
                    task={task.title}
                    deleteTaskCallback={() => deleteTask(task.id)}
                    moveTaskUpCallback={() => moveTaskUp(index)}
                    moveTaskDownCallback={() => moveTaskDown(index)}
                />
            )}

            </ol>
        </article>
    );
}
export default TodoList;