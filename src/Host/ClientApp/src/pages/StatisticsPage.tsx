import { useGetMyColonyQuery } from '../entities/MyColony';
import React, { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Page from '../widgets/Page';
import type { Slide } from '../entities/Episode';
import SlideRenderer from '../shared/SlideRenderer';

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

    const renderContent = () => {
        const colonyParameters = myColonyResult.data!.data!.colonyParameters
            .filter(x => x.parrentType != undefined);
        const slide: Slide = {
            id: `my-colony`,
            title: myColonyResult.data?.data?.name ?? '-',
            imageName: 'captain_hall',
            text: [],
            parameters: colonyParameters,
            buttons: []
        }
        return (
            <SlideRenderer
                slide={slide}
                onButtonClick={() => { }}
                onInfoSlideClick={() => { }}
                onSlideChange={() => { }}
                onNavigate={navigate}
                onClose={() => navigate(-1)}
            />
        )
    }

    return (
        <Page backgroundImage='space' darkenBackground isLoading={isLoading} error={error}>
            {renderContent()}
        </Page>
    );
};

export default StatisticsPage;