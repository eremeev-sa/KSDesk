import React, { useEffect, useState } from 'react';
import Sidebar from '../components/menu/Sidebar';
import Columns from '../components/main/Columns';
import '../styles/custom.css';
import { useUser } from '../context/UserContext'; // Используем хук для работы с глобальным состоянием пользователя

type KanbanWindowProps = {
    onLogout: () => void; // Функция для выхода из системы
};

const KanbanWindow: React.FC<KanbanWindowProps> = ({ onLogout }) => {
    const { currentUser } = useUser(); // Получаем текущего пользователя из глобального состояния
    const [userName] = useState(currentUser); // Устанавливаем имя пользователя
    const [currentBoardId, setCurrentBoardId] = useState<string>(""); // ID текущей доски

    return (
        <div className="d-flex custom-color">
            {/* Боковая панель, фиксированная по высоте и ширине */}
            <div className="custom-color position-fixed h-100 sidebar-content">
                <Sidebar
                    currentBoardId={currentBoardId} // Текущая доска
                    userName={userName} // Имя пользователя
                    onLogout={onLogout} // Выход из системы
                    onBoardClick={(id) => setCurrentBoardId(id)} // Обновление ID доски при выборе
                />
            </div>

            {/* Основная рабочая область с колонками */}
            <div className="w-100p bg-white">
                <Columns currentBoardId={currentBoardId} />
            </div>
        </div>
    );
};

export default KanbanWindow;
