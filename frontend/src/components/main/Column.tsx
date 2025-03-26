import React, { useState } from 'react';
import Tasks from './Tasks'; // Компонент для отображения задач
import { ColumnRequest } from '../../services/Column'; // Интерфейсы и функции для работы с колонками
import { Draggable } from 'react-beautiful-dnd'; // Библиотека для Drag-and-Drop
import styled from 'styled-components'; // Для стилизации компонентов
import { colors } from '@atlaskit/theme'; // Цветовые палитры
import { TaskUpdateRequest } from '../../services/Task'; // Интерфейсы и функции для работы с задачами

interface HeaderProps {
    isDragging: boolean; // Флаг, обозначающий, перетаскивается ли элемент
}

const Container = styled.div`
  margin: 0px;
  display: flex;
  flex-direction: column;
`;

const Header = styled.div<HeaderProps>`
    display: flex;
    align-items: center;
    justify-content: center;
    border-radius: 20px;
    background-color: ${({ isDragging }) =>
        isDragging ? colors.Y75 : '#f4f5f6'}; // Изменение цвета при перетаскивании
    transition: background-color 0.2s ease;
    &:hover {
      background-color: #ffffff;
    }
  `;

type ColumnProps = {
    id: string; // Идентификатор колонки
    name: string; // Название колонки
    index: number; // Порядковый номер колонки
    tasks: { // Список задач
        id: string;
        name: string;
        description: string;
        priority: string;
        columnId: string;
        assigneeId: string;
    }[];

    handleTaskUpdate: (id: string, taskRequest: TaskUpdateRequest) => void; // Обновление задачи
    handleTaskLocalUpdate: () => void; // Локальное обновление задач
    onDelete: (id: string) => void; // Удаление колонки
    onUpdate: (id: string, columnRequest: ColumnRequest) => void; // Обновление колонки
};

const Column: React.FC<ColumnProps> = ({ id, name, index, onDelete, onUpdate, tasks, handleTaskUpdate, handleTaskLocalUpdate }) => {
    const [isEditing, setIsEditing] = useState(false); // Флаг режима редактирования
    const [tempName, setTempName] = useState(name); // Временное имя для редактирования

    // Обработка клика для перехода в режим редактирования
    const handleEditClick = () => {
        setIsEditing(true);
    };

    // Сохранение изменений названия колонки
    const handleSaveClick = () => {
        const columnRequest = { id: id, name: tempName };
        onUpdate(id, columnRequest);
        setIsEditing(false); // Выход из режима редактирования
    };
    
    // Отмена изменений названия колонки
    const handleCancelClick = () => {
        setTempName(name); // Сбрасываем временное имя
        setIsEditing(false); // Выход из режима редактирования
    };

    return (
        <Draggable draggableId={id} index={index} key={id}>
            {(provided, snapshot) => (
                <Container ref={provided.innerRef} {...provided.draggableProps}>
                    <div className="ms-4">
                        <div className="card">
                            <div className='kanban-column'>
                                <div className="custom-card-header text-center d-flex">
                                    {isEditing ? (
                                        // Отображение поля ввода и кнопок в режиме редактирования
                                        <>
                                            <input
                                                title="Название колонки"
                                                type="text"
                                                className="form-control column me-2"
                                                value={tempName}
                                                onChange={(e) => setTempName(e.target.value)}
                                            />
                                            <div className="button-container d-flex align-items-center">
                                                <button
                                                    className="btn btn-accept btn-sm me-2"
                                                    style={{ flexShrink: 0 }}
                                                    onClick={handleSaveClick}
                                                >
                                                    ✔
                                                </button>
                                                <button
                                                    className="btn btn-cancel btn-sm"
                                                    style={{ flexShrink: 0 }}
                                                    onClick={handleCancelClick}
                                                >
                                                    ✖
                                                </button>
                                            </div>
                                        </>
                                    ) : (
                                        // Отображение названия и кнопки удаления в обычном режиме
                                        <div className="d-flex w-100">
                                            <Header isDragging={snapshot.isDragging}>
                                                {/* Область для захвата при перетаскивании */}
                                                <div
                                                    {...provided.dragHandleProps}
                                                    className="drag-handle center"
                                                >
                                                    ✥
                                                </div>
                                            </Header>

                                            <div
                                                style={{
                                                    fontWeight: 'bold',
                                                    margin: '8px',
                                                }}
                                            >
                                                <span
                                                    className="text-container noselect"
                                                    onDoubleClick={handleEditClick} // Двойной клик для редактирования
                                                >
                                                    {name}
                                                </span>
                                            </div>

                                            <div className="button-container d-flex align-items-center ms-auto">
                                                <button
                                                    className="btn btn-delete btn-sm"
                                                    onClick={() => onDelete(id)}
                                                >
                                                    🗑
                                                </button>
                                            </div>
                                        </div>

                                    )}
                                </div>
                                {/* Отображение списка задач */}
                                <div className="card-body">
                                    <Tasks tasks={tasks} handleTaskUpdate={handleTaskUpdate} handleTaskLocalUpdate={handleTaskLocalUpdate} columnId={id} />
                                </div>
                            </div>
                        </div>
                    </div>
                </Container>
            )}
        </Draggable>
    )
};


export default Column;
