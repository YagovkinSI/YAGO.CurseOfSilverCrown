import React, { useEffect } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import Page from '../widgets/Page';
import type { Slide } from '../entities/events/colonyEvent.types';
import type { StatisticFieldInfo } from '../entities/statistics/statistics.types';
import SlideRenderer from '../widgets/SlideRenderer';

const StatisticInfoPage: React.FC = () => {
    const navigate = useNavigate();
    const location = useLocation();
    const info = (location.state as { info?: StatisticFieldInfo } | null)?.info;

    useEffect(() => {
        if (!info) navigate('/me/statistics', { replace: true });
    }, [info, navigate]);

    if (!info) return null;

    const slide: Slide = {
        id: 'statistic-info',
        title: info.name,
        imageName: info.imageName ?? '',
        text: info.description,
        visibleEffects: [],
        requirements: [],
        buttons: [],
    };

    const renderContent = () => {
        return (
            <SlideRenderer slide={slide} />
        )
    }

    return (
        <Page backgroundImage='captain_hall' darkenBackground isLoading={false} error={undefined}>
            {renderContent()}
        </Page>
    );
};

export default StatisticInfoPage;
