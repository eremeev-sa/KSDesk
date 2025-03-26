import React, { useEffect, useState } from 'react';
import { DragDropContext, Droppable } from 'react-beautiful-dnd';
import Column from './Column';
import { ColumnRequest, createColumn, deleteColumn, getColumns, updateColumn, updateColumnOrder } from '../../services/Column';
import { getAllTasks, TaskRequest, TaskUpdateRequest, updateTask, updateTaskColumn } from '../../services/Task';
import styled from 'styled-components';
import { TaskType } from '../../models/models';

type ColumnsProps = {
    currentBoardId: string; // Текущий идентификатор доски
};

const Container = styled.div`
  background-color: white;
  min-height: 100vh;
  min-width: 100vw; /* Минимальная ширина контейнера */
  display: inline-flex; /* Для горизонтального расположения */
`;

const Columns: React.FC<ColumnsProps> = ({ currentBoardId }) => {
    const [tasks, setTasks] = useState<TaskType[]>([]); // Состояние задач
    const [data, setData] = useState<{ id: string; name: string }[]>([]); // Состояние колонок
    const [loading, setLoading] = useState(true); // Состояние загрузки
    const [tempName, setTempName] = useState(""); // Название новой колонки
    const [addNewColumn, setAddNewColumn] = useState(false); // Флаг добавления новой колонки


    // Загрузка колонок при изменении текущей доски
    useEffect(() => {
        const fetchColumns = async () => {
            if (currentBoardId !== "") {
                try {
                    setLoading(true);
                    const columns = await getColumns(currentBoardId);
                    setData(columns);
                } catch (error) {
                    console.error("Ошибка при загрузке колонок:", error);
                } finally {
                    setLoading(false);
                }
            } else {
                setData([]); // Сбрасываем данные, если доска не выбрана
            }
        };

        fetchColumns();
    }, [currentBoardId]); // Зависимость — currentBoardId

    // Загрузка задач после получения колонок
    useEffect(() => {
        const loadAllTasks = async () => {
            if (data.length > 0) {
                const columnIds = data.map(column => column.id);  // Получаем список всех columnId
                const allTasks = await getAllTasks(columnIds);  // Загружаем все задачи для этой доски
                setTasks(allTasks);
            }
        };

        loadAllTasks();
    }, [data]);

    // Добавление новой колонки
    const handleAddClick = async () => {
        try {
            const columnRequest = { name: tempName, boardId: currentBoardId };
            await createColumn(columnRequest);
            const updatedBoards = await getColumns(currentBoardId);
            setData(updatedBoards);
            setAddNewColumn(false);
            // Очищаем поле ввода
            setTempName("");
            console.log(data);
        } catch (error) {
            console.error("Ошибка при добавлении доски:", error);
        }
    };

    const handleCancelClick = () => {
        setTempName("");
        setAddNewColumn(false);
    };

    // Обновление колонки
    const handleUpdate = async (id: string, columnRequest: ColumnRequest) => {
        const name = columnRequest.name
        try {
            await updateColumn(id, columnRequest);

            const updatedBoards = await getColumns(currentBoardId);
            setData(updatedBoards);
        } catch (error) {
            console.error("Ошибка при обновлении доски:", error);
        }
    };

    // Удаление колонки
    const handleDelete = async (id: string) => {
        try {
            await deleteColumn(id);
            const updatedBoards = await getColumns(currentBoardId); // Обновляем список досок с сервера
            setData(updatedBoards); // Устанавливаем новые данные в состояние
        } catch (error) {
            console.error("Ошибка при удалении колонки:", error);
        }
    };

    // Обновление задач в текущей доске для локального хранения
    const handleTaskLocalUpdate = async () => {
        if (data.length > 0) {
            const columnIds = data.map(column => column.id);
            const allTasks = await getAllTasks(columnIds);
            setTasks(allTasks);
        }
    };

    // Обновление задачи
    const handleTaskUpdate = async (id: string, taskRequest: TaskUpdateRequest) => {
        try {
            await updateTask(id, taskRequest);
            handleTaskLocalUpdate();
        } catch (error) {
            console.error("Ошибка при обновлении задачи:", error);
        }
    };

    // Обработка завершения перетаскивания
    const handleOnDragEnd = async (result: any) => {
        const { source, destination, type } = result;
        // Если задача не была перемещена (находится в том же месте)
        if (!destination) return;


        // Получение ID колонки, в которую была перемещена задача
        const targetColumnId = result.destination.droppableId;
        console.log("Задача перемещена в колонку с ID:", targetColumnId);
        // Логика для перемещения колонок
        if (type === "COLUMN") {
            // Получение новый порядок колонок
            const reorderedColumns = Array.from(data);
            const [movedColumn] = reorderedColumns.splice(source.index, 1);
            reorderedColumns.splice(destination.index, 0, movedColumn);

            // Отправление нового поряка на сервер
            const orderedColumnIds = reorderedColumns.map((column) => column.id);
            try {
                setData(reorderedColumns); // Обновление локальных данных
                await updateColumnOrder({ orderedColumnIds });
            } catch (error) {
                console.error('Ошибка при обновлении порядка колонок:', error);
            }

            return;
        }

        // Логика для перемещения задач
        else if (type === "TASK") {
            // Получение ID задачи
            const taskId = result.draggableId;
            try {
                await updateTaskColumn(taskId, targetColumnId);
                handleTaskLocalUpdate();
            } catch (error) {
                console.error("Ошибка при обновлении порядка задач:", error);
            }

            return;
        }
    };


    return (
        <div className="kanban-columns-container mt-4">
            {loading ? (
                <p className='ms-2'>Загрузка...</p>
            ) : (
                <>
                    <DragDropContext onDragEnd={handleOnDragEnd}>
                        <Droppable
                            droppableId={currentBoardId}
                            type="COLUMN"
                            direction="horizontal"
                            isCombineEnabled={false}
                        >
                            {(provided) => (
                                <div
                                    ref={provided.innerRef}
                                    {...provided.droppableProps}
                                    style={{
                                        display: 'flex',
                                        flexDirection: 'row',
                                    }}
                                >
                                    {data.map((column, index) => (
                                        <Column
                                            key={column.id}
                                            {...column}
                                            index={index}
                                            onUpdate={handleUpdate}
                                            onDelete={handleDelete}
                                            tasks={tasks.filter(task => task.columnId === column.id)}
                                            handleTaskUpdate={handleTaskUpdate}
                                            handleTaskLocalUpdate={handleTaskLocalUpdate}
                                        />
                                    ))}
                                    {provided.placeholder}
                                </div>
                            )}

                        </Droppable>
                    </DragDropContext>
                    {addNewColumn ? <>
                        <div className="new-column">
                            <div className="ms-4">
                                <div className="card">
                                    <input
                                        title="Название доски"
                                        placeholder="Название доски"
                                        type="text"
                                        className="form-control me-2"
                                        value={tempName}
                                        onChange={(e) => setTempName(e.target.value)}
                                    />
                                    <div className="card-body">
                                    </div>
                                </div>
                            </div>
                            <div className="button-container ms-4">
                                <button
                                    className="btn btn-accept btn-sm"
                                    onClick={handleAddClick} // Добавление новой записи
                                >
                                    ✔
                                </button>
                                <button
                                    className="btn btn-cancel btn-sm"
                                    onClick={handleCancelClick} // Отмена
                                >
                                    ✖
                                </button>
                            </div>
                        </div>

                    </> :
                        <div className="mb-4 ms-4 new-column">
                            <button
                                className="btn btn-add w-100 bth-add-task center"
                                onClick={() => setAddNewColumn(true)}>
                                +
                                <div className='text-on-bth'>
                                    Добавить колонку
                                </div>
                            </button>
                        </div>
                    }
                </>
            )}
        </div>
    );
};

export default Columns;
