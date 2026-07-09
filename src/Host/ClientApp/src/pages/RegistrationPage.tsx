import { useNavigate } from 'react-router-dom';
import React, { useState } from 'react';
import * as Yup from 'yup';
import { useFormik } from 'formik';
import YagoSlide from '../shared/YagoSlide';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import YagoTextField from '../shared/YagoTextField';
import { useConvertToPermanentUserMutation, useGetMyUserQuery, useLoginMutation, useRegisterMutation } from '../entities/MyUser';

interface ILoginRegisterProps {
    isLogin: boolean;
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
    });

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

    const renderLoginInput = () => (
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
    );

    const renderPasswordInput = () => (
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
    );

    const renderConfirmPasswordInput = () => (
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
    );

    const renderForm = () => (
        <form onSubmit={formik.handleSubmit} className="flex flex-col gap-2">
            {renderLoginInput()}
            {renderPasswordInput()}
            {!isLogin && renderConfirmPasswordInput()}
            <button
                type="submit"
                disabled={isLoading}
                className={`
                    w-full py-2.5 px-4 mt-3 mb-2
                    bg-bright/20 text-bright font-medium
                    border border-bright/30 rounded-md
                    hover:bg-bright/30 hover:border-bright/50
                    transition-all duration-200
                    focus:outline-none focus:ring-2 focus:ring-bright/50
                    disabled:opacity-50 disabled:cursor-not-allowed
                `}
            >
                {isLoading ? 'Загрузка...' : name}
            </button>
        </form>
    );

    const renderToggleButtons = () => (
        <div className="flex rounded-md overflow-hidden border border-bright/20">
            <button
                onClick={() => setIsLogin(true)}
                className={`
                    px-6 py-2 w-[132px] text-sm font-medium
                    transition-all duration-200
                    ${isLogin 
                        ? 'bg-bright/20 text-bright border-b-2 border-bright' 
                        : 'bg-transparent text-muted hover:text-light hover:bg-bright/5'
                    }
                `}
            >
                Вход
            </button>
            <button
                onClick={() => setIsLogin(false)}
                className={`
                    px-6 py-2 w-[132px] text-sm font-medium
                    transition-all duration-200
                    ${!isLogin 
                        ? 'bg-bright/20 text-bright border-b-2 border-bright' 
                        : 'bg-transparent text-muted hover:text-light hover:bg-bright/5'
                    }
                `}
            >
                Регистрация
            </button>
        </div>
    );

    const renderCard = () => (
        <YagoSlide title={name} image={undefined}>
            <div className="flex flex-col gap-4 items-center">
                {!isAuthorized && renderToggleButtons()}
                <div className="w-full mt-1">
                    {renderForm()}
                </div>
            </div>
        </YagoSlide>
    );

    const renderContent = () => {
        if (isLoading) {
            return <LoadingCard />;
        }
        return renderCard();
    };

    return (
        <>
            <ErrorField title="Ошибка" error={error} />
            {renderContent()}
        </>
    );
};

export default RegistrationPage;