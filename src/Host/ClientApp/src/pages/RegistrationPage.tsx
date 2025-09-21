import { Box, Button, ToggleButton, ToggleButtonGroup } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import React, { useState } from 'react';
import * as Yup from 'yup';
import { useFormik } from 'formik';
import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import YagoTextField from '../shared/YagoTextField';
import { useChangeRegistrationMutation, useGetAuthorizationDataQuery, useLoginMutation, useRegisterMutation } from '../entities/AuthorizationData';

interface ILoginRegisterProps {
    isLogin: boolean
}

const RegistrationPage: React.FC<ILoginRegisterProps> = (props) => {
    const [isLogin, setIsLogin] = useState(props.isLogin);
    const authorizationData = useGetAuthorizationDataQuery();
    const isChanging = authorizationData.data?.isAuthorized;
    const navigate = useNavigate();

    const [loginMutate, loginMutateResult] = useLoginMutation();
    const [registerMutate, registerMutateResult] = useRegisterMutation();
    const [changeRegistrationMutate, changeRegistrationMutateResult] = useChangeRegistrationMutation();

    const isLoading = authorizationData.isLoading || loginMutateResult.isLoading || registerMutateResult.isLoading || changeRegistrationMutateResult.isLoading;
    const error = authorizationData.error ?? loginMutateResult.error ?? registerMutateResult.error ?? changeRegistrationMutateResult.error;

    const name = isChanging
        ? 'Изменить'
        : isLogin
            ? 'Вход'
            : 'Регистрация';

    React.useEffect(() => {
        if (isChanging) {
            setIsLogin(false);
        }
    }, [authorizationData, isChanging]);

    const validationSchema = Yup.object().shape({
        userName: Yup.string()
            .required('Введите логин')
            .min(4, 'Логин должен содержать не менее 4 символов')
            .max(12, 'Логин должен содержать не более 12 символов')
            .matches(/^[a-zA-Z0-9]+$/, 'Логин может содержать только латинские буквы и цифры'),
        password: Yup.string()
            .required('Введите пароль')
            .min(6, 'Пароль должен содержать не менее 6 символов')
            .matches(/[a-z]/, 'Пароль должен содержать строчную латинскую букву')
            .matches(/[A-Z]/, 'Пароль должен содержать заглавную латинскую букву')
            .matches(/[0-9]/, 'Пароль должен содержать цифру'),
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
                isChanging
                    ? changeRegistrationMutate
                    : isLogin
                        ? loginMutate
                        : registerMutate;
            mutate(values)
                .unwrap()
                .then(() => isChanging ? navigate(-1) : navigate('/game'));
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
                {!isChanging && toggleForm()}
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