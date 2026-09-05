import React from 'react';
import { useNavigate } from 'react-router-dom';
import { ArrowLeft, Search } from 'lucide-react';
import PageIllustration from '../shared/ui/PageIllustration';
import PageHeader from '../features/PageHeader';
import Page from '../widgets/Page';
import { FlexContainer } from '../shared/ui/FlexContainer';
import Surface from '../shared/ui/Surface';
import CouncilPositionCard from '../entities/council/CouncilPositionCard';
import { useGetCouncilPositionsQuery } from '../entities/council/council.api';

const CouncilPage: React.FC = () => {
    const navigate = useNavigate();

    const councilResult = useGetCouncilPositionsQuery();
    const positions = councilResult.data ?? [];

    const renderIllustration = () => (
        <PageIllustration
            image="/images/pictures/captain_hall.jpg"
            title="Совет станции"
            subtitle="Ваши советники и их полномочия"
        />
    );

    const renderCouncilList = () => (
        <div className="flex flex-col gap-2">
            {positions.map((position) => (
                <CouncilPositionCard key={position.code} position={position} />
            ))}
        </div>
    );

    const renderContent = () => (
        <div className='h-full overflow-y-auto scrollbar-hide'>
            <FlexContainer justify='start'>
                <div className="w-full max-w-2xl mx-auto px-4 py-4">
                    <PageHeader
                        title={'Совет станции'}
                        leftButton={{ icon: ArrowLeft, onClick: () => navigate(-1), label: 'Назад' }}
                        rightButton={{ icon: Search, onClick: () => undefined, disabled: true }}
                    />
                    {renderIllustration()}
                    <Surface rounded='md' variant='default' className='w-full p-3 flex flex-col gap-2'>
                        {renderCouncilList()}
                    </Surface>
                </div>
            </FlexContainer>
        </div>
    );

    return (
        <Page backgroundImage='captain_hall' darkenBackground isLoading={councilResult.isLoading} error={councilResult.error}>
            {renderContent()}
        </Page>
    );
};

export default CouncilPage;