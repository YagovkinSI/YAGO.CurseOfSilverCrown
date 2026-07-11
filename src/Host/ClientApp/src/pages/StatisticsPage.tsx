import SlideCard from '../shared/SlideCard';
import { useGetMyColonyQuery } from '../entities/MyColony';
import React, { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Button from '../shared/Button';
import ColonyParameterRowList from '../features/ColonyParameterList';
import Page from '../widgets/Page';

const StatisticsPage: React.FC = () => {
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
                <ColonyParameterRowList items={colonyParameters} />
            </div>
        );
    };

    const renderContent = () => (
        <div className="flex flex-l items-center justify-center w-full min-h-full py-2">
            <SlideCard
                title={myColonyResult.data?.data?.name ?? '-'}
                image="/images/pictures//captain_hall.jpg"
            >
                <div className="flex flex-col gap-4 items-center">
                    {renderCardContent()}
                    <Button onClick={() => navigate(-1)} variant="secondary">
                        Закрыть
                    </Button>
                </div>
            </SlideCard>
        </div>
    );

    return (
        <Page backgroundImage='space' isLoading={isLoading} error={error}>
            {renderContent()}
        </Page>
    );
};

export default StatisticsPage;