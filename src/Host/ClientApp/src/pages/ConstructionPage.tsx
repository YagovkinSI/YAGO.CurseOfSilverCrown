import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
    Building2,
    ArrowLeft,
    Search,
} from 'lucide-react';
import { useBuildMutation, useGetBuildingsQuery, type MyBuilding } from '../entities/buildings/MyBuilding';
import Text from '../shared/ui/Text';
import Page from '../widgets/Page';
import PageHeader from '../features/PageHeader';
import { FlexContainer } from '../shared/ui/FlexContainer';
import PageIllustration from '../shared/ui/PageIllustration';
import Surface from '../shared/ui/Surface';
import ResultSlideRenderer from '../entities/events/ResultSlideRenderer';
import BuildingCard from '../features/buildings/BuildingCard';
import BuildToggle from '../features/buildings/BuildToggle';
import BuildConfirmModal from '../features/buildings/BuildConfirmModal';

const ConstructionPage: React.FC = () => {
    const navigate = useNavigate();
    const { data: buildings, isLoading: getBuildingsLoading, error } = useGetBuildingsQuery();
    const [buildMutation, useBuildResult] = useBuildMutation();
    const [expandedBuilding, setExpandedBuilding] = useState<string | null>(null);
    const [buildingToBuild, setBuildingToBuild] = useState<{ building: MyBuilding; isPrivate: boolean } | null>(null);
    const [showConfirmModal, setShowConfirmModal] = useState(false);
    const [showBuildResult, setShowBuildResult] = useState(false);
    const [isPrivate, setIsPrivate] = useState(false);

    const isLoading = getBuildingsLoading || useBuildResult.isLoading;
    const eventResultSlide = useBuildResult.data?.data;

    const handleBuild = async (building: MyBuilding, isPrivate: boolean) => {
        try {
            await buildMutation({ buildType: building.type, isPrivate }).unwrap();
            setShowConfirmModal(false);
            setBuildingToBuild(null);
            setShowBuildResult(true);
        } catch (err) {
            console.error('Build failed:', err);
        }
    };

    const openConfirmModal = (building: MyBuilding, isPrivate: boolean) => {
        setBuildingToBuild({ building, isPrivate });
        setShowConfirmModal(true);
    };

    const toggleExpand = (buildingType: string) => {
        setExpandedBuilding(prev => prev === buildingType ? null : buildingType);
    };

    const handleCloseResult = () => setShowBuildResult(false);
    const handleCloseModal = () => setShowConfirmModal(false);

    // ============================================
    // Иллюстрация
    // ============================================
    const renderIllustration = () => (
        <PageIllustration
            image="/images/pictures/empty_hangar.jpg"
            title="Меню строительства"
            subtitle={isPrivate
                ? 'Частные постройки требуют меньше вложений, но приносят меньше дохода в бюджет.'
                : 'Бюджетные постройки управляются колонией и приносят стабильный доход.'}
        />
    );

    // ============================================
    // Список построек
    // ============================================
    const renderBuildingsList = () => {
        if (!buildings || buildings.length === 0) {
            return (
                <div className="flex flex-col items-center justify-center py-8 text-muted/50">
                    <Building2 className="w-12 h-12 mb-2" />
                    <Text variant="secondary" size="sm">Нет доступных построек</Text>
                </div>
            );
        }
        return (
            <div className="space-y-2">
                {buildings.map((building) => (
                    <BuildingCard
                        key={building.type}
                        building={building}
                        isPrivate={isPrivate}
                        isExpanded={expandedBuilding === building.type}
                        onToggleExpand={toggleExpand}
                        onBuildClick={openConfirmModal}
                        isBuilding={isLoading}
                    />
                ))}
            </div>
        );
    };

    // ============================================
    // Загрузка ещё
    // ============================================
    const renderLoadMore = () => {
        if ((buildings?.length ?? 0) <= 10) return null;
        return (
            <button className="w-full py-3 mt-4 text-sm text-muted hover:text-light transition-colors border border-bright/10 rounded-lg hover:bg-bright/5">
                Загрузить ещё
            </button>
        );
    };

    // ============================================
    // Основной контент
    // ============================================
    const renderBaseContent = () => (
        <div className="h-full overflow-y-auto scrollbar-hide">
            <FlexContainer justify="start">
                <div className="w-full max-w-2xl mx-auto px-4 py-4">
                    <PageHeader
                        title="Строительство"
                        leftButton={{ icon: ArrowLeft, onClick: () => navigate(-1), label: 'Назад' }}
                        rightButton={{ icon: Search, onClick: () => undefined, disabled: true }}
                    />
                    {renderIllustration()}
                    <BuildToggle isPrivate={isPrivate} onToggle={setIsPrivate} />
                    <Surface rounded="md" variant="default" className="w-full p-3 flex flex-col gap-2">
                        {renderBuildingsList()}
                    </Surface>
                    {renderLoadMore()}
                </div>
            </FlexContainer>
        </div>
    );

    // ============================================
    // Рендер
    // ============================================
    const renderContent = () => {
        if (showBuildResult && eventResultSlide) {
            return <ResultSlideRenderer eventResult={eventResultSlide} onClose={handleCloseResult} />;
        }

        return (
            <>
                {renderBaseContent()}
                <BuildConfirmModal
                    isOpen={showConfirmModal}
                    building={buildingToBuild?.building}
                    isPrivate={buildingToBuild?.isPrivate ?? false}
                    isLoading={isLoading}
                    onConfirm={handleBuild}
                    onClose={handleCloseModal}
                />
            </>
        );
    };

    return (
        <Page backgroundImage="captain_hall" darkenBackground isLoading={isLoading} error={error}>
            {renderContent()}
        </Page>
    );
};

export default ConstructionPage;