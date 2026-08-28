import React, { useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import Page from '../widgets/Page';
import { FlexContainer } from '../shared/ui/FlexContainer';
import PageHeader from '../features/PageHeader';
import { X, ArrowLeft } from 'lucide-react';
import { useGetStatisticsQuery } from '../entities/statistics/statistics.api';
import StatisticRowList from '../entities/statistics/StatisticRowList';
import type { StatisticCode } from '../entities/statistics/statistics.types';

const StatisticsPage: React.FC = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const code = (id as StatisticCode) ?? 'Main';
    const statisticsResult = useGetStatisticsQuery(code);

    const isLoading = statisticsResult.isLoading;
    const error = statisticsResult.error;
    const statistics = statisticsResult.data;

    useEffect(() => {
        if (!isLoading && !error && statistics == undefined) {
            navigate('/');
        }
    }, [navigate, isLoading, error, statistics]);

    const renderParameters = () => {
        if (!statistics || statistics.fields.length === 0) return null;
        return (
            <div className="w-full">
                <StatisticRowList fields={statistics.fields} dense={true} />
            </div>
        );
    };

    const renderCentralPart = () => (
        <div className="min-h-full w-full max-w-3xl mx-auto bg-dark/40 backdrop-blur-sm border border-bright/5">
            <div className="relative w-full overflow-hidden">
                <img
                    src={`/images/pictures/captain_hall.jpg`}
                    alt={statistics?.title ?? '-'}
                    className="w-full h-auto object-cover object-center"
                />
                <div className="absolute inset-0 bg-gradient-to-t from-dark/80 via-dark/20 to-transparent pointer-events-none" />
            </div>

            <div className="p-4">
                {renderParameters()}
            </div>
        </div>
    );

    const renderContent = () => (
        <FlexContainer className='h-full max-w-3xl mx-auto py-4 px-2 md:px-4 pb-2 md:pb-4'>
            <div className="w-full sticky top-0 flex-shrink-0 z-20 border-b border-bright/10">
                <PageHeader
                    title={statistics?.title ?? '-'}
                    leftButton={{ icon: ArrowLeft, onClick: () => navigate(-1), label: 'Назад' }}
                    rightButton={{ icon: X, onClick: () => navigate(-1), label: 'Закрыть' }}
                />
            </div>

            <div
                className="flex-1 w-full overflow-y-auto scrollbar-hide z-10 relative"
            >
                {renderCentralPart()}
            </div>
        </FlexContainer>
    );

    return (
        <Page backgroundImage='space' darkenBackground isLoading={isLoading} error={error}>
            {renderContent()}
        </Page>
    );
};

export default StatisticsPage;
