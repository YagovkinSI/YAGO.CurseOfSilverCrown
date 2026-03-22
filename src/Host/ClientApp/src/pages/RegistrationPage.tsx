import { Box, Button, ToggleButton, ToggleButtonGroup } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import React, { useState } from 'react';
import * as Yup from 'yup';
import { useFormik } from 'formik';
import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import YagoTextField from '../shared/YagoTextField';
import { useConvertToPermanentUserMutation, useGetMyUserQuery, useLoginMutation, useRegisterMutation } from '../entities/MyUser';

interface ILoginRegisterProps {
    isLogin: boolean
}

const RegistrationPage: React.FC<ILoginRegisterProps> = (props) => {
    const [isLogin, setIsLogin] = useState(props.isLogin);
    const myUserDataResult = useGetMyUserQuery();
    const isAuthorized = myUserDataResult.data?.data != undefined;
    const navigate = useNavigate();

    const [loginMutate, loginMutateResult] = useLoginMutation();
    const [registerMutate, registerMutateResult] = useRegisterMutation();
    const [convertToPermanentUser, convertToPermanentUserResult] = useConvertToPermanentUserMutation();

    const isLoading = myUserDataResult.isLoading || loginMutateResult.isLoading || registerMutateResult.isLoading || convertToPermanentUserResult.isLoading;
    const error = myUserDataResult.error ?? loginMutateResult.error ?? registerMutateResult.error ?? convertToPermanentUserResult.error;

    const name = isAuthorized
        ? 'Изменить'
        : isLogin
            ? 'Вход'
            : 'Регистрация';

    React.useEffect(() => {
        if (isAuthorized) {
            setIsLogin(false);
        }
    }, [myUserDataResult, isAuthorized]);

    const validationSchema = Yup.object().shape({
        userName: Yup.string()
            .required('Введите логин')
            .min(3, 'Логин должен содержать не менее 3 символов')
            .max(20, 'Логин должен содержать не более 20 символов')
            .matches(/^[a-zA-Z0-9_-]+$/, 'Логин может содержать только латинские буквы, цифры, подчеркивание (_) и дефис (-)')
            .matches(/[a-zA-Z]/, 'Логин должен содержать хотя бы одну латинскую букву'),
        password: Yup.string()
            .required('Введите пароль')
            .min(6, 'Пароль должен содержать не менее 6 символов')
            .max(20, 'Пароль должен содержать не более 20 символов')
            .matches(/[a-z]/, 'Пароль должен содержать строчную латинскую букву')
            .matches(/[A-Z]/, 'Пароль должен содержать заглавную латинскую букву')
            .matches(/[0-9]/, 'Пароль должен содержать цифру')
            .matches(/^[a-zA-Z0-9!@#$%^&*()\-_=+[\]{};:,./?~`"']+$/, 'Пароль содержит недопустимые символы'),
        passwordConfirm: isLogin
            ? Yup.string()
            : Yup.string()
                .required('Введите пароль ещё раз')
                .oneOf([Yup.ref('password'), ''], 'Пароли не совпадают'),
    })

    const formik = useFormik({
        initialValues: {
            userName: '',
            password: '',
            passwordConfirm: '',
        },
        validationSchema: validationSchema,
        onSubmit: (values) => {
            const mutate =
                isAuthorized
                    ? convertToPermanentUser
                    : isLogin
                        ? loginMutate
                        : registerMutate;
            mutate(values)
                .unwrap()
                .then(() => navigate('/'));
        },
    });

    const loginInput = () => {
        return (
            <YagoTextField
                label="Логин"
                name="userName"
                autoComplete="userName"
                autoFocus
                value={formik.values.userName}
                handleChange={formik.handleChange}
                handleBlur={formik.handleBlur}
                error={formik.touched.userName && Boolean(formik.errors.userName)}
                helperText={formik.touched.userName && formik.errors.userName}

            />
        )
    }

    const passwordInput = () => {
        return (
            <YagoTextField
                name="password"
                label="Введите пароль"
                type="password"
                autoComplete="current-password"
                value={formik.values.password}
                handleChange={formik.handleChange}
                handleBlur={formik.handleBlur}
                error={formik.touched.password && Boolean(formik.errors.password)}
                helperText={formik.touched.password && formik.errors.password}
            />
        )
    }

    const confirmPasswordInput = () => {
        return (
            <YagoTextField
                name="passwordConfirm"
                label="Повторите пароль"
                type="password"
                autoComplete="current-password"
                value={formik.values.passwordConfirm}
                handleChange={formik.handleChange}
                handleBlur={formik.handleBlur}
                error={formik.touched.passwordConfirm && Boolean(formik.errors.passwordConfirm)}
                helperText={formik.touched.passwordConfirm && formik.errors.passwordConfirm}
            />
        )
    }

    const renderForm = () => {
        return (
            <form onSubmit={formik.handleSubmit}>
                {loginInput()}
                {passwordInput()}
                {isLogin ? <></> : confirmPasswordInput()}
                <Button
                    type="submit"
                    fullWidth
                    variant="contained"
                    disabled={isLoading}
                    sx={{ mt: 3, mb: 2 }}
                >
                    {isLoading
                        ? 'Загрузка...'
                        : name}
                </Button>
            </form>
        )
    }

    const toggleForm = () => {
        return (
            <ToggleButtonGroup
                color="primary"
                value={isLogin ? 'login' : 'registation'}
                exclusive
                onChange={() => setIsLogin(!isLogin)}
                aria-label="Platform"
            >
                <ToggleButton value="login" style={{ width: '132px' }}>Вход</ToggleButton>
                <ToggleButton value="registation" style={{ width: '132px' }}>Регистрация</ToggleButton>
            </ToggleButtonGroup>
        )
    }

    const renderCard = () => {
        return (
            <YagoCard
                title={name}
                image={undefined}
            >
                {!isAuthorized && toggleForm()}
                <Box sx={{ mt: 1 }}>
                    {renderForm()}
                </Box>
            </YagoCard>
        )
    }

    return (
        <>
            <ErrorField title='Ошибка' error={error} />
            {isLoading
                ? <LoadingCard />
                : renderCard()}
        </>
    )
};

export default RegistrationPage