import React, { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { HelpCircle, Clock, X } from 'lucide-react';
import Button from '../shared/ui/buttons/Button';
import InputText from '../shared/ui/InputText';
import Text from '../shared/ui/Text';
import { FlexContainer } from '../shared/ui/FlexContainer';
import PageHeader, { type PageHeaderButton } from '../features/PageHeader';
import GameRequirementUI from '../entities/common/gameRequirements/GameRequirementUI';
import type { Slide, SlideButton } from '../entities/events/colonyEvent.types';
import type { GameRequirement } from '../entities/common/gameRequirements/gameRequirement.types';
import type { GameVisibleEffect } from '../entities/common/gameVisibleEffects/gameVisibleEffect.types';
import GameVisibleEffectUI from '../entities/common/gameVisibleEffects/GameVisibleEffectUI';
import Divider from '../shared/ui/Divider';

interface SlideRendererProps {
    slide: Slide;
    inputTextValue?: string;
    inputTextError?: string;
    onInputTextChange?: (e: React.ChangeEvent<HTMLInputElement>) => void;
    onButtonClick: (button: SlideButton) => void;
    onInfoSlideClick: (slideId: string) => void;
    onNavigate?: (url: string) => void;
    onSlideChange?: (slideId: string) => void;
    renderBottomSlot?: React.ReactNode;
    createdAt?: string;
    canBeClosed?: boolean;
    onClose?: () => void;
    leftButton?: PageHeaderButton;
    resetScrollTrigger?: number | string;
}

const SlideRenderer: React.FC<SlideRendererProps> = ({
    slide,
    inputTextValue,
    inputTextError,
    onInputTextChange,
    onButtonClick,
    onInfoSlideClick,
    onNavigate,
    onSlideChange,
    renderBottomSlot,
    createdAt,
    canBeClosed = true,
    leftButton,
    resetScrollTrigger,
}) => {
    const navigate = useNavigate();
    const scrollContainerRef = React.useRef<HTMLDivElement>(null);

    useEffect(() => {
        if (scrollContainerRef.current) {
            scrollContainerRef.current.scrollTop = 0;
        }
    }, [resetScrollTrigger]);

    const renderRequirements = (requirements: GameRequirement[]) => {
        if (!requirements || requirements.length === 0) return null;
        return <div className='flex flex-col mx-auto w-full gap-0.5'>
                <Divider />
                <Text>Требования:</Text>
            {requirements?.map(requirement => <GameRequirementUI
                requirement={requirement} />)}
        </div>
    }

    const renderParameters = (visibleEffects: GameVisibleEffect[]) => {
        if (!visibleEffects || visibleEffects.length === 0) return null;
        return (
            <div className="w-full">
                <Divider />
                <Text>Результат:</Text>
                <div className="flex flex-col mx-auto w-full gap-0.5"
                >
                    {visibleEffects.map((visibleEffect, index) => (
                        <GameVisibleEffectUI
                            visibleEffect={visibleEffect} 
                            key={visibleEffect.label + index}/>
                    ))}
                </div>
            </div>
        );
    };

    const renderButtons = () => {
        const buttons = slide?.buttons || [];

        return (
            <div className="flex flex-col gap-2 w-full">
                {buttons.map((button: SlideButton, index: number) => {
                    const isMutation = button.action != undefined;

                    const handleClick = () => {
                        if (button.action) {
                            onButtonClick(button);
                        } else if (button.navigate) {
                            if (onNavigate) {
                                onNavigate(button.navigate.actionUrl);
                            } else {
                                navigate(button.navigate.actionUrl);
                            }
                        } else if (button.toSlide && onSlideChange) {
                            onSlideChange(button.toSlide.slideId);
                        }
                    };

                    const needInput = button.action?.type == 'inputCompleted';
                    const disabled = !button.isAvailable || (needInput && (!!inputTextError || (inputTextValue?.length ?? 0) < 2));
                    return (
                        <div key={index} className="flex items-center gap-2">
                            <Button
                                variant={isMutation ? 'primary' : 'secondary'}
                                sizeSm="sm"
                                sizeMd="md"
                                onClick={handleClick}
                                disabled={disabled}
                                className="flex-1"
                            >
                                {button.name}
                            </Button>
                            {button.infoSlideId && (
                                <button
                                    onClick={() => onInfoSlideClick(button.infoSlideId!)}
                                    className="flex-shrink-0 w-10 h-10 rounded-lg border border-bright/20 
                                        text-muted hover:text-light hover:border-bright/40 transition-colors flex items-center justify-center"
                                    aria-label="Подробнее"
                                >
                                    <HelpCircle className="w-4 h-4" />
                                </button>
                            )}
                        </div>
                    );
                })}
            </div>
        );
    };

    const renderBottomPanel = () => {
        return (
            <div className="flex flex-col transition-all duration-300 flex-shrink-0 mt-2 overflow-hidden">
                <div className="flex-shrink-0 py-2 space-y-2 mx-auto w-full">
                    {slide.buttons.some(b => b.action?.type == 'inputCompleted') && (
                        <InputText
                            name="slideInputText"
                            label="Название колонии"
                            type="text"
                            value={inputTextValue ?? ''}
                            handleChange={onInputTextChange ?? (() => { })}
                            handleBlur={onInputTextChange ?? (() => { })}
                            error={!!inputTextError}
                            helperText={inputTextError}
                        />
                    )}

                    {renderButtons()}

                    {renderBottomSlot}

                    {createdAt && (
                        <div className="flex items-center gap-2 pt-1">
                            <Clock className="w-3 h-3 text-muted/30" />
                            <span className="text-[0.55rem] text-muted/30">
                                {createdAt}
                            </span>
                        </div>
                    )}
                </div>
            </div>
        );
    };

    const renderCentralPart = () => {
        return (
            <div className="min-h-full w-full max-w-3xl mx-auto bg-dark/40 backdrop-blur-sm border border-bright/5">
                <div className="relative w-full overflow-hidden">
                    <img
                        src={`/images/pictures/${slide?.imageName}.jpg`}
                        alt={slide.title || 'Иллюстрация'}
                        className="w-full h-auto object-cover object-center"
                    />
                    <div className="absolute inset-0 bg-gradient-to-t from-dark/80 via-dark/20 to-transparent pointer-events-none" />
                </div>

                <div className="p-4">
                    <div className="space-y-2 w-full">
                        {slide?.text.map((item, index) => (
                            <Text key={index} size="sm" align='left' className="leading-relaxed">
                                {item}
                            </Text>
                        ))}
                    </div>
                    {renderRequirements(slide?.requirements ?? [])}
                    {renderParameters(slide?.visibleEffects ?? [])}
                </div>
            </div>
        );
    };

    const rightButton = canBeClosed
        ? { icon: X, onClick: () => navigate(-1), label: 'Закрыть' }
        : undefined;
    return (
        <FlexContainer className='h-full max-w-3xl mx-auto py-4 px-2 md:px-4 pb-2 md:pb-4'>
            <div className="w-full sticky top-0 flex-shrink-0 z-20 border-b border-bright/10">
                <PageHeader
                    title={slide.title || 'Событие'}
                    leftButton={leftButton}
                    rightButton={rightButton}
                />
            </div>

            <div
                ref={scrollContainerRef}
                className="flex-1 w-full overflow-y-auto scrollbar-hide z-10 relative"
            >
                {renderCentralPart()}
            </div>

            <div className="w-full sticky bottom-0 flex-shrink-0 z-20 border-t border-bright/10">
                {renderBottomPanel()}
            </div>
        </FlexContainer>
    );
};

export default SlideRenderer;