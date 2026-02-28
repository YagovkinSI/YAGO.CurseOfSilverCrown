import * as React from 'react';
import type { SerializedError } from '@reduxjs/toolkit/react';
import type { FetchBaseQueryError } from '@reduxjs/toolkit/query';
import ModalCard from './ModalCard';
import isErrorWithStatus from './ErrorHandler';

interface ErrorFieldProps {
    title: string;
    error: string | FetchBaseQueryError | SerializedError | undefined;
}

const ErrorField: React.FC<ErrorFieldProps> = ({ title, error }) => {
    const emptyComponent = () => {
        return (<></>)
    }

    const getErrorText = (error: FetchBaseQueryError | SerializedError | string): string => {
        if (typeof error === 'string')
            return error

        if (typeof error === 'object' && 'error' in error && typeof error.error === 'string' &&
            error.error == "TypeError: Failed to fetch")
            return 'Ошибка получения данных с сервера'

        if (typeof error === 'object' && 'data' in error && typeof error.data === 'string')
            return error.data;

        if (typeof error === 'object' && 'data' in error && typeof error.data === 'object' &&
            error.data && 'title' in error.data && typeof error.data.title === 'string') {
            return error.data.title;
        }
        
        if (isErrorWithStatus(error, 401))
            return 'Необходима авторизация.';

        if (isErrorWithStatus(error, 403)) 
            return 'Недостаточно прав.';

        return 'Неизвестная ошибка'
    }

    const alertComponent = (apiError: FetchBaseQueryError | SerializedError | string) => {
        return (
            <ModalCard
                severity={'error'}
                title={title}
                text={getErrorText(apiError)}
                backgroundColor='#ffeeee'
            />
        )
    }

    if (error == undefined) {
        return emptyComponent();
    } else {
        return alertComponent(error);
    }
}

export default ErrorField