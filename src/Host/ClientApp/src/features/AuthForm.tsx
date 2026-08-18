import React from 'react';
import { useNavigate } from 'react-router-dom';
import { User, Lock, LogIn, UserPlus, Mail } from 'lucide-react';
import { useFormik } from 'formik';
import * as Yup from 'yup';
import InputText from '../shared/ui/InputText';
import { useConvertToPermanentUserMutation, useLoginMutation, useRegisterMutation } from "../entities/users/user.api";
import Button from '../shared/ui/buttons/Button';

export type AuthMode = 'login' | 'register' | 'convert';

export interface AuthFormProps {
    mode: AuthMode
}

const AuthForm: React.FC<AuthFormProps> = ({ mode }) => {
    const navigate = useNavigate();
    const [loginMutate, loginMutateResult] = useLoginMutation();
    const [registerMutate, registerMutateResult] = useRegisterMutation();
    const [convertMutate, convertMutateResult] = useConvertToPermanentUserMutation();

    const isLoading = loginMutateResult.isLoading || registerMutateResult.isLoading || convertMutateResult.isLoading;
    const error = loginMutateResult.error  ?? registerMutateResult.error ?? convertMutateResult.error;

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

    const getMutatte = () => {
        switch (mode) {
            case 'login':
                return loginMutate;
            case 'register':
                return registerMutate;
            case 'convert':
                return convertMutate;
        }
    }

    const formik = useFormik({
        initialValues: {
            userName: '',
            email: '',
            password: '',
            passwordConfirm: '',
        },
        validationSchema,
        onSubmit: (values) => {
            const mutate = getMutatte();
            mutate(values)
                .unwrap()
                .then(() => navigate(-1))
                .catch((err) => {
                    console.error('Auth failed:', err);
                });
        },
    });

    const renderLogin = () => (
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
    )

    const renderEmail = () => (
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
    )

    const renderPassword = () => (
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
    )

    const renderConfirmPassword = () => (
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
    )

    const getButtonName = () => {
        switch (mode)
        {
            case 'login': return 'Войти'
            case 'register': return 'Зарегистрироваться'
            case 'convert': return 'Сохранить'
        }
    } 

    return (
        <form onSubmit={formik.handleSubmit} className="flex flex-col gap-4 w-full">
            {renderLogin()}
            {mode !== 'login' && renderEmail()}
            {renderPassword()}
            {mode !== 'login' && renderConfirmPassword()}
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
                {isLoading ? 'Загрузка...' : getButtonName()}
            </Button>
        </form>
    );
};

export default AuthForm;