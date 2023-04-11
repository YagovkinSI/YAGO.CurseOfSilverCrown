import * as React from 'react';
import { Alert } from 'react-bootstrap';
import { useSelector } from 'react-redux';
import { ApplicationState } from '../store';

const ErrorField: React.FC = () => {
    const appState = useSelector(state => state as ApplicationState);
    const error: string = appState.user.error;

    const emptyComponent = () => {
        return (<></>)
    }

    const alertComponent = () => {
        return (
            <Alert key='danger' variant='danger'>
                ОШИБКА: {error}
            </Alert >)
    }

    if (error == '') {
        return emptyComponent();
    } else {
        return alertComponent();
    }
}

export default ErrorField