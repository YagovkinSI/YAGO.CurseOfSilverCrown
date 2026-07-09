import YagoSlide from '../shared/YagoSlide';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { useEffect } from 'react';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useGetMyUserQuery } from '../entities/MyUser';
import YagoButton from '../shared/YagoButton';

const DevelopingPage: React.FC = () => {
    const navigate = useNavigate();
    const myUserDataResult = useGetMyUserQuery();

    const isLoading = myUserDataResult.isLoading;
    const error = myUserDataResult.error;

    useEffect(() => {
        if (!(myUserDataResult.data?.data != undefined)) {
            navigate('/registration');
        }
    }, [myUserDataResult, navigate]);

    const renderDescription = () => (
        <p className="text-justify text-light/90 text-base mb-4">
            Данный раздел ещё находится в разработке.
        </p>
    );

    const renderCard = () => (
        <YagoSlide
            title="В разработке"
            image="/assets/images/pictures/homepage.jpg"
        >
            <div className="flex flex-col gap-4 items-center">
                {renderDescription()}
                <YagoButton onClick={() => navigate(-1)} variant="secondary">
                    Закрыть
                </YagoButton>
            </div>
        </YagoSlide>
    );

    const renderContent = () => {
        if (isLoading) {
            return <LoadingCard />;
        }
        if (error != undefined) {
            return <DefaultErrorCard />;
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

export default DevelopingPage;