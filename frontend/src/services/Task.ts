import { BASE_URL } from './config';

export interface TaskRequest {
    name: string;
    description: string;
    priority: string;
    columnId: string;
};

export interface TaskUpdateRequest {
    name: string;
    description: string;
    priority: string;
    assignedId: string;
};

const CURRENT_URL = `${BASE_URL}/TasksKanban`;

// Функция для получения списка задач
export const getTasks = async (columnId: string) => {
    try {
        const response = await fetch(`${BASE_URL}/ColumnsKanban/${columnId}/tasks`);
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

// Функция для получения задач для нескольких колонок
export const getAllTasks = async (columnIds: string[]) => {
    try {
        // Для каждой колонки делаем запрос с помощью getTasks
        const tasks = await Promise.all(
            columnIds.map(columnId => getTasks(columnId))
        );

        // Объединяем все задачи в один массив
        return tasks.flat();  // Метод flat() объединяет массивы в один
    } catch (error) {
        console.error("Ошибка при получении задач для нескольких колонок:", error);
        return [];  // Если ошибка, возвращаем пустой массив
    }
};


// Функция для создания задачи
export const createTask = async (taskRequest: TaskRequest) => {
    await fetch(`${CURRENT_URL}`, {
        method: "POST",
        headers: {
            "content-type": "application/json",
        },
        body: JSON.stringify(taskRequest),
    });
};

// Функция для изменения задачи
export const updateTask = async (id: string, taskRequest: TaskUpdateRequest) => {
    await fetch(`${CURRENT_URL}/${id}`, {
        method: "PUT",
        headers: {
            "content-type": "application/json",
        },
        body: JSON.stringify(taskRequest),
    });
};

// Функция для удаления задачи
export const deleteTask = async (id: string) => {
    await fetch(`${CURRENT_URL}/${id}`, {
        method: "DELETE",
    });
};

// Функция для обновления задачи
export const updateTaskColumn = async (id: string, columnId: string) => {
    const response = await fetch(`${CURRENT_URL}/${id}/column`, {
        method: "PATCH",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(columnId),
    });

    if (!response.ok) {
        throw new Error(`Ошибка при обновлении задачи: ${response.statusText}`);
    }

    return response.json(); // Возвращаем обновлённые данные
};