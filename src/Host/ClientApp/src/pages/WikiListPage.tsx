import React from 'react';
import { useNavigate } from 'react-router-dom';
import { ArrowLeft, BookOpen, Search } from 'lucide-react';
import Text from '../shared/ui/Text';
import PageIllustration from '../shared/ui/PageIllustration';
import { useGetMyColonyQuery } from '../entities/colonies/colony.api';
import PageHeader from '../features/PageHeader';
import Page from '../widgets/Page';
import { FlexContainer } from '../shared/ui/FlexContainer';
import Surface from '../shared/ui/Surface';
import WikiCard from '../entities/wiki/WikiCard';
import { useGetWikiSummariesQuery } from '../entities/wiki/wiki.api';

const WikiListPage: React.FC = () => {
    const navigate = useNavigate();

    const myColonyResult = useGetMyColonyQuery();
    const wikiSummariesResult = useGetWikiSummariesQuery();

    const isLoading = myColonyResult.isLoading || wikiSummariesResult.isLoading;
    const error = myColonyResult.error ?? wikiSummariesResult.error;

    const summaries = wikiSummariesResult.data ?? [];

    const renderIllustration = () => (
        <PageIllustration
            image="/images/pictures/register_colony.jpg"
            title="Энциклопедия"
            subtitle="Знания о мире Пояса"
        />
    );

    const renderList = () => {
        if (summaries.length === 0) {
            return (
                <div className="flex flex-col items-center justify-center py-12">
                    <BookOpen className="w-12 h-12 text-muted/30" />
                    <Text variant="secondary" size="sm" className="mt-3">
                        Статей пока нет
                    </Text>
                </div>
            );
        }

        return (
            <div className="space-y-1">
                {summaries.map((summary) => <WikiCard key={summary.code} summary={summary} />)}
            </div>
        );
    };

    const renderContent = () => (
        <div className='h-full overflow-y-auto scrollbar-hide'>
            <FlexContainer justify='start'>
                <div className="w-full max-w-2xl mx-auto px-4 py-4">
                    <PageHeader
                        title={'Wiki'}
                        leftButton={{ icon: ArrowLeft, onClick: () => navigate(-1), label: 'Назад' }}
                        rightButton={{ icon: Search, onClick: () => undefined, disabled: true }}
                    />
                    {renderIllustration()}
                    <Surface rounded='md' variant='default' className='max-h-[60vh] w-full p-3 flex flex-col gap-2 overflow-y-auto'>
                        {renderList()}
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

export default WikiListPage;
