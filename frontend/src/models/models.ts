export type TaskType = {
    id: string;
    name: string;
    description: string;
    priority: string;
    columnId: string;
    assigneeId: string;
};

export type UserType = {
    id: string;
    name: string;
    login: string;
    password: string;
};

export type SubtaskType = {
    id: string;
    name: string;
};