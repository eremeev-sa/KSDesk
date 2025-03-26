import React, { useState } from 'react';
import { updateSubtask } from '../../services/Subtask';

type SubtaskProps = {
    id: string; // ID подзадачи
    name: string; // Название подзадачи
    onDelete: (id: string) => void; // Функция для удаления подзадачи
    onUpdate: () => void; // Функция для обновления списка подзадач
};

const Subtask: React.FC<SubtaskProps> = ({ id, name, onDelete, onUpdate }) => {
    const [isEditing, setIsEditing] = useState(false); // Флаг для режима редактирования
    const [tempName, setTempName] = useState(name); // Временное название подзадачи, которое редактируется

    // Обработчик клика для начала редактирования
    const handleEditClick = () => {
        setIsEditing(true); // Включаем режим редактирования
    };

    // Функция для обновления подзадачи на сервере
    const handleSubtaskUpdate = async (id: string) => {
        try {
            const updatedSubtask = { name: tempName };  // Данные для обновления
            await updateSubtask(id, updatedSubtask);

            onUpdate();
            setTempName("");
        } catch (error) {
            console.error("Ошибка при обновлении подзадачи:", error);
        }
    };

    // Обработчик клика для сохранения изменений
    const handleSaveClick = () => {
        handleSubtaskUpdate(id); // Обновляем подзадачу
        setIsEditing(false); // Выход из режима редактирования
    };

    // Обработчик клика для отмены редактирования
    const handleCancelClick = () => {
        setTempName(name); // Восстанавливаем исходное имя подзадачи
        setIsEditing(false); // Выход из режима редактирования
    };

    return (
        <div className='subtask'>
            <div className="d-flex w-100 align-items-center justify-content-between">
                {/* Если подзадача в режиме редактирования, показываем поле ввода */}
                {isEditing ? (
                    <div className='input'>
                        <input
                            id="name-input"
                            title="Название задачи"
                            type="text"
                            className="form-control me-2"
                            value={tempName}
                            onChange={(e) => setTempName(e.target.value)} // Обновление значения
                            style={{ width: "100%" }}
                        />
                    </div>
                ) : (
                    // Отображаем название подзадачи, которое можно отредактировать по двойному клику
                    <span
                        className="text-container noselect"
                        onDoubleClick={handleEditClick}
                    >
                        {name}
                    </span>
                )}
                {/* Если подзадача в режиме редактирования, показываем кнопки для сохранения или отмены */}
                {isEditing ? (
                    <>
                        <button
                            className="btn btn-accept btn-sm me-2"
                            style={{ flexShrink: 0 }}
                            onClick={handleSaveClick} // Сохранение изменений
                        >
                            ✔
                        </button>
                        <button
                            className="btn btn-cancel btn-sm"
                            style={{ flexShrink: 0 }}
                            onClick={handleCancelClick} // Отмена редактирования
                        >
                            ✖
                        </button>
                    </>
                ) : (
                    // Кнопка для удаления подзадачи
                    <button
                        className="btn btn-delete btn-sm"
                        onClick={() => onDelete(id)} // Удаление подзадачи
                    >
                        🗑
                    </button>
                )}
            </div>
        </div>
    );
};

export default Subtask;
