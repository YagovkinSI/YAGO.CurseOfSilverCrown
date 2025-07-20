import * as React from 'react';
import type { SerializedError } from '@reduxjs/toolkit/react';
import type { FetchBaseQueryError } from '@reduxjs/toolkit/query';
import ModalCard from './ModalCard';

interface ErrorFieldProps {
    title: string;
    error: FetchBaseQueryError | SerializedError | undefined;
}

const ErrorField: React.FC<ErrorFieldProps> = ({ title, error }) => {
    const emptyComponent = () => {
        return (<></>)
    }

    const getErrorText = (error: FetchBaseQueryError | SerializedError): string => {
        if (typeof error === 'object' && 'error' in error && typeof error.error === 'string' &&
            error.error == "TypeError: Failed to fetch")
            return 'Ошибка получения данных с сервера'

        if (typeof error === 'object' && 'data' in error && typeof error.data === 'string')
            return error.data;

        if (typeof error === 'object' && 'data' in error && typeof error.data === 'object' &&
            error.data && 'title' in error.data && typeof error.data.title === 'string') {
            return error.data.title;
        }

        return 'Неизвестная ошибка'
    }

    const alertComponent = (apiError: FetchBaseQueryError | SerializedError) => {
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
        console.log(error)
        return alertComponent(error);
    }
}

export default ErrorField