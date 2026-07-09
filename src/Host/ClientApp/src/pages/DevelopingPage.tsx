import YagoSlide from '../shared/YagoSlide';
import { useEffect } from 'react';
import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useGetMyUserQuery } from '../entities/MyUser';
import YagoButton from '../shared/YagoButton';
import PageContainer from '../shared/PageContainer';

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

    const renderContent = () => (
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

    return (
        <PageContainer backgroundImage='homepage' isLoading={isLoading} error={error}>
            {renderContent()}
        </PageContainer>
    );
};

export default DevelopingPage;