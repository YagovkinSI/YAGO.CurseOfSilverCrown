import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
    Building2,
    ArrowLeft,
    Search,
    HelpCircle,
    CheckCircle,
    Lock,
    X,
} from 'lucide-react';
import { useBuildMutation, useGetBuildingsQuery, type MyBuilding } from '../entities/buildings/MyBuilding';
import Title from '../shared/ui/Title';
import Text from '../shared/ui/Text';
import Button from '../shared/ui/buttons/Button';
import Page from '../widgets/Page';
import PageHeader from '../features/PageHeader';
import { FlexContainer } from '../shared/ui/FlexContainer';
import Surface from '../shared/ui/Surface';
import Card from '../shared/ui/Card';
import ResultSlideRenderer from '../entities/events/ResultSlideRenderer';
import { IsDesktop } from '../features/MediaHelper';

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
    const isDesktop = IsDesktop();

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

    const renderToggle = () => {
        return (
            <Surface rounded="md" variant="default" className="w-full mb-3">
                <div className="flex items-center gap-2">
                    <div className="flex bg-dark border border-bright/15 rounded-lg p-0.5 flex-1">
                        <button
                            onClick={() => setIsPrivate(false)}
                            className={`
                            flex-1 py-1.5 px-3 rounded-md text-sm font-medium transition-colors
                            ${isPrivate
                                    ? 'text-muted hover:text-light'
                                    : 'bg-bright text-dark'
                                }
                        `}
                        >
                            Бюджетные
                        </button>
                        <button
                            onClick={() => setIsPrivate(true)}
                            className={`
                            flex-1 py-1.5 px-3 rounded-md text-sm font-medium transition-colors
                            ${isPrivate
                                    ? 'bg-bright text-dark'
                                    : 'text-muted hover:text-light'
                                }
                        `}
                        >
                            Частные
                        </button>
                    </div>
                </div>
            </Surface>
        );
    };

    // ============================================
    // Модалка подтверждения
    // ============================================
    const renderConfirmModal = () => {
        if (!buildingToBuild || !showConfirmModal) return null;

        const { building, isPrivate } = buildingToBuild;
        const buildingData = isPrivate ? building.private : building.state;
        const isAvailable = buildingData.buildAvailable;
        const cost = buildingData.cost;

        return (
            <div className="fixed inset-0 z-[2000] flex items-center justify-center bg-dark/80 backdrop-blur-sm p-4">
                <Card variant="glow" className="max-w-md w-full">
                    <div className="flex items-center justify-between mb-4">
                        <Title size="h2">Подтверждение</Title>
                        <button
                            onClick={() => setShowConfirmModal(false)}
                            className="text-muted hover:text-light transition-colors p-1"
                        >
                            <X className="w-5 h-5" />
                        </button>
                    </div>

                    <div className="space-y-4">
                        <Text variant="secondary" size="sm" align="left">
                            Построить <span className="text-light font-medium">{building.name}</span>
                            {' '}({isPrivate ? 'частную' : 'бюджетную'})?
                        </Text>

                        <div className="bg-dark/50 border border-bright/10 rounded-lg p-3 space-y-1.5">
                            <div className="flex justify-between text-sm">
                                <span className="text-muted">Тип</span>
                                <span className="text-light">{isPrivate ? 'Частная' : 'Бюджетная'}</span>
                            </div>
                            <div className="flex justify-between text-sm">
                                <span className="text-muted">Стоимость</span>
                                <span className="text-bright font-medium">{cost} SOL</span>
                            </div>
                            <div className="flex justify-between text-sm">
                                <span className="text-muted">Уже построено</span>
                                <span className="text-light">{buildingData.buildingCount}</span>
                            </div>
                            <div className="flex justify-between text-sm">
                                <span className="text-muted">Статус</span>
                                <span className={isAvailable ? 'text-good' : 'text-danger'}>
                                    {isAvailable ? 'Доступно' : buildingData.unavailabilityReason || 'Недоступно'}
                                </span>
                            </div>
                        </div>

                        <div className="flex gap-2">
                            <Button
                                variant="primary"
                                onClick={() => handleBuild(building, isPrivate)}
                                disabled={!isAvailable || isLoading}
                                className="flex-1"
                            >
                                {isLoading ? 'Строительство...' : 'Построить'}
                            </Button>
                            <Button
                                variant="secondary"
                                onClick={() => setShowConfirmModal(false)}
                            >
                                Отмена
                            </Button>
                        </div>
                    </div>
                </Card>
            </div>
        );
    };

    const renderIllustration = () => (
        <div className="relative rounded-xl overflow-hidden h-32 md:h-48 mb-4">
            <img
                src="/images/pictures/empty_hangar.jpg"
                className="w-full h-full object-cover"
                alt="События"
            />
            <div className="absolute inset-0 bg-gradient-to-t from-dark via-dark/50 to-transparent" />
            <div className="absolute bottom-4 left-4">
                <h2 className="text-lg font-bold text-light">Меню строительства</h2>
                <p className="text-sm text-muted">Частные постройки требуют меньше вложений, но приносят меньше дохода в бюджет.</p>
            </div>
        </div>
    );

    // ============================================
    // Рендер кнопки строительства
    // ============================================
    const renderBuildButton = (building: MyBuilding) => {
        const buildingData = isPrivate ? building.private : building.state;
        const isAvailable = buildingData.buildAvailable;
        const cost = buildingData.cost;

        return (
            <div className="flex items-center gap-2">
                <Button
                    variant={isAvailable ? 'primary' : 'secondary'}
                    sizeSm="sm"
                    sizeMd="md"
                    onClick={() => openConfirmModal(building, isPrivate)}
                    disabled={!isAvailable || isLoading}
                    className="flex-1 justify-start text-left"
                >
                    <div className={
                        `flex items-start w-full 
                        ${isDesktop ? 'flex-row' : 'flex-col'}`}
                    >
                        <span className="text-xs font-medium">
                            {isPrivate ? '👤 Частная' : '🏛️ Бюджетная'}
                        </span>
                        <span className="text-[0.6rem] opacity-70">
                            {isAvailable
                                ? `💰 ${cost} SOL`
                                : `🔒 ${buildingData.unavailabilityReason || 'Недоступно'}`
                            }
                        </span>
                    </div>
                </Button>
                <button
                    onClick={() => console.log('info', building, isPrivate)}
                    className="flex-shrink-0 w-9 h-9 rounded-lg border border-bright/20 
                        text-muted hover:text-light hover:border-bright/40 transition-colors flex items-center justify-center"
                    aria-label="Подробнее"
                >
                    <HelpCircle className="w-4 h-4" />
                </button>
            </div>
        );
    };

    // ============================================
    // Рендер карточки здания
    // ============================================
    const renderBuildingCard = (building: MyBuilding) => {
        const isExpanded = expandedBuilding === building.type;
        const isAvailable = building.state.buildAvailable || building.private.buildAvailable;
        const totalBuilt = building.state.buildingCount + building.private.buildingCount;

        return (
            <Surface
                key={building.type}
                rounded="md"
                variant="default"
                className={`
                    w-full p-3 gap-2 transition-all duration-200
                    ${isAvailable ? 'hover:border-bright/30' : 'opacity-70'}
                    ${isExpanded ? 'border-bright/30' : ''}
                `}
            >
                {/* Шапка карточки — кликабельная для разворачивания */}
                <div
                    className="flex items-start justify-between cursor-pointer"
                    onClick={() => toggleExpand(building.type)}
                >
                    <div className="flex-1 min-w-0">
                        <div className="flex items-center gap-2">
                            <Title size="h3" className="text-light truncate">
                                {building.name}
                            </Title>
                            {isAvailable ? (
                                <span className="text-[0.55rem] text-good flex items-center gap-0.5 flex-shrink-0">
                                    <CheckCircle className="w-3 h-3" />
                                </span>
                            ) : (
                                <span className="text-[0.55rem] text-muted flex items-center gap-0.5 flex-shrink-0">
                                    <Lock className="w-3 h-3" />
                                </span>
                            )}
                        </div>
                        <div className="flex items-center gap-3 mt-0.5 text-xs text-muted/50">
                            <span className="flex items-center gap-1">
                                <Building2 className="w-3 h-3" />
                                {totalBuilt} шт. (частных: {building.private.buildingCount}, бюджетных: {building.state.buildingCount})
                            </span>
                        </div>
                    </div>
                    <div className="text-muted/30 text-sm flex-shrink-0 ml-2">
                        {isExpanded ? '▲' : '▼'}
                    </div>
                </div>

                {/* Описание (показывается при разворачивании) */}
                {isExpanded && (
                    <div className="pt-2 border-t border-bright/10 space-y-2">
                        <div className="space-y-0.5">
                            {building.description.map((line, idx) => (
                                <Text key={idx} size="xs" variant="secondary" align="left" className="leading-relaxed">
                                    • {line}
                                </Text>
                            ))}
                        </div>

                        <div className="flex flex-col gap-1.5 pt-1">
                            {renderBuildButton(building)}
                        </div>
                    </div>
                )}
            </Surface>
        );
    };

    // ============================================
    // Рендер списка
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
                {buildings.map((building) => renderBuildingCard(building))}
            </div>
        );
    };

    // ============================================
    // Основной рендер
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
                    {renderToggle()}

                    <Surface rounded="md" variant="default" className="w-full p-3 flex flex-col gap-2">
                        {renderBuildingsList()}
                    </Surface>

                    {(buildings?.length ?? 0) > 10 && (
                        <button className="w-full py-3 mt-4 text-sm text-muted hover:text-light transition-colors border border-bright/10 rounded-lg hover:bg-bright/5">
                            Загрузить ещё
                        </button>
                    )}
                </div>
            </FlexContainer>
        </div>
    );

    const renderContent = () => {
        return showBuildResult && eventResultSlide != undefined
            ? <ResultSlideRenderer
                eventResult={eventResultSlide!}
                onClose={() => setShowBuildResult(false)}
            />
            : <>
                {renderBaseContent()}
                {renderConfirmModal()}
            </>
    }

    return (
        <Page backgroundImage="captain_hall" darkenBackground isLoading={isLoading} error={error}>
            {renderContent()}
        </Page>
    );
};

export default ConstructionPage;