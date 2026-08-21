import { useGetMyColonyQuery } from '../entities/colonies/colony.api';
import React, { useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import Page from '../widgets/Page';
import ColonyParameterRowList from '../features/ColonyParameterList';
import type { StatMenu } from '../entities/colonies/colony.types';
import { FlexContainer } from '../shared/ui/FlexContainer';
import PageHeader from '../features/PageHeader';
import { X } from 'lucide-react';

const StatisticsPage: React.FC = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const myColonyResult = useGetMyColonyQuery();
    const scrollContainerRef = React.useRef<HTMLDivElement>(null);

    const isLoading = myColonyResult.isLoading;
    const error = myColonyResult.error;

    let statMenu: StatMenu = 'stats';
    switch (id) {
        case 'other':
            statMenu = 'other'
            break;
    }

    useEffect(() => {
        if (myColonyResult.data != undefined && myColonyResult.data!.data == undefined) {
            navigate('/');
        }
    }, [navigate, myColonyResult]);

    const colonyParameters = myColonyResult.data!.data!.colonyParameters
            .filter(x => x.statMenus?.includes(statMenu));
    const renderParameters = () => {
        if (!colonyParameters || colonyParameters.length === 0) return null;
        return (
            <div className="w-full">
                <ColonyParameterRowList items={colonyParameters ?? []} dense={true} />
            </div>
        );
    };

    const renderCentralPart = () => {
        return (
            <div className="min-h-full w-full max-w-3xl mx-auto bg-dark/40 backdrop-blur-sm border border-bright/5">
                <div className="relative w-full overflow-hidden">
                    <img
                        src={`/images/pictures/captain_hall.jpg`}
                        alt={myColonyResult.data?.data?.name ?? '-'}
                        className="w-full h-auto object-cover object-center"
                    />
                    <div className="absolute inset-0 bg-gradient-to-t from-dark/80 via-dark/20 to-transparent pointer-events-none" />
                </div>

                <div className="p-4">
                    {renderParameters()}
                </div>
            </div>
        );
    };

    const renderContent = () => (
        <FlexContainer className='h-full max-w-3xl mx-auto py-4 px-2 md:px-4 pb-2 md:pb-4'>
            <div className="w-full sticky top-0 flex-shrink-0 z-20 border-b border-bright/10">
                <PageHeader
                    title={myColonyResult.data?.data?.name ?? '-'}
                    leftButton={undefined}
                    rightButton={{ icon: X, onClick: () => navigate(-1), label: 'Закрыть' }}
                />
            </div>

            <div
                ref={scrollContainerRef}
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