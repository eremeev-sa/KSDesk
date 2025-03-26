import React from "react";

type UserInfoProps = {
  userName: string; // Имя пользователя
  onLogout: () => void; // Функция для выхода из системы
};

const UserInfo: React.FC<UserInfoProps> = ({ userName, onLogout }) => {
  return (
    <div className="user-info">
      {/* Отображение имени пользователя */}
      <h2 className="user-name">{userName}</h2>

      {/* Кнопка выхода */}
      <button className="btn btn-exit" onClick={onLogout}>
        Выйти
      </button>
    </div>
  );
};

export default UserInfo;
