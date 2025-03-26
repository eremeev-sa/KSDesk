import { BASE_URL } from './config';

export interface BoardRequest {
    Name: string;
}

const CURRENT_URL = `${BASE_URL}/BoardsKanban`;

// Функция для получения списка досок
export const getBoards = async () => {
    try {
        const response = await fetch(`${CURRENT_URL}`);
        if (!response.ok) {
            throw new Error(`Failed to fetch boards: ${response.status}`);
        }
        return await response.json();
    } catch (error) {
        console.error('Error fetching boards:', error);
        throw error;
    }
};

// Функция для создания доски
export const createBoard = async (boardRequest: BoardRequest) => {
    try {
        const response = await fetch(`${CURRENT_URL}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(boardRequest),
        });
        if (!response.ok) {
            throw new Error(`Failed to create board: ${response.status}`);
        }
        return await response.json();
    } catch (error) {
        console.error('Error creating board:', error);
        throw error;
    }
};

// Функция для изменения доски
export const updateBoard = async (id: string, boardRequest: BoardRequest) => {
    try {
        const response = await fetch(`${CURRENT_URL}/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(boardRequest),
        });
        if (!response.ok) {
            throw new Error(`Failed to update board: ${response.status}`);
        }
        return await response.json();
    } catch (error) {
        console.error('Error updating board:', error);
        throw error;
    }
};

// Функция для удаления доски
export const deleteBoard = async (id: string) => {
    try {
        const response = await fetch(`${CURRENT_URL}/${id}`, {
            method: 'DELETE',
        });
        if (!response.ok) {
            throw new Error(`Failed to delete board: ${response.status}`);
        }
    } catch (error) {
        console.error('Error deleting board:', error);
        throw error;
    }
};