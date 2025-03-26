import { BASE_URL } from './config';

export interface UserRequest {
    name: string;
    login: string;
    password: string;
}

export interface AuthRequest {
    login: string;
    password: string;
}

// Убедимся, что нет лишних слэшей
const CURRENT_URL = `${BASE_URL}/UsersKanban`.replace(/\/+$/, '');

// Функция для авторизации
export const login = async (login: string, password: string) => {
    const url = `${CURRENT_URL}/authenticate`; // Формируем URL явно
    console.log('Sending request to:', url); // Логируем URL для отладки
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ login, password } as AuthRequest),
        });
        if (!response.ok) {
            const errorBody = await response.text();
            throw new Error(`Login failed: ${response.status}, Body: ${errorBody}`);
        }
        return await response.json();
    } catch (error) {
        console.error('Error during login:', error);
        throw error;
    }
};

// Функция для получения списка пользователей
export const getUsers = async () => {
    try {
        const response = await fetch(`${CURRENT_URL}`);
        if (!response.ok) {
            throw new Error(`Failed to fetch users: ${response.status}`);
        }
        return await response.json();
    } catch (error) {
        console.error('Error fetching users:', error);
        throw error;
    }
};

// Функция для создания пользователя
export const createUser = async (userRequest: UserRequest) => {
    try {
        const response = await fetch(`${CURRENT_URL}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(userRequest),
        });
        if (!response.ok) {
            throw new Error(`Failed to create user: ${response.status}`);
        }
        return await response.json();
    } catch (error) {
        console.error('Error creating user:', error);
        throw error;
    }
};

// Функция для изменения пользователя
export const updateUser = async (id: string, userRequest: UserRequest) => {
    try {
        const response = await fetch(`${CURRENT_URL}/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(userRequest),
        });
        if (!response.ok) {
            throw new Error(`Failed to update user: ${response.status}`);
        }
        return await response.json();
    } catch (error) {
        console.error('Error updating user:', error);
        throw error;
    }
};

// Функция для удаления пользователя
export const deleteUser = async (id: string) => {
    try {
        const response = await fetch(`${CURRENT_URL}/${id}`, {
            method: 'DELETE',
        });
        if (!response.ok) {
            throw new Error(`Failed to delete user: ${response.status}`);
        }
    } catch (error) {
        console.error('Error deleting user:', error);
        throw error;
    }
};