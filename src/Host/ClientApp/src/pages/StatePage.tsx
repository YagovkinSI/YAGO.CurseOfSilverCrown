import YagoSlide from '../shared/YagoSlide';
import { useGetMyColonyQuery } from '../entities/MyColony';
import React, { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import YagoButton from '../shared/YagoButton';
import ColonyParameterList from '../features/ColonyParameterList';
import PageContainer from '../shared/PageContainer';

const StatePage: React.FC = () => {
    const myColonyResult = useGetMyColonyQuery();

    const isLoading = myColonyResult.isLoading;
    const error = myColonyResult.error;

    const navigate = useNavigate();

    useEffect(() => {
        if (myColonyResult.data != undefined && myColonyResult.data!.data == undefined) {
            navigate('/');
        }
    }, [navigate, myColonyResult]);

    const renderCardContent = () => {
        const colonyParameters = myColonyResult.data!.data!.colonyParameters
            .filter(x => x.parrentType != undefined);
        
        return (
            <div className="flex flex-col gap-1 w-full max-w-[350px] md:max-w-[700px] mx-auto">
                <ColonyParameterList items={colonyParameters} />
            </div>
        );
    };

    const renderContent = () => (
        <YagoSlide
            title={myColonyResult.data?.data?.name ?? '-'}
            image="/assets/images/pictures/captain_hall.jpg"
        >
            <div className="flex flex-col gap-4 items-center">
                {renderCardContent()}
                <YagoButton onClick={() => navigate(-1)} variant="secondary">
                    Закрыть
                </YagoButton>
            </div>
        </YagoSlide>
    );

    return (
        <PageContainer backgroundImage='space' isLoading={isLoading} error={error}>
            {renderContent()}
        </PageContainer>
    );
};

export default StatePage;