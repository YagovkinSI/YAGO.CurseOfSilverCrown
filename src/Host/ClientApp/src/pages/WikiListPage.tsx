import React, { useMemo, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { ArrowLeft, BookOpen, ChevronDown, Search } from 'lucide-react';
import Text from '../shared/ui/Text';
import PageIllustration from '../shared/ui/PageIllustration';
import { useGetMyColonyQuery } from '../entities/colonies/colony.api';
import PageHeader from '../features/PageHeader';
import Page from '../widgets/Page';
import { FlexContainer } from '../shared/ui/FlexContainer';
import Surface from '../shared/ui/Surface';
import WikiCard from '../entities/wiki/WikiCard';
import { useGetWikiSummariesQuery } from '../entities/wiki/wiki.api';
import type { WikiSummary } from '../entities/wiki/wiki.types';

const SECTION_ORDER: Record<string, number> = {
    life: 1,
    faction: 2,
    station: 3,
    gameplay: 4,
    history: 5,
};

const SECTION_NAMES: Record<string, string> = {
    station: 'Станции',
    life: 'Жизнь в Поясе',
    faction: 'Фракции',
    gameplay: 'Параметры',
    history: 'История',
};

interface SectionGroup {
    code: string;
    name: string;
    order: number;
    items: WikiSummary[];
}

const WikiListPage: React.FC = () => {
    const navigate = useNavigate();

    const myColonyResult = useGetMyColonyQuery();
    const wikiSummariesResult = useGetWikiSummariesQuery();

    const isLoading = myColonyResult.isLoading || wikiSummariesResult.isLoading;
    const error = myColonyResult.error ?? wikiSummariesResult.error;

    const summaries = useMemo(() => wikiSummariesResult.data ?? [], [wikiSummariesResult.data]);
    const [openSections, setOpenSections] = useState<Record<string, boolean>>({});

    const groups = useMemo<SectionGroup[]>(() => {
        const bySection = new Map<string, WikiSummary[]>();
        summaries.forEach((summary) => {
            const list = bySection.get(summary.section) ?? [];
            list.push(summary);
            bySection.set(summary.section, list);
        });

        return Array.from(bySection.entries())
            .map(([code, items]) => ({
                code,
                name: SECTION_NAMES[code] ?? code,
                order: SECTION_ORDER[code] ?? Number.MAX_SAFE_INTEGER,
                items: [...items].sort((a, b) => a.order - b.order),
            }))
            .sort((a, b) => a.order - b.order);
    }, [summaries]);

    const toggleSection = (code: string) => {
        setOpenSections((prev) => ({ ...prev, [code]: !(prev[code] ?? false) }));
    };

    const renderIllustration = () => (
        <PageIllustration
            image="/images/pictures/register_colony.jpg"
            title="Энциклопедия"
            subtitle="Знания о мире Пояса"
        />
    );

    const renderListEmpty = () => (
        <div className="flex flex-col items-center justify-center py-12">
            <BookOpen className="w-12 h-12 text-muted/30" />
            <Text variant="secondary" size="sm" className="mt-3">
                Статей пока нет
            </Text>
        </div>
    );

    const renderListGroupTitle = (group: SectionGroup, isOpen: boolean) => {
        const hasUnread = group.items.some((item) => !item.isRead);
        return (
            <button
                className="w-full flex items-center gap-2 px-3 py-2.5 cursor-pointer hover:bg-bright/5 transition-colors"
                onClick={() => toggleSection(group.code)}
                type="button"
            >
                <Text variant="primary" size="sm" className="font-medium flex-1 text-left">
                    {group.name}
                </Text>
                {!isOpen && hasUnread && (
                    <span className="flex-shrink-0 w-2 h-2 rounded-full bg-green-400" />
                )}
                <ChevronDown
                    className={`flex-shrink-0 w-4 h-4 text-muted/60 transition-transform ${isOpen ? 'rotate-180' : ''}`}
                />
            </button>
        );
    };

    const renderListGroup = (group: SectionGroup) => {
        const isOpen = openSections[group.code] ?? false;
        return (
            <Surface key={group.code} rounded='md' variant='default' className='overflow-hidden'>
                {renderListGroupTitle(group, isOpen)}
                {isOpen && (
                    <div className="flex flex-col gap-1 px-2 pb-2">
                        {group.items.map((summary) => (
                            <WikiCard key={summary.code} summary={summary} />
                        ))}
                    </div>
                )}
            </Surface>
        );
    };

    const renderList = () => {
        if (summaries.length === 0) { return renderListEmpty(); }

        return (
            <div className="space-y-2">
                {groups.map((group) => {
                    return renderListGroup(group);
                })}
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
                    <div className="w-full p-3 flex flex-col gap-2 overflow-y-auto">
                        {renderList()}
                    </div>
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
