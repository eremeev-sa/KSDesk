import React, { createContext, useContext, useState, ReactNode } from "react";

// Типы данных для контекста
type UserContextType = {
    currentUser: string;
    setCurrentUser: (user: string) => void;
};

// Создаём контекст с начальным значением null
const UserContext = createContext<UserContextType | null>(null);

// Хук для удобного использования контекста
export const useUser = () => {
    const context = useContext(UserContext);
    if (!context) {
        throw new Error("useUser должен использоваться внутри UserProvider");
    }
    return context;
};

// Провайдер для передачи состояния
export const UserProvider = ({ children }: { children: ReactNode }) => {
    const [currentUser, setCurrentUser] = useState("");

    return (
        <UserContext.Provider value={{ currentUser, setCurrentUser }}>
            {children}
        </UserContext.Provider>
    );
};
