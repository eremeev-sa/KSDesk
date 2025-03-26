import { BASE_URL } from './config';

export interface UserRequest {
    name: string;
    login: string;
    password: string;
};

const CURRENT_URL = `${BASE_URL}/UsersKanban`;

// Функция для получения списка пользователей
export const getUsers = async () => {
    try {
        const response = await fetch(`${CURRENT_URL}`);
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

// Функция для создания пользователя
export const createUser = async (userRequest: UserRequest) => {
    await fetch(`${CURRENT_URL}`, {
        method: "POST",
        headers: {
            "content-type": "application/json",
        },
        body: JSON.stringify(userRequest),
    });
};

// Функция для изменения пользователя
export const updateUser = async (id: string, userRequest: UserRequest) => {
    await fetch(`${CURRENT_URL}/${id}`, {
        method: "PUT",
        headers: {
            "content-type": "application/json",
        },
        body: JSON.stringify(userRequest),
    });
};

// Функция для удаления пользователя
export const deleteUser = async (id: string) => {
    await fetch(`${CURRENT_URL}/${id}`, {
        method: "DELETE",
    });
};