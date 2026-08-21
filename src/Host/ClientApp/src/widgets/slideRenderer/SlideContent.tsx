import React from 'react';
import Text from '../../shared/ui/Text';
import GameRequirementUI from '../../entities/common/gameRequirements/GameRequirementUI';
import GameVisibleEffectUI from '../../entities/common/gameVisibleEffects/GameVisibleEffectUI';
import type { Slide } from '../../entities/events/colonyEvent.types';

interface SlideContentProps {
    slide: Slide;
}

const SlideContent: React.FC<SlideContentProps> = ({ slide }) => {
    const renderParameterRows = <T,>(
        title: string,
        items: T[] | undefined,
        renderItem: (item: T, index: number) => React.ReactNode,
    ) => {
        if (!items || items.length === 0) return null;
        return (
            <div className="w-full max-w-md mx-auto py-4">
                <Text align='left'>{title}</Text>
                <div className="flex flex-col mx-auto w-full gap-0.5">
                    {items.map((item, index) => renderItem(item, index))}
                </div>
            </div>
        );
    };

    const renderImage = () => (
        <div className="relative w-full overflow-hidden">
            <img
                src={`/images/pictures/${slide.imageName}.jpg`}
                alt={slide.title || 'Иллюстрация'}
                className="w-full h-auto object-cover object-center"
            />
            <div className="absolute inset-0 bg-gradient-to-t from-dark/80 via-dark/20 to-transparent pointer-events-none" />
        </div>
    );

    const renderText = () => (
        <div className="space-y-2 w-full">
            {slide.text.map((item, index) => (
                <Text key={index} size="sm" align='left' className="leading-relaxed">
                    {item}
                </Text>
            ))}
        </div>
    );

    const renderRequirements = () => renderParameterRows(
        'Требования:',
        slide.requirements,
        (requirement, index) => <GameRequirementUI
            key={requirement.label + index}
            requirement={requirement} />
    );

    const renderEffects = () => renderParameterRows(
        'Результат:',
        slide.visibleEffects,
        (visibleEffect, index) => <GameVisibleEffectUI
            key={visibleEffect.label + index}
            visibleEffect={visibleEffect} />
    );

    return (
        <div className="min-h-full w-full max-w-3xl mx-auto bg-dark/40 backdrop-blur-sm border border-bright/5">
            {renderImage()}
            <div className="p-4">
                {renderText()}
                {renderRequirements()}
                {renderEffects()}
            </div>
        </div>
    );
};

export default SlideContent;