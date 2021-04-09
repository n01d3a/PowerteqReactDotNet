import axios from 'axios';

const backendServerAddress = process.env.REACT_APP_BACKEND_SERVER;

export function getAllTodoItems() {
    return axios.get(`${backendServerAddress}/Todo`);
}

export function addTodoItem(todoItem) {
    return axios.post(`${backendServerAddress}/Todo`, todoItem);
}

export function updateTodoItem(todoItem) {
    return axios.put(`${backendServerAddress}/Todo`, todoItem);
}

export function deleteTodoItem(todoItemId) {
    return axios.delete(`${backendServerAddress}/Todo/${todoItemId}`);
}