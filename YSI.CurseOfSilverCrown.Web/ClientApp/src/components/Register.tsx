import * as React from 'react';
import { Button, Card, Form } from 'react-bootstrap';
import { useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { ApplicationState } from '../store';
import { userActionCreators } from '../store/User';

interface RegisterFormState {
    login: string,
    password: string,
    passwordConfirm: string,
    loginError: string,
    passwordError: string,
    passwordConfirmError: string,
}

const defaultRegisterFormState: RegisterFormState = {
    login: '',
    password: '',
    passwordConfirm: '',
    loginError: '',
    passwordError: '',
    passwordConfirmError: '',
}

const Register: React.FC = () => {
    const appState = useSelector(state => state as ApplicationState);
    const dispatch = useDispatch();
    const [registerFormState, setRegisterFormState] = useState(defaultRegisterFormState);

    const submit = (event: React.FormEvent<EventTarget>) => {
        event.preventDefault();
        if (appState.user.isLoading || !validateForm())
            return;
        dispatch(userActionCreators.register(
            registerFormState.login,
            registerFormState.password,
            registerFormState.passwordConfirm
        ));
    }

    const validateForm = () => {
        let success = validateLogin() &&
            validatePassword() &&
            validatePasswordConfirm();
        return success;
    }

    const validateLogin = () => {
        var simbols = /^[a-zA-Z0-9]+$/;
        let error = '';
        if (registerFormState.login === '')
            error = 'Введите логин';
        else if (registerFormState.login.length < 4)
            error = 'Логин должен содержать не менее 4 символов';
        else if (registerFormState.login.length > 12)
            error = 'Логин должен содержать не более 12 символов';
        else if (!registerFormState.login.match(simbols))
            error = 'Логин может содержать только латинские буквы и цифры';
        else
            return true;
        setRegisterFormState({ ...registerFormState, loginError: error })
        return false;
    }

    const validatePassword = () => {
        var lowercase = /[a-z]/;
        var upercase = /[A-Z]/;
        var digit = /[0-9]/;
        let error = '';
        if (registerFormState.password === '')
            error = 'Введите пароль';
        else if (registerFormState.password.length < 6)
            error = 'Пароль должен содержать не менее 6 символов';
        else if (!registerFormState.password.match(lowercase))
            error = 'Пароль должен содержать строчную латинскую букву';
        else if (!registerFormState.password.match(upercase))
            error = 'Пароль должен содержать заглавную латинскую букву';
        else if (!registerFormState.password.match(digit))
            error = 'Пароль должен содержать цифру';
        else
            return true;
        setRegisterFormState({ ...registerFormState, passwordError: error })
        return false;
    }

    const validatePasswordConfirm = () => {
        if (registerFormState.passwordError === '' &&
            registerFormState.passwordConfirm !== registerFormState.password)
            setRegisterFormState({
                ...registerFormState,
                passwordConfirmError: 'Введенные пароли не совпадают'
            })
        else
            return true;
        return false;
    }

    const loginFormGroup = () => {
        return (
            <Form.Group className="mb-3" controlId="formLogin">
                <Form.Label>Логин</Form.Label>
                <Form.Control
                    placeholder="Введите логин"
                    value={registerFormState.login}
                    onChange={e => {
                        setRegisterFormState({
                            ...registerFormState,
                            login: e.target.value,
                            loginError: ''
                        });
                    }}
                    isInvalid={registerFormState.loginError !== ''} />
                <Form.Control.Feedback type='invalid'>
                    {registerFormState.loginError}
                </Form.Control.Feedback>
            </Form.Group>
        )
    }

    const passwordFormGroup = () => {
        return (
            <Form.Group className="mb-3" controlId="formBasicPassword">
                <Form.Label>Пароль</Form.Label>
                <Form.Control
                    type="password"
                    placeholder="Введите пароль"
                    value={registerFormState.password}
                    onChange={e => {
                        setRegisterFormState({
                            ...registerFormState,
                            password: e.target.value,
                            passwordError: '',
                            passwordConfirmError: ''
                        });
                    }}
                    isInvalid={registerFormState.passwordError !== ''} />
                <Form.Control.Feedback type='invalid'>
                    {registerFormState.passwordError}
                </Form.Control.Feedback>
            </Form.Group>
        )
    }

    const confirmPasswordFormGroup = () => {
        return (
            <Form.Group className="mb-3" controlId="formConfirmPassword">
                <Form.Label>Повторите пароль</Form.Label>
                <Form.Control
                    type="password"
                    placeholder="Повторите пароль"
                    value={registerFormState.passwordConfirm}
                    onChange={e => {
                        setRegisterFormState({
                            ...registerFormState,
                            passwordConfirm: e.target.value,
                            passwordConfirmError: ''
                        });
                    }}
                    isInvalid={registerFormState.passwordConfirmError !== ''} />
                <Form.Control.Feedback type='invalid'>
                    {registerFormState.passwordConfirmError}
                </Form.Control.Feedback>
            </Form.Group>
        )
    }

    return (
        <div>
            <Card style={{ width: '18rem', margin: 'auto', marginTop: '1rem' }}>
                <Card.Header as="h2">Регистрация</Card.Header>
                <Card.Body>
                    <Form onSubmit={submit}>
                        {loginFormGroup()}
                        {passwordFormGroup()}
                        {confirmPasswordFormGroup()}
                        <Button
                            variant="primary" 
                            disabled={appState.user.isLoading}
                            type="submit">
                            {appState.user.isLoading
                                ? 'Загрузка...'
                                : 'Регистрация'} 
                        </Button>
                    </Form>
                </Card.Body>
            </Card>
        </div>
    )
};

export default Register;
