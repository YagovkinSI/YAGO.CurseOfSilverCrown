import { Box, Button, TextField, ToggleButton, ToggleButtonGroup } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import React, { useState } from 'react';
import * as Yup from 'yup';
import { useFormik } from 'formik';
import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import { useLoginMutation, useRegisterMutation } from '../entities/CurrentUser';
import LoadingCard from '../shared/LoadingCard';

interface ILoginRegisterProps {
    isLogin: boolean
}

const RegistrationPage: React.FC<ILoginRegisterProps> = (props) => {
    const [isLogin, setIsLogin] = useState(props.isLogin);
    const navigate = useNavigate();

    const [loginMutate, { data: loginData, isLoading: isLoginLoading, error: loginError }] = useLoginMutation();
    const [registerMutate, { data: registerData, isLoading: isRegisterLoading, error: registerError }] = useRegisterMutation();
    const data = isLogin ? loginData : registerData;
    const isLoading = isLogin ? isLoginLoading : isRegisterLoading;
    const error = isLogin ? loginError : registerError;

    const name = isLogin ? 'Вход' : 'Регистрация';

    React.useEffect(() => {
        if (data?.isAuthorized) {
            navigate('/');
        }
    }, [data, navigate]);

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
            const mutate = isLogin ? loginMutate : registerMutate;
            mutate(values);
        },
    });

    const loginInput = () => {
        return (
            <TextField
                margin="normal"
                required
                fullWidth
                id="userName"
                label="Логин"
                name="userName"
                autoComplete="userName"
                autoFocus
                value={formik.values.userName}
                onChange={formik.handleChange}
                onBlur={formik.handleBlur}
                error={formik.touched.userName && Boolean(formik.errors.userName)}
                helperText={formik.touched.userName && formik.errors.userName}

            />
        )
    }

    const passwordInput = () => {
        return (
            <TextField
                margin="normal"
                required
                fullWidth
                name="password"
                label="Введите пароль"
                type="password"
                id="password"
                autoComplete="current-password"
                value={formik.values.password}
                onChange={formik.handleChange}
                onBlur={formik.handleBlur}
                error={formik.touched.password && Boolean(formik.errors.password)}
                helperText={formik.touched.password && formik.errors.password}
            />
        )
    }

    const confirmPasswordInput = () => {
        return (
            <TextField
                margin="normal"
                required
                fullWidth
                name="passwordConfirm"
                label="Повторите пароль"
                type="password"
                id="passwordConfirm"
                autoComplete="current-password"
                value={formik.values.passwordConfirm}
                onChange={formik.handleChange}
                onBlur={formik.handleBlur}
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
                <ToggleButton value="login" style={{width: '132px'}}>Вход</ToggleButton>
                <ToggleButton value="registation" style={{width: '132px'}}>Регистрация</ToggleButton>
            </ToggleButtonGroup>
        )
    }

    const renderCard = () => {
        return (
            <YagoCard 
                title={isLogin ? "Вход" : "Регистрация"}
                image={undefined} 
            >
                {toggleForm()}
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