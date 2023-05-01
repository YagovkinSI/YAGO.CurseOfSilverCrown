import * as React from 'react';
import { Alert } from 'react-bootstrap';
import { useAppSelector } from '../../store';

const ErrorField: React.FC = () => {
    const state = useAppSelector(state => state.userReducer);

    const emptyComponent = () => {
        return (<></>)
    }

    const alertComponent = () => {
        return (
            <Alert key='danger' variant='danger'>
                ОШИБКА: {state.error}
            </Alert >)
    }

    if (state.error == '') {
        return emptyComponent();
    } else {
        return alertComponent();
    }
}

export default ErrorField