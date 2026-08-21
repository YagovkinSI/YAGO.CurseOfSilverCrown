import React, { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { ArrowLeft, Landmark, Search } from 'lucide-react';
import Text from '../shared/ui/Text';
import { useGetMyColonyQuery } from '../entities/colonies/colony.api';
import PageHeader from '../features/PageHeader';
import Page from '../widgets/Page';
import { FlexContainer } from '../shared/ui/FlexContainer';
import Surface from '../shared/ui/Surface';
import ReformCard from '../entities/reforms/ReformCard';
import { useGetReformsQuery } from '../entities/reforms/reform.api';

const ReformsPage: React.FC = () => {
    const navigate = useNavigate();

    const myColonyResult = useGetMyColonyQuery();
    const reformsResult = useGetReformsQuery();

    const isLoading = myColonyResult.isLoading || reformsResult.isLoading;
    const error = myColonyResult.error ?? reformsResult.error;

    const reforms = reformsResult.data ?? [];

    useEffect(() => {
        if (myColonyResult.data != undefined && myColonyResult.data.data == undefined) {
            navigate('/');
        }
    }, [myColonyResult, navigate]);

    const renderIllustration = () => (
        <div className="relative rounded-xl overflow-hidden h-32 md:h-48 mb-4">
            <img
                src="/images/pictures/register_colony.jpg"
                className="w-full h-full object-cover"
                alt="Реформы"
            />
            <div className="absolute inset-0 bg-gradient-to-t from-dark via-dark/50 to-transparent" />
            <div className="absolute bottom-4 left-4">
                <h2 className="text-lg font-bold text-light">Реформы колонии</h2>
                <p className="text-sm text-muted">Указы, меняющие жизнь колонии</p>
            </div>
        </div>
    );

    const renderReformsList = () => {
        if (reforms.length === 0) {
            return (
                <div className="flex flex-col items-center justify-center py-12">
                    <Landmark className="w-12 h-12 text-muted/30" />
                    <Text variant="secondary" size="sm" className="mt-3">
                        Реформ пока нет
                    </Text>
                </div>
            );
        }

        return (
            <div className="space-y-1">
                {reforms.map((reform) => <ReformCard key={reform.code} reform={reform} />)}
            </div>
        );
    };

    const renderContent = () => (
        <div className='h-full overflow-y-auto scrollbar-hide'>
            <FlexContainer justify='start'>
                <div className="w-full max-w-2xl mx-auto px-4 py-4">
                    <PageHeader
                        title={'Реформы'}
                        leftButton={{ icon: ArrowLeft, onClick: () => navigate(-1), label: 'Назад' }}
                        rightButton={{ icon: Search, onClick: () => undefined, disabled: true }}
                    />
                    {renderIllustration()}
                    <Surface rounded='md' variant='default' className='max-h-[60vh] w-full p-3 flex flex-col gap-2 overflow-y-auto'>
                        {renderReformsList()}
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

export default ReformsPage;