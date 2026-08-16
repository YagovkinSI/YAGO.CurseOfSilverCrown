import { useGetMyColonyQuery } from '../entities/colonies/colony.api';
import React, { useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import Page from '../widgets/Page';
import SlideRenderer from '../widgets/SlideRenderer';
import type { StatMenu } from '../entities/colonies/colony.types';
import type { Slide } from '../entities/events/colonyEvent.types';

const StatisticsPage: React.FC = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const myColonyResult = useGetMyColonyQuery();

    const isLoading = myColonyResult.isLoading;
    const error = myColonyResult.error;

    let statMenu : StatMenu = 'stats';
    switch (id)  {
        case 'other':
            statMenu = 'other'
            break;
    }

    useEffect(() => {
        if (myColonyResult.data != undefined && myColonyResult.data!.data == undefined) {
            navigate('/');
        }
    }, [navigate, myColonyResult]);

    const renderContent = () => {
        if (myColonyResult.data?.data == undefined)
            return <></>;
        const colonyParameters = myColonyResult.data!.data!.colonyParameters
            .filter(x => x.statMenus?.includes(statMenu));
        const slide: Slide = {
            id: `my-colony`,
            title: myColonyResult.data?.data?.name ?? '-',
            imageName: 'captain_hall',
            text: [],
            parameters: colonyParameters,
            requirements: [],
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