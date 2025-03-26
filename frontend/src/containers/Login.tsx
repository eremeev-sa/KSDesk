import React, { useState } from 'react';
import { login } from '../services/User';
import { useUser } from '../context/UserContext';

type LoginProps = {
    onLogin: () => void;
};

const Login: React.FC<LoginProps> = ({ onLogin }) => {
    const { setCurrentUser } = useUser();
    const [userLogin, setUserLogin] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState<string | null>(null);

    const handleLogin = async () => {
        try {
            setError(null);

            // Проверяем, что поля не пустые
            if (!userLogin || !password) {
                setError('Пожалуйста, заполните логин и пароль');
                return;
            }

            const userId = await login(userLogin, password);
            setCurrentUser(userId);
            onLogin();
        } catch (error) {
            console.error('Login failed:', error);
            setError('Неверный логин или пароль');
        }
    };

    return (
        <div className="card mx-auto mt-4" style={{ maxWidth: '400px' }}>
            <div className="card-body">
                <h2 className="card-title text-center">Авторизация</h2>
                {error && <p className="text-danger text-center">{error}</p>}
                <form>
                    <div className="mb-3">
                        <label htmlFor="login" className="form-label">
                            Логин
                        </label>
                        <input
                            type="text"
                            className="form-control"
                            id="login"
                            value={userLogin}
                            onChange={(e) => setUserLogin(e.target.value)}
                        />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="password" className="form-label">
                            Пароль
                        </label>
                        <input
                            type="password"
                            className="form-control"
                            id="password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                        />
                    </div>
                    <button
                        type="button"
                        className="btn btn-primary w-100"
                        onClick={handleLogin}
                    >
                        Войти
                    </button>
                </form>
            </div>
        </div>
    );
};

export default Login;