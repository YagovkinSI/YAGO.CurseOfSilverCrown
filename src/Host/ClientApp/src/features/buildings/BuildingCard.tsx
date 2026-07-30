import React from 'react';
import {
    Building2,
    HelpCircle,
    CheckCircle,
    Lock,
} from 'lucide-react';
import type { MyBuilding } from '../../entities/buildings/MyBuilding';
import { IsDesktop } from '../MediaHelper';
import Title from '../../shared/ui/Title';
import Text from '../../shared/ui/Text';
import Button from '../../shared/ui/buttons/Button';
import ColonyParameterRowList from '../../features/ColonyParameterList';
import Surface from '../../shared/ui/Surface';

interface BuildingCardProps {
    building: MyBuilding;
    isPrivate: boolean;
    isExpanded: boolean;
    onToggleExpand: (buildingType: string) => void;
    onBuildClick: (building: MyBuilding, isPrivate: boolean) => void;
    isBuilding: boolean;
}

// ============================================
// Вспомогательные рендеры
// ============================================
const renderStatusIcon = (isAvailable: boolean) => {
    if (isAvailable) {
        return <CheckCircle className="w-3 h-3 text-good" />;
    }
    return <Lock className="w-3 h-3 text-muted" />;
};

const renderBuildButtonText = (isPrivate: boolean, isAvailable: boolean, cost: number, isDesktop: boolean, unavailabilityReason: string | undefined) => (
    <div className={`flex items-start w-full ${isDesktop ? 'flex-row' : 'flex-col'}`}>
        <span className="text-xs font-medium">
            {isPrivate ? '👤 Частная' : '🏛️ Бюджетная'}
        </span>
        <span className="text-[0.6rem] opacity-70">
            {isAvailable
                ? `💰 ${cost} SOL`
                : `🔒 ${unavailabilityReason || 'Недоступно'}`
            }
        </span>
    </div>
);

// ============================================
// Основной компонент
// ============================================
const BuildingCard: React.FC<BuildingCardProps> = ({
    building,
    isPrivate,
    isExpanded,
    onToggleExpand,
    onBuildClick,
    isBuilding,
}) => {
    const isDesktop = IsDesktop();
    const buildingData = isPrivate ? building.private : building.state;
    const isAvailable = buildingData.buildAvailable;
    const totalBuilt = building.state.buildingCount + building.private.buildingCount;

    // ===== Шапка карточки =====
    const renderHeader = () => (
        <div
            className="flex items-start justify-between cursor-pointer"
            onClick={() => onToggleExpand(building.type)}
        >
            <div className="flex-1 min-w-0">
                <div className="flex items-center gap-2">
                    <Title size="h3" className="text-light truncate">
                        {building.name}
                    </Title>
                    <span className="text-[0.55rem] flex items-center gap-0.5 flex-shrink-0">
                        {renderStatusIcon(isAvailable)}
                    </span>
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
    );

    // ===== Кнопка "Построить" =====
    const renderBuildButton = () => (
        <div className="flex items-center gap-2">
            <Button
                variant={isAvailable ? 'primary' : 'secondary'}
                sizeSm="sm"
                sizeMd="md"
                onClick={() => onBuildClick(building, isPrivate)}
                disabled={!isAvailable || isBuilding}
                className="flex-1 justify-start text-left"
            >
                {renderBuildButtonText(isPrivate, isAvailable, buildingData.cost, isDesktop, buildingData.unavailabilityReason)}
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

    // ===== Развёрнутая часть =====
    const renderExpandedContent = () => {
        if (!isExpanded) return null;

        return (
            <div className="pt-2 border-t border-bright/10 space-y-2">
                <div className="space-y-0.5">
                    {building.description.map((line, idx) => (
                        <Text key={idx} size="xs" variant="secondary" align="left" className="leading-relaxed">
                            • {line}
                        </Text>
                    ))}
                </div>

                {buildingData.bonuses && buildingData.bonuses.length > 0 && (
                    <div className="w-full">
                        <ColonyParameterRowList items={buildingData.bonuses} dense={true} />
                    </div>
                )}

                <div className="flex flex-col gap-1.5 pt-1">
                    {renderBuildButton()}
                </div>
            </div>
        );
    };

    return (
        <Surface
            rounded="md"
            variant="default"
            className={`
                w-full p-3 gap-2 transition-all duration-200
                ${isAvailable ? 'hover:border-bright/30' : 'opacity-70'}
                ${isExpanded ? 'border-bright/30' : ''}
            `}
        >
            {renderHeader()}
            {renderExpandedContent()}
        </Surface>
    );
};

export default BuildingCard;