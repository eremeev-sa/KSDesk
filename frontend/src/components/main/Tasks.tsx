import React, { useEffect, useState } from 'react';
import Task from './Task';
import { Droppable } from 'react-beautiful-dnd';
import { createTask, deleteTask, TaskUpdateRequest } from '../../services/Task';
import { TaskType, UserType } from '../../models/models';
import { getUsers } from '../../services/User';

type TasksProps = {
    tasks: {
        id: string;
        name: string;
        description: string;
        priority: string;
        columnId: string;
        assigneeId: string;
    }[];
    columnId: string;

    handleTaskUpdate: (id: string, taskRequest: TaskUpdateRequest) => void;
    handleTaskLocalUpdate: () => void;
};

const Tasks: React.FC<TasksProps> = ({ tasks, columnId, handleTaskUpdate, handleTaskLocalUpdate }) => {
    const [addNewTask, setAddNewTask] = useState(false); // Флаг для отображения формы добавления задачи
    const [tempTaskName, setTempTaskName] = useState(''); // Временное название задачи
    const [localTasks, setLocalTasks] = useState<TaskType[]>(tasks); // Локальное состояние задач
    const [usersData, setUsersData] = useState<UserType[]>([]); // Данные о пользователях для назначений задач

    // Обновление локального состояния задач при изменении props.tasks
    useEffect(() => {
        setLocalTasks(tasks);
    }, [tasks]); // Обновляется каждый раз, когда tasks изменяется

    // Получение данных пользователей из API
    useEffect(() => {
        const fetchColumns = async () => {
            const users = await getUsers();
            setUsersData(users);
        }
        fetchColumns();
    }, []);

    // Добавление новой задачи
    const handleAddClick = async () => {
        try {
            const newTask = {
                name: tempTaskName,
                description: "",
                priority: "",
                columnId: columnId,
            };
            await createTask(newTask); // Отправляем запрос на создание задачи
            handleTaskLocalUpdate(); // Уведомляем родительский компонент об обновлении задач
            setTempTaskName(''); // Очищаем поле ввода
            setAddNewTask(false); // Скрываем форму добавления
        } catch (error) {
            console.error("Ошибка при добавлении задачи:", error);
        }
    };

    // Удаление задачи
    const handleDelete = async (id: string) => {
        try {
            await deleteTask(id);
            handleTaskLocalUpdate(); // Уведомляем родительский компонент об обновлении задач
        } catch (error) {
            console.error("Ошибка при удалении задачи:", error);
        }
    };

    // Отмена добавления задачи
    const handleCancelClick = () => {
        setTempTaskName(""); // Очищаем временное название
        setAddNewTask(false); // Скрываем форму добавления
    };

    return (
        <div className="row task">
            {/* Обертка для работы с перетаскиванием задач */}
            <div className="accordion accordion-flush overflow-y-on" id="accordion">
                <Droppable
                    droppableId={columnId}
                    type="TASK"
                    direction="vertical"
                    isCombineEnabled={false}
                >
                    {(provided) => (
                        <div
                            ref={provided.innerRef}
                            {...provided.droppableProps}
                            style={{
                                display: "flex",
                                flexDirection: "column",
                                minHeight: "100px", // минимальная высота для визуализации
                            }}
                        >
                            {localTasks.length === 0 ? (
                                <div style={{ textAlign: "center", padding: "10px" }}>
                                    Задач нет
                                </div>
                            ) : (
                                localTasks.map((task, index) => (
                                    <div className="accordion-item" key={task.id}>
                                        <Task
                                            key={task.id}
                                            {...task}
                                            index={index}
                                            task={task}
                                            onDelete={handleDelete}
                                            handleTaskUpdate={handleTaskUpdate}
                                            usersData={usersData}
                                        />
                                    </div>
                                ))
                            )}
                            {provided.placeholder} {/* Место для визуализации перетаскивания */}
                        </div>
                    )}
                </Droppable>
            </div>

            {/* Форма для добавления новой задачи */}
            {addNewTask ? (
                <div className="row mt-2">
                    <input
                        type="text"
                        className="form-control mb-2"
                        placeholder="Название задачи"
                        value={tempTaskName}
                        onChange={(e) => setTempTaskName(e.target.value)}
                    />
                    <div className="button-container">
                        <button
                            className="btn btn-accept"
                            onClick={handleAddClick}
                        >
                            ✔
                        </button>
                        <button
                            className="btn btn-cancel"
                            onClick={handleCancelClick}
                        >
                            ✖
                        </button>
                    </div>
                </div>
            ) : (
                // Кнопка для отображения формы добавления
                <div className="row w-100 mt-2 center">
                    <button
                        className="btn btn-add w-100 me-2 bth-add-task center"
                        onClick={() => setAddNewTask(true)}>
                        +
                        <div className='text-on-bth'>
                            Добавить задачу
                        </div>
                    </button>
                </div>
            )}
        </div>
    );
};


export default Tasks;
