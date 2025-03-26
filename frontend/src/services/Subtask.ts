import { BASE_URL } from './config';

export interface SubtaskRequest {
    name: string;
};

const CURRENT_URL = `${BASE_URL}/SubtasksKanban`;

// Функция для получения списка подзадач
export const getSubtasks = async (taskId: string) => {
    try {
        const response = await fetch(`${BASE_URL}/TasksKanban/${taskId}/subtasks`);
        if (!response.ok) {
            throw new Error("Не удалось получить данные с бэкенда");
        }
        const data = await response.json();
        return data;
    } catch (error) {
        console.error(error);
        return [];
    }
};

// Функция для создания подзадачи
export const createSubtask = async (subtaskRequest: SubtaskRequest) => {
    await fetch(`${CURRENT_URL}`, {
        method: "POST",
        headers: {
            "content-type": "application/json",
        },
        body: JSON.stringify(subtaskRequest),
    });
};

// Функция для изменения подзадачи
export const updateSubtask = async (id: string, subtaskRequest: SubtaskRequest) => {
    await fetch(`${CURRENT_URL}/${id}`, {
        method: "PUT",
        headers: {
            "content-type": "application/json",
        },
        body: JSON.stringify(subtaskRequest),
    });
};

// Функция для удаления подзадачи
export const deleteSubtask = async (id: string) => {
    await fetch(`${CURRENT_URL}/${id}`, {
        method: "DELETE",
    });
};