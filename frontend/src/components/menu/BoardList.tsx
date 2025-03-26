import React, { useEffect, useState } from "react";
import Board from "./Board";
import { updateBoard, BoardRequest, createBoard, getBoards } from "../../services/Board";

type BoardListProps = {
  data: { id: string; name: string }[];
  onDelete: (id: string) => void;
  onUpdate: (id: string, boardRequest: BoardRequest) => void;
  onBoardClick: (id: string) => void;
  currentBoardId: string;
};

const BoardList: React.FC<BoardListProps> = ({
  data,
  onDelete,
  onUpdate,
  onBoardClick,
  currentBoardId,
}) => {
  const [addNewBoard, setAddNewBoard] = useState(false); // Состояние для добавления новой доски
  const [tempName, setTempName] = useState(""); // Временное название для новой доски
  const [boardsData, setBoardsData] = useState(data); // Локальное состояние данных о досках

  // Обновление локального состояния при изменении данных из props
  useEffect(() => {
    setBoardsData(data);
    console.log("Полученные данные в BoardList:", data);
  }, [data]);

  // Функция для добавления новой доски
  const handleAddClick = async () => {
    try {
      const boardRequest = { Name: tempName }; // Формируем объект для API
      await createBoard(boardRequest); // Отправляем запрос на создание доски
      const updatedBoards = await getBoards(); // Получаем обновленный список досок с сервера
      setBoardsData(updatedBoards); // Обновляем локальное состояние
      setAddNewBoard(false); // Закрываем форму добавления
      setTempName(""); // Очищаем временное название
    } catch (error) {
      console.error("Ошибка при добавлении доски:", error);
    }
  };

  // Функция для отмены добавления новой доски
  const handleCancelClick = () => {
    setTempName("");
    setAddNewBoard(false);
  };

  return (
    <div className="">
      {/* Заголовок секции с досками */}
      <div className="sidebar-title">
        <h3 className="board-info">Доски</h3>
        {!addNewBoard && (
          <button
            className="btn btn-add"
            onClick={() => setAddNewBoard(true)}
          >
            +
          </button>
        )}
      </div>

      {/* Список досок */}
      <div className="boards-list">
        {boardsData.length === 0 ? (
          <a className="center">Нет досок</a>
        ) : (
          boardsData.map((board) => (
            <div
              key={board.id}
              className={`kanban-boards custom-button ${
                currentBoardId === board.id ? "selected" : "custom-color"
              }`} // Добавляем класс, если доска выбрана
            >
              <Board
                key={board.id}
                Id={board.id}
                Name={board.name}
                onDelete={onDelete}
                onUpdate={onUpdate}
                onBoardClick={onBoardClick}
              />
            </div>
          ))
        )}

        {/* Форма для добавления новой доски */}
        {addNewBoard && (
          <div className="list-group-item new-board d-flex align-items-center justify-content-between">
            <input
              title="Название доски"
              placeholder="Название доски"
              type="text"
              className="form-control board me-2"
              value={tempName}
              onChange={(e) => setTempName(e.target.value)}
            />
            <div className="button-container">
              <button
                className="btn btn-accept btn-sm"
                onClick={handleAddClick}
              >
                ✔
              </button>
              <button
                className="btn btn-cance btn-sm"
                onClick={handleCancelClick}
              >
                ✖
              </button>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default BoardList;
