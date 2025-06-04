import * as React from 'react';
import { Alert, AlertTitle } from '@mui/material';
import type { SerializedError } from '@reduxjs/toolkit/react';
import type { FetchBaseQueryError } from '@reduxjs/toolkit/query';

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
            error.data && 'title' in error.data &&  typeof error.data.title === 'string') {
            return error.data.title;
        }

        return 'Неизвестная ошибка'
    }

    const alertComponent = (apiError: FetchBaseQueryError | SerializedError) => {
        return (
            <Alert severity="error" sx={{ mt: '1rem', margin: '1rem' }}>
                <AlertTitle>{title}</AlertTitle>
                {getErrorText(apiError)}
            </Alert >)
    }

    if (error == undefined) {
        return emptyComponent();
    } else {
        return alertComponent(error);
    }
}
  
export default ErrorField