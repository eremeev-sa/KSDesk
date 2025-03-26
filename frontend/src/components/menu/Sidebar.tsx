import React, { useState, useEffect } from "react";
import BoardList from "./BoardList";
import UserInfo from "./UserInfo";
import { getBoards, updateBoard, BoardRequest, deleteBoard } from "../../services/Board";
import "@fontsource/ibm-plex-sans";

type SidebarProps = {
  userName: string; // Имя текущего пользователя
  onLogout: () => void; // Функция для выхода из системы
  onBoardClick: (id: string) => void; // Обработчик клика на доску
  currentBoardId: string; // ID текущей выбранной доски
};

const Sidebar: React.FC<SidebarProps> = ({ userName, onLogout, onBoardClick, currentBoardId }) => {
  const [data, setData] = useState<{ id: string; name: string }[]>([]); // Список досок
  const [loading, setLoading] = useState(true); // Состояние загрузки

  // Загрузка данных о досках при монтировании компонента
  useEffect(() => {
    const fetchBoards = async () => {
      try {
        const boards = await getBoards(); // Получение списка досок с сервера
        setData(boards); // Обновление состояния с досками
        setLoading(false); // Завершение загрузки
      } catch (error) {
        console.error("Ошибка при загрузке досок:", error);
      }
    };

    fetchBoards();
  }, []);

  // Обработчик обновления доски
  const handleUpdate = async (id: string, boardRequest: BoardRequest) => {
    try {
      await updateBoard(id, boardRequest); // Отправка обновленных данных доски на сервер
      const updatedBoards = await getBoards(); // Получение обновленного списка досок
      setData(updatedBoards); // Обновление данных в состоянии
    } catch (error) {
      console.error("Ошибка при обновлении доски:", error);
    }
  };

  // Обработчик удаления доски
  const handleDelete = async (id: string) => {
    try {
      await deleteBoard(id); // Удаление доски на сервере
      const updatedBoards = await getBoards(); // Получение обновленного списка досок
      setData(updatedBoards); // Обновление данных в состоянии
      onBoardClick(""); // Сброс текущей выбранной доски
    } catch (error) {
      console.error("Ошибка при удалении доски:", error);
    }
  };

  return (
    <div className="sidebar">
      {/* Заголовок боковой панели */}
      <div className="sidebar-header-text">
        <div className="sidebar-header-ico"></div>
        <div>КС Деск</div>
      </div>

      {/* Список досок */}
      <div className="boards">
        {loading ? (
          <h2>Загрузка...</h2>
        ) : (
          <BoardList
            currentBoardId={currentBoardId} // Текущая выбранная доска
            data={data} // Данные досок
            onUpdate={handleUpdate} // Обработчик обновления доски
            onDelete={handleDelete} // Обработчик удаления доски
            onBoardClick={onBoardClick} // Обработчик выбора доски
          />
        )}
      </div>

      {/* Информация о пользователе */}
      <div className="user">
        <UserInfo userName={userName} onLogout={onLogout} />
      </div>
    </div>
  );
};

export default Sidebar;
