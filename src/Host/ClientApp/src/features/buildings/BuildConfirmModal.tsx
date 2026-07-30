import React from 'react';
import { X } from 'lucide-react';
import { type MyBuilding } from '../../entities/buildings/MyBuilding';
import Title from '../../shared/ui/Title';
import Text from '../../shared/ui/Text';
import Button from '../../shared/ui/buttons/Button';
import Card from '../../shared/ui/Card';

interface BuildConfirmModalProps {
    isOpen: boolean;
    building: MyBuilding | undefined;
    isPrivate: boolean;
    isLoading: boolean;
    onConfirm: (building: MyBuilding, isPrivate: boolean) => void;
    onClose: () => void;
}

// ============================================
// Вспомогательные рендеры
// ============================================
const renderDetailRow = (label: string, value: React.ReactNode, valueClassName?: string) => (
    <div className="flex justify-between text-sm">
        <span className="text-muted">{label}</span>
        <span className={valueClassName || 'text-light'}>{value}</span>
    </div>
);

const renderBuildingData = (building: MyBuilding, isPrivate: boolean) => {
    const buildingData = isPrivate ? building.private : building.state;
    const isAvailable = buildingData.buildAvailable;
    const cost = buildingData.cost;

    return (
        <div className="bg-dark/50 border border-bright/10 rounded-lg p-3 space-y-1.5">
            {renderDetailRow('Тип', isPrivate ? 'Частная' : 'Бюджетная')}
            {renderDetailRow('Стоимость', `${cost} SOL`, 'text-bright font-medium')}
            {renderDetailRow('Уже построено', buildingData.buildingCount)}
            {renderDetailRow(
                'Статус',
                isAvailable ? 'Доступно' : buildingData.unavailabilityReason || 'Недоступно',
                isAvailable ? 'text-good' : 'text-danger'
            )}
        </div>
    );
};

// ============================================
// Основной компонент
// ============================================
const BuildConfirmModal: React.FC<BuildConfirmModalProps> = ({
    isOpen,
    building,
    isPrivate,
    isLoading,
    onConfirm,
    onClose,
}) => {
    if (!isOpen || !building) return null;

    const buildingData = isPrivate ? building.private : building.state;
    const isAvailable = buildingData.buildAvailable;

    const renderHeader = () => (
        <div className="flex items-center justify-between mb-4">
            <Title size="h2">Подтверждение</Title>
            <button
                onClick={onClose}
                className="text-muted hover:text-light transition-colors p-1"
            >
                <X className="w-5 h-5" />
            </button>
        </div>
    );

    const renderMessage = () => (
        <Text variant="secondary" size="sm" align="left">
            Построить <span className="text-light font-medium">{building.name}</span>
            {' '}({isPrivate ? 'частную' : 'бюджетную'})?
        </Text>
    );

    const renderActions = () => (
        <div className="flex gap-2">
            <Button
                variant="primary"
                onClick={() => onConfirm(building, isPrivate)}
                disabled={!isAvailable || isLoading}
                className="flex-1"
            >
                {isLoading ? 'Строительство...' : 'Построить'}
            </Button>
            <Button variant="secondary" onClick={onClose}>
                Отмена
            </Button>
        </div>
    );

    return (
        <div className="fixed inset-0 z-[2000] flex items-center justify-center bg-dark/80 backdrop-blur-sm p-4">
            <Card variant="glow" className="max-w-md w-full">
                {renderHeader()}
                <div className="space-y-4">
                    {renderMessage()}
                    {renderBuildingData(building, isPrivate)}
                    {renderActions()}
                </div>
            </Card>
        </div>
    );
};

export default BuildConfirmModal;