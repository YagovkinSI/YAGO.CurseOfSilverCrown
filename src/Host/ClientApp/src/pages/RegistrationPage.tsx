import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { User, Lock, ArrowLeft, LogIn, UserPlus, Mail } from 'lucide-react';
import { useFormik } from 'formik';
import * as Yup from 'yup';
import InputText from '../shared/ui/InputText';
import { useLoginMutation, useRegisterMutation } from "../entities/users/user.api";
import Text from '../shared/ui/Text';
import Title from '../shared/ui/Title';
import Button from '../shared/ui/buttons/Button';
import Card from '../shared/ui/Card';
import Page from '../widgets/Page';
import ButtonLink from '../shared/ui/buttons/ButtonLink';

type AuthMode = 'login' | 'register';

const RegistrationPage: React.FC = () => {
    const navigate = useNavigate();
    const [mode, setMode] = useState<AuthMode>('login');
    const [loginMutate, loginMutateResult] = useLoginMutation();
    const [registerMutate, registerMutateResult] = useRegisterMutation();

    const isLoading = loginMutateResult.isLoading || registerMutateResult.isLoading;
    const error = loginMutateResult.error ?? registerMutateResult.error;

    // Валидация
    const validationSchema = Yup.object().shape({
        userName: Yup.string()
            .required('Введите логин')
            .min(3, 'Логин должен содержать не менее 3 символов')
            .max(20, 'Логин должен содержать не более 20 символов')
            .matches(/^[a-zA-Z0-9_-]+$/, 'Логин может содержать только латинские буквы, цифры, подчеркивание (_) и дефис (-)')
            .matches(/[a-zA-Z]/, 'Логин должен содержать хотя бы одну латинскую букву'),
        email: Yup.string()
            .email('Введите корректный email')
            .nullable()
            .notRequired(),
        password: Yup.string()
            .required('Введите пароль')
            .min(6, 'Пароль должен содержать не менее 6 символов')
            .max(20, 'Пароль должен содержать не более 20 символов')
            .matches(/[a-z]/, 'Пароль должен содержать строчную латинскую букву')
            .matches(/[A-Z]/, 'Пароль должен содержать заглавную латинскую букву')
            .matches(/[0-9]/, 'Пароль должен содержать цифру')
            .matches(/^[a-zA-Z0-9!@#$%^&*()\-_=+[\]{};:,./?~`"']+$/, 'Пароль содержит недопустимые символы'),
        passwordConfirm: mode === 'login'
            ? Yup.string()
            : Yup.string()
                .required('Введите пароль ещё раз')
                .oneOf([Yup.ref('password'), ''], 'Пароли не совпадают'),
    });

    const formik = useFormik({
        initialValues: {
            userName: '',
            email: '',
            password: '',
            passwordConfirm: '',
        },
        validationSchema,
        onSubmit: (values) => {
            const mutate = mode === 'login' ? loginMutate : registerMutate;
            mutate(values)
                .unwrap()
                .then(() => navigate('/me/colony'))
                .catch((err) => {
                    console.error('Auth failed:', err);
                });
        },
    });

    const renderHeader = () => (
        <div className="flex items-center justify-between w-full">
            <button
                onClick={() => navigate('/')}
                className="flex items-center gap-2 text-muted hover:text-light transition-colors"
            >
                <ArrowLeft className="w-4 h-4" />
                <span className="text-sm">Назад</span>
            </button>
            <ButtonLink
                onClick={() => setMode(mode === 'login' ? 'register' : 'login')}
            >
                {mode === 'login' ? 'Создать аккаунт' : 'Уже есть аккаунт? Войти'}
            </ButtonLink>
        </div>
    );

    const renderTitle = () => (
        <div className="text-center">
            <Title>{mode === 'login' ? 'Вход' : 'Регистрация'}</Title>
            <Text variant="secondary" size="sm" className="mt-1">
                {mode === 'login'
                    ? 'Войдите в свой аккаунт'
                    : 'Создайте новый аккаунт'}
            </Text>
        </div>
    );

    const renderForm = () => (
        <form onSubmit={formik.handleSubmit} className="flex flex-col gap-4 w-full">
            <InputText
                label="Логин"
                name="userName"
                autoComplete="username"
                autoFocus
                value={formik.values.userName}
                handleChange={formik.handleChange}
                handleBlur={formik.handleBlur}
                error={formik.touched.userName && Boolean(formik.errors.userName)}
                helperText={formik.touched.userName && formik.errors.userName}
                icon={<User className="w-4 h-4" />}
            />

            {mode === 'register' && (
                <InputText
                    label="Email (необязательно)"
                    name="email"
                    type="email"
                    autoComplete="email"
                    value={formik.values.email}
                    handleChange={formik.handleChange}
                    handleBlur={formik.handleBlur}
                    error={formik.touched.email && Boolean(formik.errors.email)}
                    helperText={formik.touched.email && formik.errors.email}
                    icon={<Mail className="w-4 h-4" />}
                />
            )}

            <InputText
                name="password"
                label="Пароль"
                type="password"
                autoComplete={mode === 'login' ? 'current-password' : 'new-password'}
                value={formik.values.password}
                handleChange={formik.handleChange}
                handleBlur={formik.handleBlur}
                error={formik.touched.password && Boolean(formik.errors.password)}
                helperText={formik.touched.password && formik.errors.password}
                icon={<Lock className="w-4 h-4" />}
            />

            {mode === 'register' && (
                <InputText
                    name="passwordConfirm"
                    label="Повторите пароль"
                    type="password"
                    autoComplete="new-password"
                    value={formik.values.passwordConfirm}
                    handleChange={formik.handleChange}
                    handleBlur={formik.handleBlur}
                    error={formik.touched.passwordConfirm && Boolean(formik.errors.passwordConfirm)}
                    helperText={formik.touched.passwordConfirm && formik.errors.passwordConfirm}
                    icon={<Lock className="w-4 h-4" />}
                />
            )}

            {error && (
                <div className="text-danger text-sm text-center bg-danger/10 border border-danger/20 rounded-lg p-2">
                    {String(error)}
                </div>
            )}

            <Button
                type="submit"
                disabled={isLoading}
                icon={mode === 'login' ? LogIn : UserPlus}
                className="mt-2"
            >
                {isLoading ? 'Загрузка...' : mode === 'login' ? 'Войти' : 'Зарегистрироваться'}
            </Button>
        </form>
    );

    const renderFooter = () => (
        <Text variant="glass-dim" size="xs" className="mt-2">
            {mode === 'login'
                ? 'Введите свои данные для входа'
                : 'Создайте аккаунт, чтобы начать игру'}
        </Text>
    );

    const renderContent = () => (
        <div className='h-full overflow-y-auto scrollbar-hide'>
            <div className="flex flex-l items-center justify-center w-full min-h-full py-2">
                <div className="flex items-center justify-center w-full h-full px-4">
                    <Card variant="glow" className="flex flex-col items-center max-w-md w-full">
                        {renderHeader()}
                        {renderTitle()}
                        {renderForm()}
                        {renderFooter()}
                    </Card>
                </div>
            </div>
        </div>
    );

    return (
        <Page backgroundImage='city_in_space' isLoading={isLoading} error={error}>
            {renderContent()}
        </Page>
    );
};

export default RegistrationPage;