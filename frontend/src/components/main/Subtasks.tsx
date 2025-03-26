import React, { useEffect, useState } from 'react';
import { createSubtask, deleteSubtask, getSubtasks } from '../../services/Subtask';
import { SubtaskType } from '../../models/models';
import Subtask from './Subtask';

type SubtaskProps = {
    data: { id: string; name: string }[];
    taskId: string; // ID основной задачи, к которой подзадачи привязаны
};

const Subtasks: React.FC<SubtaskProps> = ({ data, taskId }) => {
    const [addNewSubtask, setAddNewSubtask] = useState(false); // Флаг для отображения формы добавления подзадачи
    const [tempSubtaskName, setTempSubtaskName] = useState(""); // Временное значение для ввода имени подзадачи
    const [localSubtasks, setLocalSubtasks] = useState<SubtaskType[]>(data); // Локальное состояние подзадач

    useEffect(() => {
        setLocalSubtasks(data); // Обновление локальных подзадач при изменении входных данных
    }, [data]);

    // Функция для получения актуальных подзадач после обновления
    const handleSubtaskLocalUpdate = async () => {
        const updatedSubasks = await getSubtasks(taskId); // Получаем обновленные подзадачи с сервера
        setLocalSubtasks(updatedSubasks);  // Обновляем локальное состояние подзадач
    };

    // Функция для добавления новой подзадачи
    const handleAddClick = async () => {
        try {
            const newTask = { name: tempSubtaskName, taskId: taskId }; // Данные для новой подзадачи
            await createSubtask(newTask);

            handleSubtaskLocalUpdate(); // Обновляем список подзадач
            setTempSubtaskName(""); // Очищаем поле ввода
            setAddNewSubtask(false); // Закрываем форму добавления подзадачи
        } catch (error) {
            console.error("Ошибка при добавлении подзадачи:", error);
        }
    };

    // Функция для удаления подзадачи
    const handleDelete = async (id: string) => {
        console.log("Удаление доски с id:", id);
        try {
            await deleteSubtask(id);
            const updatedSubasks = await getSubtasks(taskId);
            setLocalSubtasks(updatedSubasks); // Обновляем локальные данные подзадач после удаления

        } catch (error) {
            console.error("Ошибка при удалении подзадачи:", error);
        }
    };

    // Функция для отмены добавления подзадачи
    const handleCancelClick = () => {
        setTempSubtaskName(""); // Очищаем поле ввода
        setAddNewSubtask(false); // Закрываем форму добавления
    };
    return (
        <div className="row">
            <ul className="list-group h-100p subtasks">
                {localSubtasks.map((subtask) => (
                    <li className="list-group-item subtask" key={subtask.id}>
                        <Subtask
                            id={subtask.id}
                            name={subtask.name}
                            onDelete={handleDelete} // Функция для удаления подзадачи
                            onUpdate={handleSubtaskLocalUpdate} // Функция для обновления списка подзадач
                        />
                    </li>
                ))}
            </ul>
            {/* Форма добавления подзадачи */}
            {addNewSubtask ? (
                <div className="row mt-2 input">
                    <input
                        type="text"
                        className="form-control mb-2 w-100"
                        placeholder="Название подзадачи"
                        value={tempSubtaskName} // Текущее значение ввода
                        onChange={(e) => setTempSubtaskName(e.target.value)} // Обновление значения
                    />
                    <div className="button-container">
                        <button
                            className="btn btn-accept"
                            onClick={handleAddClick} // Добавление подзадачи
                        >
                            ✔
                        </button>
                        <button
                            className="btn btn-cancel"
                            onClick={handleCancelClick} // Отмена добавления подзадачи
                        >
                            ✖
                        </button>
                    </div>
                </div>
            ) : (
                // Кнопка для добавления подзадачи
                <div className="row w-100 mt-2 center">
                    <button
                        className="btn btn-add w-100 me-2 bth-add-subtask center"
                        onClick={() => setAddNewSubtask(true)} // Отображение формы добавления
                    >
                        +
                        <div className='text-on-bth'>
                            Добавить подзадачу
                        </div>
                    </button>
                </div>
            )}
        </div>
    );
};

export default Subtasks;
