import React, { useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { ArrowLeft, Search, BarChart3 } from 'lucide-react';
import Text from '../shared/ui/Text';
import { useGetStatisticsQuery } from '../entities/statistics/statistics.api';
import PageHeader from '../features/PageHeader';
import Page from '../widgets/Page';
import { FlexContainer } from '../shared/ui/FlexContainer';
import Surface from '../shared/ui/Surface';
import PageIllustration from '../shared/ui/PageIllustration';
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

    const renderStatisticsList = () => {
        if (!statistics || statistics.fields.length === 0) {
            return (
                <div className="flex flex-col items-center justify-center py-12">
                    <BarChart3 className="w-12 h-12 text-muted/30" />
                    <Text variant="secondary" size="sm" className="mt-3">
                        Показателей пока нет
                    </Text>
                </div>
            );
        }

        return (
            <StatisticRowList fields={statistics.fields} maxWidth='full' dense />
        );
    };

    const renderContent = () => (
        <div className='h-full overflow-y-auto scrollbar-hide'>
            <FlexContainer justify='start'>
                <div className="w-full max-w-2xl mx-auto px-4 py-4">
                    <PageHeader
                        title={'Статистика'}
                        leftButton={{ icon: ArrowLeft, onClick: () => navigate(-1), label: 'Назад' }}
                        rightButton={{ icon: Search, onClick: () => undefined, disabled: true }}
                    />
                    <PageIllustration
                        image="/images/pictures/captain_hall.jpg"
                        title="Статистика колонии"
                        subtitle="Показатели состояния колонии"
                    />
                    <Surface rounded='md' variant='default' className='max-h-[60vh] w-full p-3 flex flex-col gap-2 overflow-y-auto'>
                        {renderStatisticsList()}
                    </Surface>
                </div>
            </FlexContainer>
        </div>
    );

    return (
        <Page backgroundImage='captain_hall' darkenBackground isLoading={isLoading} error={error}>
            {renderContent()}
        </Page>
    );
};

export default StatisticsPage;
