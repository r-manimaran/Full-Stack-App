/**
 * 
 * @param {*} param0 
 * @returns 
 */
function TodoItem({task, deleteTaskCallback, moveTaskUpCallback, moveTaskDownCallback}) {
    return(
        <li aria-label="task">
            <span className="text">{task}</span>
            <button type="button" aria-label="Delete task" className="delete-button"
                    onClick={() => deleteTaskCallback()}>
                    🗑️
            </button>
            <button type="button" aria-label="Move task up" className="up-button"
                    onClick={() => moveTaskUpCallback()}>
                    ⬆️
            </button>
            <button type="button" aria-label="Move task down" className="down-button"
                    onClick={() => moveTaskDownCallback()}>
                    ⬇️
            </button>                
        </li>
    );
}
export default TodoItem;